using System.Text.Json;
using System.Text.Json.Serialization;

using vineyard_backend.Converters;

namespace vineyard_backend.Models
{
    public class Polygon
    {
        public int id { get; set; }
        
        public int regionId { get; set; }

        public double scoring { get; set; }

        public double[] center { get; set; }

        public double width { get; set; }

        public double height { get; set; }
        
        public double[] longitude { get; set; }

        public double[] latitude { get; set; }

        public virtual Param Param { get; set; }

        public Region Region { get; set; }
    }
}