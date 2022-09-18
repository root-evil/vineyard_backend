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
                .HasColumnName("id");

            entity.Property(e => e.center)
                .HasColumnName("area");

            entity.Property(e => e.freeArea)
                .HasColumnName("free_area");

            entity.Property(e => e.center)
                .HasColumnName("center");

            entity.Property(e => e.bounds)
                .HasColumnName("bounds");

            entity.Property(e => e.geo)
                .HasColumnName("geo");

            entity.Property(e => e.regionId)
                .HasColumnName("region_id");

            entity.Property(e => e.paramId)
                .HasColumnName("param_id");
                
            entity.HasOne(e => e.Region)
                .WithMany(p => p.Polygons)
                .HasForeignKey(d => d.regionId)
                .HasConstraintName("region_id")
                .IsRequired(false);
        }
    }
}