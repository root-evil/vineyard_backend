using System.Text.Json;
using System.Text.Json.Serialization;

namespace vineyard_backend.Converters
{
    public class TwoDArrayConverter<T> : JsonConverter<T[,]>
    {
        public override T[,] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            throw new NotImplementedException();
        }
        public override void Write(Utf8JsonWriter writer, T[,] val, JsonSerializerOptions options) {
            if((val?.Length ?? 0) == 0)
            {
                writer.WriteNullValue();
                return;
            }
            writer.WriteStartArray();
            var xLen = val.Length / val.Rank;
            var yLen = val.Rank;
            for(int i = 0; i < xLen; i++)
            {
                writer.WriteStartArray();
                for(int j = 0; j < yLen; j++)
                {
                    writer.WriteRawValue(JsonSerializer.Serialize(val[i, j]));
                }
                writer.WriteEndArray();
            }
            writer.WriteEndArray();
        }
    }
}