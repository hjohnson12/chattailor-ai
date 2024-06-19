using ChatTailorAI.Shared.Models.Chat.Anthropic.Content;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using ChatTailorAI.Shared.Models.Chat.Google.Content;

namespace ChatTailorAI.Shared.Models.Chat.Google.Converters
{
    public class GoogleChatContentConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(List<IGoogleChatContent>);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JArray contentArray = JArray.Load(reader);
            List<IGoogleChatContent> contents = new List<IGoogleChatContent>();

            foreach (var item in contentArray)
            {
                string type = null;
                if (item["inlineData"] != null)
                {
                    type = "inlineData";
                }
                else if (item["text"] != null)
                {
                    type = "text";
                }

                if (type == null)
                {
                    throw new Exception("Neither 'text' nor 'inlineData' property exists");
                }

                switch (type)
                {
                    case "text":
                        contents.Add(item.ToObject<GoogleTextContentPart>(serializer));
                        break;
                    case "image":
                        contents.Add(item.ToObject<GoogleImageContentPart>(serializer));
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
