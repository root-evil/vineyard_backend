using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

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
        public double? min_sunny_days { get; set; }
        public double? max_sunny_days { get; set; }
        public int? water_seasonlyty { get; set; }
        public bool? forest { get; set; }
        public string? floodedMonthsId { get; set; }
        public string? soilId { get; set; }

        [NotMapped]
        public virtual Polygon? Polygon { get; set; }
        [NotMapped]
        public virtual Marker? Marker { get; set; }
        
        [NotMapped]
        public FloodedMonths? floodedMonths => Enum.Parse<FloodedMonths>(floodedMonthsId ?? "No");

        [NotMapped]
        public Soil? soil => soilId == null ? null : Enum.Parse<Soil>(soilId);
        
        public virtual ICollection<Detail> Details { get; set; }

        [NotMapped]
        public IEnumerable<Polygon> BetterNearPolygons { get; set; }

        [NotMapped]
        public IEnumerable<Polygon> WorseNearPolygons { get; set; }
    }
}