
namespace vineyard_backend.Models
{
    public class Detail
    {
        public int id { get; set; }
        public string date { get; set; }
        public decimal tavg { get; set; }
        public decimal tmax { get; set; }
        public decimal tmin { get; set; }
        public decimal pavg { get; set; }
        public decimal pmax { get; set; }
        public decimal pmin { get; set; }
        public int paramId { get; set; }
        public Param Param { get; set; }
    }
}