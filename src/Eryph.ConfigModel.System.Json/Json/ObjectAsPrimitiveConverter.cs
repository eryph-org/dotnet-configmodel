using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Eryph.ConfigModel.Json
{
    public sealed class ObjectAsPrimitiveConverter : JsonConverter<object>
    {
        public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
        {
            throw new NotSupportedException();
            //if (value.GetType() == typeof(object))
            //{
            //    writer.WriteStartObject();
            //    writer.WriteEndObject();
            //}
            //else
            //{
            //    JsonSerializer.Serialize(writer, value, value.GetType(), options);
            //}
        }

        public override object Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.Null:
                    return null;
                case JsonTokenType.False:
                    return false;
                case JsonTokenType.True:
                    return true;
                case JsonTokenType.String:
                    return reader.GetString();
                case JsonTokenType.Number:
                {
                    if (reader.TryGetInt32(out var i))
                        return i;
                    if (reader.TryGetInt64(out var l))
                        return l;
                    if(reader.TryGetDouble(out var d))
                        return d;

                    using (var doc = JsonDocument.ParseValue(ref reader))
                    {
                        throw new JsonException($"Cannot parse number {doc.RootElement}");
                    }
                }
                case JsonTokenType.StartArray:
                {
                    var list = new List<object>();
                    while (reader.Read())
                    {
                        switch (reader.TokenType)
                        {
                            case JsonTokenType.None:
                            case JsonTokenType.StartObject:
                            case JsonTokenType.EndObject:
                            case JsonTokenType.StartArray:
                            case JsonTokenType.PropertyName:
                            case JsonTokenType.Comment:
                            case JsonTokenType.String:
                            case JsonTokenType.Number:
                            case JsonTokenType.True:
                            case JsonTokenType.False:
                            case JsonTokenType.Null:
                            default:
                                list.Add(Read(ref reader, typeof(object), options));
                                break;
                            case JsonTokenType.EndArray:
                                return list;
                        }
                    }
                    throw new JsonException();
                }
                case JsonTokenType.StartObject:
                    var dict = CreateDictionary();
                    while (reader.Read())
                    {
                        switch (reader.TokenType)
                        {
                            case JsonTokenType.EndObject:
                                return dict;
                            case JsonTokenType.PropertyName:
                                var key = reader.GetString();
                                reader.Read();
                                if (key != null) dict.Add(key, Read(ref reader, typeof(object), options));
                                break;
                            case JsonTokenType.None:
                            case JsonTokenType.StartObject:
                            case JsonTokenType.StartArray:
                            case JsonTokenType.EndArray:
                            case JsonTokenType.Comment:
                            case JsonTokenType.String:
                            case JsonTokenType.Number:
                            case JsonTokenType.True:
                            case JsonTokenType.False:
                            case JsonTokenType.Null:
                            default:
                                throw new JsonException();
                        }
                    }
                    throw new JsonException();
                default:
                    throw new JsonException($"Unknown token {reader.TokenType}");
            }
        }

        private IDictionary<object, object> CreateDictionary() => new Dictionary<object, object>();
    }
}