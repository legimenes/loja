using System;

namespace Loja.Core.Identity.Models
{
    public class RefreshToken
    {
        public string Token { get; set; }
        public long UserId { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}