using System.Text.Json.Serialization;
using vineyard_backend.Converters;

namespace vineyard_backend.Models
{
    public class MapResponse
    {
        [JsonConverter(typeof(CoordinatConverter))]
        public double[] center { get; set; }
        public IEnumerable<Polygon> Polygons { get; set; }
        public IEnumerable<Marker> Markers { get; set; }
    }
}