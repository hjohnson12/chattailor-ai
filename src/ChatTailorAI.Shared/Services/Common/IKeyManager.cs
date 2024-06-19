using ChatTailorAI.Shared.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Services.Common
{
    public interface IKeyManager
    {
        string GetKey(ApiKeyType keyType);
        void UpdateKey(ApiKeyType keyType, string newKey);
    }

}
