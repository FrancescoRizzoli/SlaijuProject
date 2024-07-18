using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace UnityEngine.UI
{
    public class ScreenSettings : ScreenBase
    {
        [Header("Settings")]
        [SerializeField]
        GameObject active;
        [SerializeField]
        Button ToggleAudioButton;

        private void Start()
        {
            ToggleAudioButton.onClick.AddListener(ToggleAudio);
            UpdateToggleButtonText();
        }

        private void ToggleAudio()
        {
            Settings.ToggleMusic();
            UpdateToggleButtonText();
        }

        private void UpdateToggleButtonText()
        {
            bool isMusicOn = Settings.keysValues[nameof(SettingType.Music)] == 1;
            active.SetActive(!isMusicOn);
        }
    }
}
