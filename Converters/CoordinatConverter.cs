using System.Text.Json;
using System.Text.Json.Serialization;
using System;

namespace vineyard_backend.Converters
{
    public class CoordinatConverter : JsonConverter<double[]>
    {
        public override double[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            throw new NotImplementedException();
        }
        public override void Write(Utf8JsonWriter writer, double[] val, JsonSerializerOptions options) {
            if((val?.Length ?? 0) != 2)
            {
                writer.WriteNullValue();
                return;
            }
            
            writer.WriteStartArray();
            writer.WriteNumberValue(val[0]);
            writer.WriteNumberValue(val[1]);
            writer.WriteEndArray();
        }
    }
}