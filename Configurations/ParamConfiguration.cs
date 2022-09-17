using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using vineyard_backend.Models;

namespace vineyard_backend.Configuration
{
    public class ParamConfiguration : IEntityTypeConfiguration<Param>
    {
        public void Configure(EntityTypeBuilder<Param> entity)
        {
            entity.ToTable("params", "public");

            entity.HasKey(e => e.id)
                .HasName("id");

            entity.Property(e => e.id)
                .IsRequired()
                .HasColumnName("id");
                
            entity.Property(e => e.scoring)
                .HasColumnName("scoring");
            entity.Property(e => e.floodedMonthsId)
                .HasColumnName("flooded");
            entity.Property(e => e.soilId)
                .HasColumnName("soil");
            entity.Property(e => e.min_relief_aspect)
                .HasColumnName("min_relief_aspect");
            entity.Property(e => e.max_relief_aspect)
                .HasColumnName("max_relief_aspect");
            entity.Property(e => e.avg_relief_aspect)
                .HasColumnName("avg_relief_aspect");
            entity.Property(e => e.min_relief_height)
                .HasColumnName("min_relief_height");
            entity.Property(e => e.max_relief_height)
                .HasColumnName("max_relief_height");
            entity.Property(e => e.avg_relief_height)
                .HasColumnName("avg_relief_height");
            entity.Property(e => e.avg_relief_slope)
                .HasColumnName("avg_relief_slope");
            entity.Property(e => e.min_relief_slope)
                .HasColumnName("min_relief_slope");
            entity.Property(e => e.max_relief_slope)
                .HasColumnName("max_relief_slope");
            entity.Property(e => e.avg_sunny_days)
                .HasColumnName("avg_sunny_days");
            entity.Property(e => e.mix_sunny_days)
                .HasColumnName("mix_sunny_days");
            entity.Property(e => e.man_sunny_days)
                .HasColumnName("man_sunny_days");
            entity.Property(e => e.water_seasonlyty)
                .HasColumnName("water_seasonlyty");

            entity.HasOne(e => e.Polygon)
                .WithOne(p => p.Param)
                .HasForeignKey<Polygon>(d => d.paramId)
                .HasConstraintName("param_id");

            entity.HasOne(e => e.Marker)
                .WithOne(p => p.Param)
                .HasForeignKey<Marker>(d => d.paramId)
                .HasConstraintName("param_id");

        }
    }
}