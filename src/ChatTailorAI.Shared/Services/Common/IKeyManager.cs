using ChatTailorAI.Shared.Events;

namespace ChatTailorAI.Shared.Services.Common
{
    public interface IKeyManager
    {
        string GetKey(ApiKeyType keyType);
        void UpdateKey(ApiKeyType keyType, string newKey);
    }

}
