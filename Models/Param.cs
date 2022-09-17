
namespace vineyard_backend.Models
{
    public class Param
    {
        public int id { get; set; }
        public float min_relief_aspect { get; set; }
        public float max_relief_aspect { get; set; }
        public float avg_relief_aspect { get; set; }
        public float min_relief_height { get; set; }
        public float max_relief_height { get; set; }
        public float avg_relief_height { get; set; }
        public float avg_relief_slope { get; set; }
        public float avg_sunny_days { get; set; }
        public float mix_sunny_days { get; set; }
        public float man_sunny_days { get; set; }
        public float water_seasonlyty { get; set; }
        public int polygonId { get; set; }
        public virtual Polygon Polygon { get; set; }
        public virtual ICollection<Detail> Details { get; set; }
    }
}