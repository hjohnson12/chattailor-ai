using ChatTailorAI.Shared.Models.Chat;
using ChatTailorAI.Shared.Models.Chat.Anthropic;
using ChatTailorAI.Shared.Models.Chat.Google;
using ChatTailorAI.Shared.Models.Chat.LMStudio;
using ChatTailorAI.Shared.Models.Chat.OpenAI;
using ChatTailorAI.Shared.Models.Settings;
using ChatTailorAI.Shared.Services.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Factories
{
    public interface IChatSettingsFactory
    {
        ChatSettings CreateChatSettings(string chatServiceType);
        OpenAIChatSettings CreateOpenAIChatSettings();
    }
    
    public class ChatSettingsFactory : IChatSettingsFactory
    {
        private readonly IUserSettingsService _userSettingsService;

        public ChatSettingsFactory(IUserSettingsService userSettingsService)
        {
            _userSettingsService = userSettingsService;
        }

        public ChatSettings CreateChatSettings(string chatServiceType)
        {
            switch (chatServiceType)
            {
                case "openai":
                    return CreateOpenAIChatSettings();
                case "google":
                    return CreateGoogleChatSettings();
                case "anthropic":
                    return CreateAnthropicChatSettings();
                case "lmstudio":
                    return CreateLMStudioChatSettings();
                default:
                    throw new Exception($"Chat service type {chatServiceType} is not supported.");
            }
        }

        public OpenAIChatSettings CreateOpenAIChatSettings()
        {
            var chatSettings = new OpenAIChatSettings
            {
                // TODO: Get particular convo settings if settings are set
                Stream = _userSettingsService.Get<bool>(UserSettings.StreamReply),
                FrequencyPenalty = _userSettingsService.Get<double>(UserSettings.FrequencyPenalty),
                MaxTokens = _userSettingsService.Get<int>(UserSettings.MaxTokens),
                PresencePenalty = _userSettingsService.Get<double>(UserSettings.PresencePenalty),
                Temperature = _userSettingsService.Get<double>(UserSettings.Temperature),
            };

            return chatSettings;
        }

        public GoogleChatSettings CreateGoogleChatSettings()
        {
            var chatSettings = new GoogleChatSettings
            {
                // TODO: Get particular convo settings if settings are set
                Stream = _userSettingsService.Get<bool>(UserSettings.StreamReply),
                FrequencyPenalty = _userSettingsService.Get<double>(UserSettings.FrequencyPenalty),
                MaxTokens = _userSettingsService.Get<int>(UserSettings.MaxTokens),
                PresencePenalty = _userSettingsService.Get<double>(UserSettings.PresencePenalty),
                Temperature = _userSettingsService.Get<double>(UserSettings.Temperature),
            };

            return chatSettings;
        }

        public AnthropicChatSettings CreateAnthropicChatSettings()
        {
            var chatSettings = new AnthropicChatSettings
            {
                // TODO: Get particular convo settings if settings are set
                Stream = _userSettingsService.Get<bool>(UserSettings.StreamReply),
                FrequencyPenalty = _userSettingsService.Get<double>(UserSettings.FrequencyPenalty),
                MaxTokens = _userSettingsService.Get<int>(UserSettings.MaxTokens),
                PresencePenalty = _userSettingsService.Get<double>(UserSettings.PresencePenalty),
                Temperature = _userSettingsService.Get<double>(UserSettings.Temperature),
                System = _userSettingsService.Get<string>(UserSettings.SystemMessage),
            };

            return chatSettings;
        }

        public LMStudioChatSettings CreateLMStudioChatSettings()
        {
            var chatSettings = new LMStudioChatSettings
            {
                Stream = _userSettingsService.Get<bool>(UserSettings.StreamReply),
                MaxTokens = _userSettingsService.Get<int>(UserSettings.MaxTokens),
                Temperature = _userSettingsService.Get<double>(UserSettings.Temperature),
            };

            return chatSettings;
        }
    }
}
