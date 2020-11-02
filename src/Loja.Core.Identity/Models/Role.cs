namespace Loja.Core.Identity.Models
{
    public class Role
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string ConcurrencyStamp { get; set; }

        public Role() { }
    }
}