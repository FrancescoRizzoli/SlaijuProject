using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;

namespace Utility
{
    public static class Settings
    {
        public delegate void OnSettingsChange();
        public static event OnSettingsChange onSettingsChange;

        public static Dictionary<string, int> keysValues = new Dictionary<string, int>()
        {
            { nameof(SettingType.Music), 1 },
            { "levelReached", 3 },
        };

        /// <summary>
        /// Initialize PlayerPrefs
        /// </summary>
        static Settings()
        {
            List<string> keys = new List<string>(keysValues.Keys);

            foreach (string key in keys)
            {
                if (PlayerPrefs.HasKey(key))
                {
                    keysValues[key] = PlayerPrefs.GetInt(key);
                }
            }
        }

        /// <summary>
        /// Save PlayerPrefs
        /// </summary>
        public static void SaveSettings()
        {
            foreach (string key in keysValues.Keys)
            {
                PlayerPrefs.SetInt(key, keysValues[key]);
            }

            PlayerPrefs.Save();
        }

        /// <summary>
        /// Change settings without changing PlayerPrefs
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetInt(string key, int value)
        {
            if (keysValues.ContainsKey(key))
            {
                keysValues[key] = value;
                onSettingsChange?.Invoke();
            }
        }

        /// <summary>
        /// Update the level reached if the new level is higher
        /// </summary>
        /// <param name="newLevel"></param>
        public static void UpdateLevelReached(int newLevel)
        {
            if (newLevel > keysValues["levelReached"])
            {
                keysValues["levelReached"] = newLevel;
                SaveSettings();
                onSettingsChange?.Invoke();
            }
        }

        /// <summary>
        /// Get the current level reached
        /// </summary>
        /// <returns></returns>
        public static int GetLevelReached()
        {
            return keysValues.ContainsKey("levelReached") ? keysValues["levelReached"] : 0;
        }
        public static void ToggleMusic()
        {
            if (keysValues.ContainsKey(nameof(SettingType.Music)))
            {
                keysValues[nameof(SettingType.Music)] = keysValues[nameof(SettingType.Music)] == 1 ? 0 : 1;
                SaveSettings();
                onSettingsChange?.Invoke();
            }
        }
    }
}
