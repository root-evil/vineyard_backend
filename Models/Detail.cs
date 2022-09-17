
namespace vineyard_backend.Models
{
    public class Detail
    {
        public int id { get; set; }
        public string date { get; set; }
        public double? tavg { get; set; }
        public double? tmax { get; set; }
        public double? tmin { get; set; }
        public double? pavg { get; set; }
        public double? pmax { get; set; }
        public double? pmin { get; set; }
        public int paramId { get; set; }
        public Param Param { get; set; }
    }
}