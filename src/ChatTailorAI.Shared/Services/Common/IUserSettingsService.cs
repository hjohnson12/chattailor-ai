using System;
using Newtonsoft.Json;

namespace ChatTailorAI.Shared.Services.Common
{
    /// <summary>
    /// Interface for storing
    /// and retrieving user settings.
    /// </summary>
    public interface IUserSettingsService
    {
        /// <summary>
        /// Raised when a settings is changed.
        /// </summary>
        event EventHandler<string> SettingChanged;

        /// <summary>
        /// Saves settings into persistent local
        /// storage.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="settingKey">The settings key, generally found in <see cref="UserSettings"/>.</param>
        /// <param name="value">The value to save.</param>
        void Set<T>(string settingKey, T value);

        /// <summary>
        /// Retrieves the value for the desired settings key.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="settingKey">The settings key, generally found in <see cref="UserSettings"/>.</param>
        /// <returns>The desired value or returns the default value.</returns>
        T Get<T>(string settingKey);

        /// <summary>
        /// Retrieves the value for the desired settings key.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="settingKey">The settings key, generally found in <see cref="UserSettings"/>.</param>
        /// <param name="defaultOverride">The default override to use if the setting has no value.</param>
        /// <returns>The desired value or returns the default override.</returns>
        T Get<T>(string settingKey, T defaultOverride);

        /// <summary>
        /// Retrieves the value for the desired settings key
        /// and performs json deserialization on the stored value.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="settingKey">The settings key, generally found in <see cref="UserSettings"/>.</param>
        /// <param name="jsonSerializerSettings">The <see cref="JsonSerializerSettings"/> instance to serialize <typeparamref name="T"/> values.</param>
        /// <returns>The desired value or returns the default.</returns>
        T GetAndDeserialize<T>(string settingKey, JsonSerializerSettings jsonSerializerSettings);

        /// <summary>
        /// Saves settings into persistent local storage
        /// after serializing the object.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="settingKey">The settings key, generally found in <see cref="UserSettings"/>.</param>
        /// <param name="value">The value to save.</param>
        /// <param name="jsonSerializerSettings">The <see cref="JsonSerializerSettings"/> instance to serialize <typeparamref name="T"/> values.</param>
        void SetAndSerialize<T>(string settingKey, T value, JsonSerializerSettings jsonSerializerSettings);
    }
}
