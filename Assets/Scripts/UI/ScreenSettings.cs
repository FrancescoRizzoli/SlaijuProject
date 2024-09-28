

using UnityEngine.UIElements;
using Utility;

namespace UnityEngine.UI
{
    public class ScreenSettings : ScreenBase
    {
        [Header("Settings")]
        [SerializeField]
        Slider barSFX;
        [SerializeField]
        Slider barMusic;
        [SerializeField]
        Toggle cameraToggle;
        [SerializeField]
        GameObject isometric;
        [SerializeField]
        GameObject topDonw;

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
            if(cameraToggle.isOn )
            {
                isometric.SetActive(true);
                topDonw.SetActive(false);
            }
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
            isometric.SetActive(toggle);
            topDonw.SetActive(!toggle);
            Settings.SaveSettings();
        }

    }
}
