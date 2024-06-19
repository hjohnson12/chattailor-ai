using ChatTailorAI.Shared.Events.EventArgs;
using ChatTailorAI.Shared.Events;
using System;
using System.Collections.Generic;
using System.Text;
using ChatTailorAI.Shared.Models.Chat.OpenAI;
using ChatTailorAI.Shared.Dto.Chat.OpenAI;

namespace ChatTailorAI.Shared.Services.Events
{
    public interface IEventAggregator
    { 
        event EventHandler<ModeChangedEventArgs> ModeChanged;
        event EventHandler<MessageReceivedEvent> ChatMessageReceived;
        event EventHandler<ApiKeyChangedEventArgs> ApiKeyChanged;
        event EventHandler<AssistantCreatedEvent> AssistantCreated;
        event EventHandler<NotificationRaisedEvent> NotificationRaised;
        event EventHandler<AuthenticationRequestedEvent> AuthenticationRequested;
        event EventHandler<PromptCreatedEvent> PromptCreated;
        event EventHandler<ChatUpdatedEvent> ChatUpdated;
        event EventHandler<ImageGeneratedEvent> ImageGenerated;

        void PublishModeChanged(string mode);
        void PublishMessageReceived(MessageReceivedEvent message);
        void PublishApiKeyChange(ApiKeyChangedEventArgs apiKey);
        void PublishNewAssistantCreated(AssistantCreatedEvent assistantCreatedEvent);
        void PublishNotificationRaised(NotificationRaisedEvent notificationRaisedEvent);
        void PublishAuthenticationRequested(AuthenticationRequestedEvent authenticationRequestedEvent);
        void PublishNewPromptCreated(PromptCreatedEvent promptCreatedEvent);
        void PublishChatUpdated(ChatUpdatedEvent chatUpdatedEvent);
        void PublishImageGenerated(ImageGeneratedEvent imageGeneratedEvent);
    }
}