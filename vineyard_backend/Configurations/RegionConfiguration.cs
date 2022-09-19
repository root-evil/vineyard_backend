using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using vineyard_backend.Models;

namespace vineyard_backend.Configuration
{
    public class RegionConfiguration : IEntityTypeConfiguration<Region>
    {
        public void Configure(EntityTypeBuilder<Region> entity)
        {
            entity.ToTable("regions", "public");

            entity.HasKey(e => e.id)
                .HasName("id");

            entity.Property(e => e.name)
                .IsRequired()
                .HasColumnName("name")
                .HasMaxLength(64);

            entity.Property(e => e.center)
                .HasColumnName("center");

            entity.Property(e => e.bounds)
                .HasColumnName("bounds");
                
        }
    }
}