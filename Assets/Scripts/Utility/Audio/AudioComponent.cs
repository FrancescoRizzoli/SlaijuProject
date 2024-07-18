using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Utility
{
    /// <summary>
    /// used to handle audio with ui Button and Animation Event
    /// </summary>
    public class AudioComponent : MonoBehaviour
    {
        [SerializeField]
        private List<AudioWrapper> audioWrappers;
        [SerializeField]
        private AudioMixerGroup mixerGroup;
        [SerializeField]
        private AudioSource audioSource;

        private Dictionary<AudioIdentifier, AudioWrapper> audioWrapperDictionary;

        private void Awake()
        {
            InitializeDictionary();
        }

        //initialize the dictionary with the content of the list for faster search
        private void InitializeDictionary()
        {
            audioWrapperDictionary = new Dictionary<AudioIdentifier, AudioWrapper>();
            foreach (var wrapper in audioWrappers)
            {
                if (audioWrapperDictionary.ContainsKey(wrapper.audioIdentifier))
                {
                    Debug.LogWarning($"Duplicate AudioIdentifier {wrapper.audioIdentifier} found. Skipping.");
                    continue;
                }
                audioWrapperDictionary.Add(wrapper.audioIdentifier, wrapper);
            }
        }

       
        /// <summary>
        /// play audio with enu,
        /// </summary>
        /// <param name="identifier"></param>
        public void PlayAudioEnum(AudioIdentifier identifier)
        {
            if (audioWrapperDictionary.TryGetValue(identifier, out var wrapper))
            {
                if (wrapper.audioClip != null)
                {
                    AudioManager.instance.PlayAudioClip(wrapper.audioClip, mixerGroup, wrapper.pitchType);
                }
                else
                {
                    Debug.LogWarning($"AudioClip for {identifier} is null.");
                }
            }
            else
            {
                Debug.LogWarning($"AudioIdentifier {identifier} not found in audioWrappers.");
            }
        }

       /// <summary>
       /// play audio with index use in ui Button
       /// </summary>
       /// <param name="index"></param>
        public void PlayAudioId(int index)
        {
            if (index < 0 || index >= audioWrappers.Count)
            {
                Debug.LogWarning($"Index {index} is out of range.");
                return;
            }
            var wrapper = audioWrappers[index];
            if (wrapper.audioClip != null)
            {
                AudioManager.instance.PlayAudioClip(wrapper.audioClip, mixerGroup, wrapper.pitchType);
            }
            else
            {
                Debug.LogWarning($"AudioClip at index {index} is null.");
            }
        }
        /// <summary>
        /// play audio clip from the audioSOurce of the component
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="audioSource"></param>
        /// <param name="oneShot"></param>
        public void PlayAudioWithPredefinedSourceEnum(AudioIdentifier identifier)
        {
            if (audioSource == null)
                return;
            if (audioWrapperDictionary.TryGetValue(identifier, out var wrapper))
            {
                if (wrapper.audioClip != null)
                {
                    AudioManager.instance.PlayAudioClipPreDefinedSource(wrapper.audioClip, audioSource, wrapper.pitchType, wrapper.oneShot);
                }
                else
                {
                    Debug.LogWarning($"AudioClip for {identifier} is null.");
                }
            }
            else
            {
                Debug.LogWarning($"AudioIdentifier {identifier} not found in audioWrappers.");
            }
        }

      
        public void PlayAudioWithPredefinedSourceInt(int index)
        {
            if (audioSource == null)
                return;
            if (index < 0 || index >= audioWrappers.Count)
            {
                Debug.LogWarning($"Index {index} is out of range.");
                return;
            }
            var wrapper = audioWrappers[index];
            if (wrapper.audioClip != null)
            {
                AudioManager.instance.PlayAudioClipPreDefinedSource(wrapper.audioClip, audioSource, wrapper.pitchType, wrapper.oneShot);
            }
            else
            {
                Debug.LogWarning($"AudioClip at index {index} is null.");
            }
        }
    }
}