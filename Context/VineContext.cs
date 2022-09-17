using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using vineyard_backend.Configuration;
using vineyard_backend.Models;

namespace vineyard_backend.Context
{

    public partial class VineContext : DbContext
    {
        public VineContext(DbContextOptions<VineContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Region> Regions { get; set; }
        public virtual DbSet<Polygon> Polygons { get; set; }
        public virtual DbSet<Marker> Markers { get; set; }
        public virtual DbSet<Param> Prams { get; set; }
        public virtual DbSet<Detail> Details { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("public");

            modelBuilder.ApplyConfiguration(new RegionConfiguration());
            modelBuilder.ApplyConfiguration(new PolygonConfiguration());
            modelBuilder.ApplyConfiguration(new MarkerConfiguration());
            modelBuilder.ApplyConfiguration(new ParamConfiguration());
            modelBuilder.ApplyConfiguration(new DetailConfiguration());
        }
    }
}
