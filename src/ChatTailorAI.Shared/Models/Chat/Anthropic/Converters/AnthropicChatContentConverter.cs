using ChatTailorAI.Shared.Models.Chat.Anthropic.Content;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace ChatTailorAI.Shared.Models.Chat.Anthropic.Converters
{
    public class AnthropicChatContentConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(List<IAnthropicChatContent>);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JArray contentArray = JArray.Load(reader);
            List<IAnthropicChatContent> contents = new List<IAnthropicChatContent>();

            foreach (var item in contentArray)
            {
                var type = item["type"].Value<string>();
                switch (type)
                {
                    case "text":
                        contents.Add(item.ToObject<AnthropicTextContentPart>(serializer));
                        break;
                    case "image":
                        contents.Add(item.ToObject<AnthropicImageContentPart>(serializer));
                        break;
                    default:
                        throw new Exception("Unknown content type");
                }
            }
            return contents;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException("Unnecessary because CanWrite is false. The type will skip the converter.");
        }

        public override bool CanWrite => false;
    }
}