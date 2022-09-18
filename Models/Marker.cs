using System.Text.Json;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

using vineyard_backend.Converters;

namespace vineyard_backend.Models
{
    public class Marker
    {
        public int id { get; set; }
        
        public int regionId { get; set; }

        public double? scoring { get; set; }

        [JsonConverter(typeof(CoordinatConverter))]
        public double[]? center { get; set; }

        public int paramId { get; set; }

        [NotMapped]
        public Param? Param { get; set; }

        [NotMapped]
        public Region? Region { get; set; }
    }
}