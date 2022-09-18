using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using vineyard_backend.Models;

namespace vineyard_backend.Configuration
{
    public class MarkerConfiguration : IEntityTypeConfiguration<Marker>
    {
        public void Configure(EntityTypeBuilder<Marker> entity)
        {
            entity.ToTable("markers", "public");

            entity.HasKey(e => e.id)
                .HasName("id");

            entity.Property(e => e.id)
                .IsRequired()
                .HasColumnName("id")
                .HasMaxLength(36);

            entity.Property(e => e.center)
                .HasColumnName("center");

            entity.Property(e => e.bounds)
                .HasColumnName("bounds");

            entity.Property(e => e.regionId)
                .HasColumnName("region_id");

            entity.Property(e => e.paramId)
                .HasColumnName("param_id");
                
            entity.HasOne(e => e.Region)
                .WithMany(p => p.Markers)
                .HasForeignKey(d => d.regionId)
                .HasConstraintName("region_id")
                .IsRequired(false);
        }
    }
}