
namespace vineyard_backend.Models
{
    public class MapResponse<T>
    {
        public double[] center { get; set; }
        public T data { get; set; }
    }
}