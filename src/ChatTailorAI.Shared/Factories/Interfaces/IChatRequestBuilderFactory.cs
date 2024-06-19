using ChatTailorAI.Shared.Builders;
using ChatTailorAI.Shared.Models.Chat;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Factories.Interfaces
{
    public interface IChatRequestBuilderFactory
    {
        IChatRequestBuilder GetBuilder(string chatServiceName);
    }

    //public interface IChatRequestBuilderFactory
    //{
    //    IChatRequestBuilder<TChatRequest, TMessage, TSettings> GetBuilder<TChatRequest, TMessage, TSettings>(string chatServiceName)
    //        where TChatRequest : ChatRequest<TSettings, TMessage>
    //        where TMessage : IChatModelMessage
    //        where TSettings : ChatSettings;
    //}

    //public interface IChatRequestBuilderFactory
    //{
    //    IChatRequestBuilder GetBuilder(string chatServiceName);
    //}
}