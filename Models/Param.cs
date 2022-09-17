using System.Runtime.Serialization;

namespace vineyard_backend.Models
{
    public class Param
    {
        public int id { get; set; }
        public double? scoring { get; set; }
        public double? min_relief_aspect { get; set; }
        public double? max_relief_aspect { get; set; }
        public double? avg_relief_aspect { get; set; }
        public double? min_relief_height { get; set; }
        public double? max_relief_height { get; set; }
        public double? avg_relief_height { get; set; }
        public double? min_relief_slope { get; set; }
        public double? max_relief_slope { get; set; }
        public double? avg_relief_slope { get; set; }
        public double? avg_sunny_days { get; set; }
        public double? mix_sunny_days { get; set; }
        public double? man_sunny_days { get; set; }
        public int? water_seasonlyty { get; set; }
        public FloodedMonths? floodedMonths { get; set; }
        public bool? forrest { get; set; }
        public Soil? soil { get; set; }
        public virtual Polygon? Polygon { get; set; }
        public virtual Marker? Marker { get; set; }
        public virtual ICollection<Detail> Details { get; set; }

        [IgnoreDataMember]
        public IEnumerable<Polygon> BetterNearPolygons { get; set; }

        [IgnoreDataMember]
        public IEnumerable<Polygon> WorseNearPolygons { get; set; }
    }
}