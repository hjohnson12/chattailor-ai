using System;
using ChatTailorAI.Shared.Models.Settings;
using ChatTailorAI.Shared.Services.Common;
using Newtonsoft.Json;
using Windows.Storage;

namespace ChatTailorAI.Services.Uwp.Settings
{
    public class UserSettingsService : IUserSettingsService
    {
        public event EventHandler<string> SettingChanged;

        public T Get<T>(string settingKey)
        {
            object result = ApplicationData.Current.LocalSettings.Values[settingKey];
            return result is null ? (T)UserSettings.Defaults[settingKey] : (T)result;
        }

        public T Get<T>(string settingKey, T defaultOverride)
        {
            object result = ApplicationData.Current.LocalSettings.Values[settingKey];
            return result is null ? defaultOverride : (T)result;
        }

        public T GetAndDeserialize<T>(string settingKey, JsonSerializerSettings jsonSerializerSettings)
        {
            object result = ApplicationData.Current.LocalSettings.Values[settingKey];
            if (result is string serialized)
            {
                return JsonConvert.DeserializeObject<T>(serialized, jsonSerializerSettings);
            }

            return (T)UserSettings.Defaults[settingKey];
        }

        public void Set<T>(string settingKey, T value)
        {
            ApplicationData.Current.LocalSettings.Values[settingKey] = value;
            SettingChanged?.Invoke(this, settingKey);
        }

        public void SetAndSerialize<T>(string settingKey, T value, JsonSerializerSettings jsonSerializerSettings)
        {
            var serialized = JsonConvert.SerializeObject(value);
            Set(settingKey, serialized);
        }
    }
}