
using Utility;

namespace UnityEngine.UI
{
    public class ScreenSettings : ScreenBase
    {
        [Header("Settings")]
        [SerializeField]
        Button ToggleAudioButton;
        [SerializeField]
        Slider barSFX;
        [SerializeField]
        Slider barMusic;
        [SerializeField]
        Toggle cameraToggle;

        private void Start()
        {
           
            barSFX.onValueChanged.AddListener(SfxSlider);
            barMusic.onValueChanged.AddListener(MusicSlider);
            cameraToggle.onValueChanged.AddListener(CameraChangeListener);
            SetSlider();
            SetCameraToggle();
        }

       

       

        private void SetSlider()
        {
            barSFX.value = Settings.keysFloatValues[nameof(SettingType.Sfx)];
            barMusic.value = Settings.keysFloatValues[nameof(SettingType.Music)];

        }
        private void SetCameraToggle()
        {
            cameraToggle.isOn = Settings.keysValues[nameof(SettingType.Camera)] == 1;
        }


        private void SfxSlider(float value)
        {
            Settings.SliderSFX(value);
        }
        private void MusicSlider(float value)
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
