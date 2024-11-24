using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace ChatTailorAI.Shared.Models.Chat.LMStudio.Responses
{
    public class LMStudioModelsResponse
    {
        [JsonProperty("data")]
        public List<ModelData> Data { get; set; }

        [JsonProperty("object")]
        public string Object { get; set; }

        public List<string> GetModelIds()
        {
            return Data.Select(d => d.Id).ToList();
        }
    }

    public class ModelData
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("owned_by")]
        public string OwnedBy { get; set; }

        [JsonProperty("permission")]
        public List<Permission> Permission { get; set; }
    }

    public class Permission
    {
    }
}