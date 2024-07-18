using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Audio;

namespace Utility
{
    public class ButtonAudio : MonoBehaviour
    {
        [SerializeField]
        private AudioClip audioClip;
        [SerializeField]
        AudioMixerGroup audioMixerGroup;
        private void Awake()
        {
            Button button = GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(PlayAudioClip);
            }
        }
        public void PlayAudioClip()
        {
            AudioManager.instance.PlayAudioClip(audioClip,audioMixerGroup);
        }
    }
}
