using System;

namespace Loja.Core.Identity.Models
{
    public class User
    {
        public long Id { get; set; }
        public long? RoleId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string ConcurrencyStamp { get; set; }
        public DateTime CreationDate { get; set; }
        public bool Active { get; set; }

        public virtual Role Role { get; set; }

        public User() { }
    }
}