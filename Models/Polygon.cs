using System.Text.Json;
using System.Text.Json.Serialization;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

using vineyard_backend.Converters;

namespace vineyard_backend.Models
{
    public class Polygon
    {
        public int id { get; set; }

        public double? scoring { get; set; }

        public long area { get; set; }

        public long freeArea { get; set; }

        [JsonConverter(typeof(CoordinatConverter))]
        public double[] center { get; set; }

        public double width { get; set; }

        public double height { get; set; }


        [JsonConverter(typeof(TwoDArrayConverter<double>))]
        public double[,] geo { get; set; }

        public int? paramId { get; set; }

        [NotMapped]
        public virtual Param? Param { get; set; }

        public int? regionId { get; set; }

        [NotMapped]
        public virtual Region? Region { get; set; }
    }
}