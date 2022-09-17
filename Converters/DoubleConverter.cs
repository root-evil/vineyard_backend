using System.Text.Json;
using System.Text.Json.Serialization;
using System;

namespace vineyard_backend.Converters
{
    public class DoubleConverter : JsonConverter<double>
    {
        public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            throw new NotImplementedException();
        }
        public override void Write(Utf8JsonWriter writer, double val, JsonSerializerOptions options) {
            writer.WriteNumberValue(Decimal.Round(Convert.ToDecimal(val), 3));
        }
    }
}