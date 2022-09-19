using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

namespace vineyard_backend.Models
{
    public class Detail
    {
        public int id { get; set; }
        
        [JsonIgnore]
        public int? monthId { get; set; }
        public double? tavg { get; set; }
        public double? tmax { get; set; }
        public double? tmin { get; set; }
        public double? pavg { get; set; }
        public double? pmax { get; set; }
        public double? pmin { get; set; }
        
        [JsonIgnore]
        public int paramId { get; set; }

        [NotMapped]
        public Param? Param { get; set; }

        [NotMapped]
        public Month? Month => (Month?) monthId;
    }
}