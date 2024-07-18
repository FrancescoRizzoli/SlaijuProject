using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Core
{
    public class ChangeMenuSong : MonoBehaviour
    {
        [SerializeField]
        private AudioClip audioClip;
        [SerializeField]
        AudioMixerGroup audioMixerGroup;
        [SerializeField]
        AudioSource audioSource;

        AudioClip oldClip;

        public void PlaySongOnce()
        {
            oldClip = audioSource.clip;
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        public void ResumePreviuosSong()
        {
            audioSource.clip = oldClip;
            audioSource.Play();
        }
    }
}
