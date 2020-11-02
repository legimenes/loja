using Loja.Core.Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Loja.Core.Identity.Mappings
{
    public class RoleMap : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("roles");
            builder.HasKey(m => m.Id);
            builder.Property(m => m.Name);
            builder.Property(m => m.ConcurrencyStamp);
        }
    }
}