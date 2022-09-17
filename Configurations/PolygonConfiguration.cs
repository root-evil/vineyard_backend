using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using vineyard_backend.Models;

namespace vineyard_backend.Configuration
{
    public class PolygonConfiguration : IEntityTypeConfiguration<Polygon>
    {
        public void Configure(EntityTypeBuilder<Polygon> entity)
        {
            entity.ToTable("polygon", "public");

            entity.HasKey(e => e.id)
                .HasName("id");

            entity.Property(e => e.id)
                .IsRequired()
                .HasColumnName("id")
                .HasMaxLength(36);

            entity.Property(e => e.regionId)
                .IsRequired()
                .HasColumnName("region_id")
                .HasMaxLength(36);

            entity.Property(e => e.center)
                .HasColumnType("float[]")
                .HasColumnName("center");

            entity.Property(e => e.height)
                .HasPrecision(20, 8)
                .HasColumnName("height");

            entity.Property(e => e.width)
                .HasPrecision(20, 8)
                .HasColumnName("width");

            entity.Property(e => e.longitude)
                .HasColumnName("longitude");

            entity.Property(e => e.latitude)
                .HasColumnName("latitude");

            entity.HasOne(e => e.Region)
                .WithMany(p => p.Polygons)
                .HasForeignKey(d => d.regionId);

        }
    }
}