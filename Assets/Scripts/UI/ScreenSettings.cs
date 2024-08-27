
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
        [SerializeField]
        Slider bar;

        private void Start()
        {
            ToggleAudioButton.onClick.AddListener(ToggleAudio);
            bar.onValueChanged.AddListener(AudioSlider);
            UpdateToggleButtonText();
            setSlider();
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

        private void setSlider()
        {
            bar.value = Settings.keysFloatValues[nameof(SettingType.Sfx)];
        }

        private void AudioSlider(float value)
        {
            Settings.SliderMusic(value);
        }

    }
}
