using ChatTailorAI.Shared.Transformers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Factories.Interfaces
{
    public interface IChatMessageTransformerFactory
    {
        IChatMessageTransformer Create(string chatServiceName);
    }
}
