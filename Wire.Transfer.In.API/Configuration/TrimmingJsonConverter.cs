using System;
using Newtonsoft.Json;

namespace Wire.Transfer.In.API.Configuration
{
    /// <summary>
    ///     Helper trimming strings in command model state
    /// </summary>
    /// <example>
    ///     Before:
    ///     {
    ///     "name": "  test  "
    ///     }
    ///     After:
    ///     {
    ///     "name": "test"
    ///     }
    /// </example>
    public class TrimmingJsonConverter : JsonConverter
    {
        public override bool CanRead => true;
        public override bool CanWrite => false;

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            return ((string) reader.Value)?.Trim();
        }
    }
}