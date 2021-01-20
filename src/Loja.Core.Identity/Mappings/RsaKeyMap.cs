using Loja.Core.Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Loja.Core.Identity.Mappings
{
    public class RsaKeyMap : IEntityTypeConfiguration<RsaKey>
    {
        public void Configure(EntityTypeBuilder<RsaKey> builder)
        {
            builder.ToTable("rsakeys");
            builder.HasNoKey();
        }
    }
}