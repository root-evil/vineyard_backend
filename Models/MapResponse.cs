
namespace vineyard_backend.Models
{
    public class MapResponse
    {
        public double[] center { get; set; }
        public IEnumerable<Polygon> Polygons { get; set; }
        public IEnumerable<Marker> Markers { get; set; }
    }
}