using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using vineyard_backend.Models;

namespace vineyard_backend.Configuration
{
    public class DetailConfiguration : IEntityTypeConfiguration<Detail>
    {
        public void Configure(EntityTypeBuilder<Detail> entity)
        {
            entity.ToTable("details", "public");

            entity.HasKey(e => e.id)
                .HasName("id");

            entity.Property(e => e.id)
                .IsRequired()
                .HasColumnName("id");

            entity.Property(e => e.date)
                .HasColumnName("date");
            entity.Property(e => e.tavg)
                .HasColumnName("tavg");
            entity.Property(e => e.tmax)
                .HasColumnName("tmax");
            entity.Property(e => e.tmin)
                .HasColumnName("tmin");
            entity.Property(e => e.pavg)
                .HasColumnName("pavg");
            entity.Property(e => e.pmax)
                .HasColumnName("pmax");
            entity.Property(e => e.pmin)
                .HasColumnName("pmin");
            entity.Property(e => e.paramId)
                .HasColumnName("params_id");

            entity.HasOne(e => e.Param)
                .WithMany(p => p.Details)
                .HasForeignKey(d => d.paramId)
                .HasConstraintName("params_id");
        }
    }
}