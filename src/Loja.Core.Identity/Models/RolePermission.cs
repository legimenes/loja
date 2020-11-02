namespace Loja.Core.Identity.Models
{
    public class RolePermission
    {
        public long Id { get; set; }
        public long RoleId { get; set; }
        public long PermissionId { get; set; }
        public string Value1 { get; set; }

        public virtual Role Role { get; set; }
        public virtual Permission Permission { get; set; }

        public RolePermission() { }
    }
}