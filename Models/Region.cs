using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;
using vineyard_backend.Converters;

namespace vineyard_backend.Models
{
    public class Region
    {
        public int id { get; set; }
        public string? name { get; set; }
        public double width { get; set; }
        public double height { get; set; }

        [JsonConverter(typeof(CoordinatConverter))]
        public double[]? center { get; set; }


        [NotMapped]
        public virtual ICollection<Polygon>? Polygons { get; set; }

        [NotMapped]
        public virtual ICollection<Marker>? Markers { get; set; }
    }
}