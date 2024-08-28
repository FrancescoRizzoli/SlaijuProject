
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
        [SerializeField]
        Toggle cameraToggle;

        private void Start()
        {
            ToggleAudioButton.onClick.AddListener(ToggleAudio);
            bar.onValueChanged.AddListener(AudioSlider);
            cameraToggle.onValueChanged.AddListener(CameraChangeListener);
            UpdateToggleButtonText();
            SetSlider();
            SetCameraToggle();
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

        private void SetSlider()
        {
            bar.value = Settings.keysFloatValues[nameof(SettingType.Sfx)];
        }
        private void SetCameraToggle()
        {
            cameraToggle.isOn = Settings.keysValues[nameof(SettingType.Camera)] == 1;
        }


        private void AudioSlider(float value)
        {
            Settings.SliderMusic(value);
        }

        private void CameraChangeListener(bool toggle)
        {
           
            Settings.SetInt(nameof(SettingType.Camera),toggle ? 1 : 0);

            Settings.SaveSettings();
        }

    }
}
