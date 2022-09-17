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

            entity.Property(e => e.regionId)
                .IsRequired()
                .HasColumnName("region_id")
                .HasMaxLength(36);

            entity.Property(e => e.paramId)
                .HasColumnName("param_id");

            entity.Property(e => e.center)
                .HasColumnType("float[]")
                .HasColumnName("center");

            entity.HasOne(e => e.Region)
                .WithMany(p => p.Markers)
                .HasForeignKey(d => d.regionId);

            entity.HasOne(e => e.Param)
                .WithOne(p => p.Marker)
                .HasForeignKey<Marker>(d => d.paramId)
                .HasConstraintName("param_id");

        }
    }
}