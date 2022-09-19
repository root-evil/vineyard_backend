using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;
using vineyard_backend.Converters;

namespace vineyard_backend.Models
{
    public class Region
    {
        public int id { get; set; }
        public string? name { get; set; }

        [JsonConverter(typeof(CoordinatConverter))]
        public double[]? center { get; set; }

        [JsonConverter(typeof(TwoDArrayConverter<double>))]
        public double[,]? bounds { get; set; }

        [NotMapped]
        public virtual ICollection<Polygon>? Polygons { get; set; }

        [NotMapped]
        public virtual ICollection<Marker>? Markers { get; set; }
    }
}