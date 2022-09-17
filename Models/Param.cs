
namespace vineyard_backend.Models
{
    public class Param
    {
        public int id { get; set; }
        public double min_relief_aspect { get; set; }
        public double max_relief_aspect { get; set; }
        public double avg_relief_aspect { get; set; }
        public double min_relief_height { get; set; }
        public double max_relief_height { get; set; }
        public double avg_relief_height { get; set; }
        public double avg_relief_slope { get; set; }
        public double avg_sunny_days { get; set; }
        public double mix_sunny_days { get; set; }
        public double man_sunny_days { get; set; }
        public double water_seasonlyty { get; set; }
        public int polygonId { get; set; }
        public virtual Polygon Polygon { get; set; }
        public virtual ICollection<Detail> Details { get; set; }
    }
}