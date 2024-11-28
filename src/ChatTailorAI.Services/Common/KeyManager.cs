using System.Collections.Generic;
using ChatTailorAI.Shared.Events;
using ChatTailorAI.Shared.Models.Settings;
using ChatTailorAI.Shared.Services.Common;
using ChatTailorAI.Shared.Services.Events;

namespace ChatTailorAI.Services.Common
{
    public class KeyManager
    {
        private readonly Dictionary<ApiKeyType, string> _keys = new Dictionary<ApiKeyType, string>();
        private readonly IAppSettingsService _appSettingsService;
        private readonly IUserSettingsService _userSettingsService;
        private readonly IEventAggregator _eventAggregator;

        public KeyManager(
            IAppSettingsService appSettingsService,
            IUserSettingsService userSettingsService,
            IEventAggregator eventAggregator
            )
        {
            _appSettingsService = appSettingsService;
            _userSettingsService = userSettingsService;
            _eventAggregator = eventAggregator;

            _eventAggregator.ApiKeyChanged += OnApiKeyChanged;

            LoadAllKeys();
        }

        private void OnApiKeyChanged(object sender, Shared.Events.EventArgs.ApiKeyChangedEventArgs e)
        {
            UpdateKey(e.KeyType, e.ApiKey);
        }

        private void LoadAllKeys()
        {
            _keys[ApiKeyType.OpenAI] = GetKeyFromSettings(UserSettings.OpenAiApiKey, _appSettingsService.OpenAiApiKey);
            _keys[ApiKeyType.AzureSpeech] = GetKeyFromSettings(UserSettings.AzureSpeechServicesKey, "");
            _keys[ApiKeyType.ElevenLabs] = GetKeyFromSettings(UserSettings.ElevenLabsApiKey, "");
            _keys[ApiKeyType.Google] = GetKeyFromSettings(UserSettings.GoogleAIApiKey, "");
            _keys[ApiKeyType.Anthropic] = GetKeyFromSettings(UserSettings.AnthropicApiKey, "");
        }

        private string GetKeyFromSettings(string userSettingKey, string appSettingValue)
        {
            return !string.IsNullOrEmpty(appSettingValue) ? appSettingValue : _userSettingsService.Get<string>(userSettingKey);
        }

        public void UpdateKey(ApiKeyType keyType, string newKey)
        {
            if (!string.IsNullOrEmpty(newKey))
            {
                _keys[keyType] = newKey;
                NotifyKeyUpdated(keyType, newKey);
            }
        }

        public string GetKey(ApiKeyType keyType)
        {
            return _keys.TryGetValue(keyType, out var key) ? key : null;
        }

        private void NotifyKeyUpdated(ApiKeyType keyType, string newKey)
        {
            // TODO: Notify, or have service classes just read from here
        }
    }
}
