using System.Text.Json;
using System.Text.Json.Serialization;
using System;

namespace vineyard_backend.Converters
{
    public class TwoDArrayConverter<T> : JsonConverter<T[,]>
    {
        public override T[,] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            throw new NotImplementedException();
        }
        public override void Write(Utf8JsonWriter writer, T[,] val, JsonSerializerOptions options) {
            if(val == null)
                writer.WriteNullValue();
            writer.WriteStartArray();
            for(int i = 0; i < val.Length; i++)
            {
                writer.WriteStartArray();
                for(int j = 0; j < val.LongLength; i++)
                {
                    writer.WriteNumberValue(0);
                }
                writer.WriteEndArray();
            }
            writer.WriteEndArray();
        }
    }
}