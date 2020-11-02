namespace Loja.Core.Identity.Models
{
    public class RoleMenu
    {
        public long Id { get; set; }
        public long RoleId { get; set; }
        public long MenuId { get; set; }

        public virtual Role Role { get; set; }
        public virtual Menu Menu { get; set; }

        public RoleMenu() { }
    }
}