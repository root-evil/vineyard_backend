using System.Text.Json;
using System.Text.Json.Serialization;

using vineyard_backend.Converters;

namespace vineyard_backend.Models
{
    public class Marker
    {
        public int id { get; set; }
        
        public int regionId { get; set; }
        
        public int paramId { get; set; }

        public double scoring { get; set; }

        public double[] center { get; set; }

        public Param Param { get; set; }

        public Region Region { get; set; }
    }
}