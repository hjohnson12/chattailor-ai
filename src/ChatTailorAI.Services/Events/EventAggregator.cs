using ChatTailorAI.Shared.Dto.Chat.OpenAI;
using ChatTailorAI.Shared.Events;
using ChatTailorAI.Shared.Events.EventArgs;
using ChatTailorAI.Shared.Models.Chat.OpenAI;
using ChatTailorAI.Shared.Services.Events;
using System;
using System.Collections.Generic;

namespace ChatTailorAI.Services.Events
{
    public class EventAggregator : IEventAggregator
    {
        public event EventHandler<ModeChangedEventArgs> ModeChanged;
        public event EventHandler<MessageReceivedEvent> ChatMessageReceived;
        public event EventHandler<ApiKeyChangedEventArgs> ApiKeyChanged;
        public event EventHandler<AssistantCreatedEvent> AssistantCreated;
        public event EventHandler<NotificationRaisedEvent> NotificationRaised;
        public event EventHandler<AuthenticationRequestedEvent> AuthenticationRequested;
        public event EventHandler<PromptCreatedEvent> PromptCreated;
        public event EventHandler<ChatUpdatedEvent> ChatUpdated;
        public event EventHandler<ImageGeneratedEvent> ImageGenerated;

        public void PublishModeChanged(string mode)
        {
            ModeChanged?.Invoke(this, new ModeChangedEventArgs { Mode = mode });
        }

        public void PublishMessageReceived(MessageReceivedEvent messageReceivedEvent)
        {
            ChatMessageReceived?.Invoke(this, messageReceivedEvent);
        }

        public void PublishApiKeyChange(ApiKeyChangedEventArgs apiKey)
        {
            ApiKeyChanged?.Invoke(this, apiKey);
        }

        public void PublishNewAssistantCreated(AssistantCreatedEvent assistantCreatedEvent)
        {
            AssistantCreated?.Invoke(this, assistantCreatedEvent);
        }

        public void PublishNotificationRaised(NotificationRaisedEvent notificationRaisedEvent)
        {
            NotificationRaised?.Invoke(this, notificationRaisedEvent);
        }

        public void PublishAuthenticationRequested(AuthenticationRequestedEvent authenticationRequestedEvent)
        {
            AuthenticationRequested?.Invoke(this, authenticationRequestedEvent);
        }

        public void PublishNewPromptCreated(PromptCreatedEvent promptCreatedEvent)
        {
            PromptCreated?.Invoke(this, promptCreatedEvent);
        }

        public void PublishChatUpdated(ChatUpdatedEvent chatUpdatedEvent)
        {
            ChatUpdated?.Invoke(this, chatUpdatedEvent);
        }

        public void PublishImageGenerated(ImageGeneratedEvent imageGeneratedEvent)
        { 
            ImageGenerated?.Invoke(this, imageGeneratedEvent);
        }
    }    
}