using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Loja.Core.DomainObjects
{
    public abstract class Entity
    {
        public long Id { get; set; }

        public override bool Equals(object obj)
        {
            var compareTo = obj as Entity;

            if (ReferenceEquals(this, compareTo)) return true;
            if (ReferenceEquals(null, compareTo)) return false;

            return CompareObjects(this, obj);
        }
        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 907) + Id.GetHashCode();
        }
        public static bool operator ==(Entity a, Entity b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }
        public static bool operator !=(Entity a, Entity b)
        {
            return !(a == b);
        }

        private static bool CompareEnumerations(object value1, object value2, string[] ignorePropertiesList)
        {
            if (value1 == null && value2 != null || value1 != null && value2 == null)
                return false;
            else if (value1 != null && value2 != null)
            {
                IEnumerable<object> enumValue1, enumValue2;
                enumValue1 = ((IEnumerable)value1).Cast<object>();
                enumValue2 = ((IEnumerable)value2).Cast<object>();

                if (enumValue1.Count() != enumValue2.Count())
                    return false;
                else
                {
                    object enumValue1Item, enumValue2Item;
                    Type enumValue1ItemType;
                    for (int itemIndex = 0; itemIndex < enumValue1.Count(); itemIndex++)
                    {
                        enumValue1Item = enumValue1.ElementAt(itemIndex);
                        enumValue2Item = enumValue2.ElementAt(itemIndex);
                        enumValue1ItemType = enumValue1Item.GetType();
                        if (IsAssignableFrom(enumValue1ItemType) || IsPrimitiveType(enumValue1ItemType) || IsValueType(enumValue1ItemType))
                        {
                            if (!CompareValues(enumValue1Item, enumValue2Item))
                                return false;
                        }
                        else if (!CompareObjects(enumValue1Item, enumValue2Item, ignorePropertiesList))
                            return false;
                    }
                }
            }
            return true;
        }
        private static bool CompareObjects(object object1, object object2, string[] ignorePropertiesList = null)
        {
            bool areObjectsEqual = true;

            if (object1 == null && object2 == null)
                areObjectsEqual = true;
            else if (object1 != null && object2 != null)
            {
                object value1, value2;
                
                PropertyInfo[] properties = object1.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (PropertyInfo propertyInfo in properties)
                {
                    if (!propertyInfo.CanRead || (ignorePropertiesList != null && ignorePropertiesList.Contains(propertyInfo.Name)))
                        continue;

                    value1 = propertyInfo.GetValue(object1, null);
                    value2 = propertyInfo.GetValue(object2, null);

                    if (IsAssignableFrom(propertyInfo.PropertyType) || IsPrimitiveType(propertyInfo.PropertyType) || IsValueType(propertyInfo.PropertyType))
                    {
                        if (!CompareValues(value1, value2))
                        {
                            areObjectsEqual = false;
                        }
                    }
                    else if (IsEnumerableType(propertyInfo.PropertyType))
                    {
                        if (!CompareEnumerations(value1, value2, ignorePropertiesList))
                            areObjectsEqual = false;
                    }
                    else if (propertyInfo.PropertyType.IsClass)
                    {
                        if (!CompareObjects(propertyInfo.GetValue(object1, null), propertyInfo.GetValue(object2, null), ignorePropertiesList))
                        {
                            areObjectsEqual = false;
                        }
                    }
                    else
                    {
                        areObjectsEqual = false;
                    }
                }
            }
            else
                areObjectsEqual = false;

            return areObjectsEqual;
        }
        private static bool CompareValues(object value1, object value2)
        {
            bool areValuesEqual = true;
            IComparable selfValueComparer = value1 as IComparable;

            if (value1 == null && value2 != null || value1 != null && value2 == null)
                areValuesEqual = false;
            else if (selfValueComparer != null && selfValueComparer.CompareTo(value2) != 0)
                areValuesEqual = false;
            else if (!object.Equals(value1, value2))
                areValuesEqual = false;

            return areValuesEqual;
        }

        private static bool IsAssignableFrom(Type type)
        {
            return typeof(IComparable).IsAssignableFrom(type);
        }
        private static bool IsEnumerableType(Type type)
        {
            return (typeof(IEnumerable).IsAssignableFrom(type));
        }
        private static bool IsPrimitiveType(Type type)
        {
            return type.IsPrimitive;
        }
        private static bool IsValueType(Type type)
        {
            return type.IsValueType;
        }
    }
}