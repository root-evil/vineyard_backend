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
        public bool? forest { get; set; }
        public string? floodedMonthsId { get; set; }
        public string? soilId { get; set; }

        [IgnoreDataMember]
        public virtual Polygon? Polygon { get; set; }
        [IgnoreDataMember]
        public Marker? Marker { get; set; }
        
        [IgnoreDataMember]
        public FloodedMonths? floodedMonths => Enum.Parse<FloodedMonths>(floodedMonthsId ?? "No");

        [IgnoreDataMember]
        public Soil? soil => soilId == null ? null : Enum.Parse<Soil>(soilId);
        
        public virtual ICollection<Detail> Details { get; set; }

        [IgnoreDataMember]
        public IEnumerable<Polygon> BetterNearPolygons { get; set; }

        [IgnoreDataMember]
        public IEnumerable<Polygon> WorseNearPolygons { get; set; }
    }
}