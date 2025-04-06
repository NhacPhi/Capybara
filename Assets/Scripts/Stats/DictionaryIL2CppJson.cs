using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class DictionaryIL2CppJson<TKey, TValue> : JsonConverter<Dictionary<TKey, TValue>>
{
    public override void WriteJson(JsonWriter writer, Dictionary<TKey, TValue> value, JsonSerializer serializer)
    {
        if (value == null)
        {
            writer.WriteNull();
            return;
        }

        serializer.Serialize(writer, value);
    }

    public override Dictionary<TKey, TValue> ReadJson(JsonReader reader, Type objectType, Dictionary<TKey, TValue> existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
        {
            return null;
        }

        var jsonObject = JObject.Load(reader);
        var result = new Dictionary<TKey, TValue>();

        foreach (var property in jsonObject.Properties())
        {
            var key = (TKey)Convert.ChangeType(property.Name, typeof(TKey));
            var value = property.Value.ToObject<TValue>(serializer);
            result[key] = value;
        }

        return result;
    }
}