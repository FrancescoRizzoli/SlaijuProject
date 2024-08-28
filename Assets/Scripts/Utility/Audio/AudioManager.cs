using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;


namespace Utility
{
    public class AudioManager : MonoBehaviour
    {
        //use to make the audio pitch change sound more tuned
        private int[] pentatonicSemitones = new[] { 0, 2, 4, 7, 9 };
        const float pitchFactor = 1.059463f;


        #region Singleton
        private static AudioManager _instance = null;
        public static AudioManager instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("Audio manager");
                    _instance = go.AddComponent<AudioManager>();
                    DontDestroyOnLoad(go);
                    go.hideFlags = HideFlags.DontSave;
                }
                return _instance;
            }
        }
        #endregion

        [SerializeField]
        private AudioMixer AudioMixer = null;
        private ObjectPool<AudioSource> pool;
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
                pool = new ObjectPool<AudioSource>(CreateNewPoolEntry);
                
            }
            else if (_instance != this)
            {
                
                Destroy(gameObject);
            }
        }
        private void Start()
        {
            SetMixer();
        }

        
        private void OnEnable()
        {
            Settings.onSettingsChange += SetMixer;
            
        }
        private void OnDisable()
        {
            Settings.onSettingsChange -= SetMixer;
            
        }
        AudioSource CreateNewPoolEntry()
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            return source;
        }



        public void PlayAudioClip(AudioClip clip, AudioMixerGroup mixerGroup, PitchType type = PitchType.defaultPitch)
        {
            if (clip == null)
                return;

            AudioSource audioSource = pool.Get();
            audioSource.clip = clip;
            audioSource.pitch = GetPitchType(type);
            if (mixerGroup != null)
                audioSource.outputAudioMixerGroup = mixerGroup;
            audioSource.Play();
            StartCoroutine(DestroyAudioSourceOnFinish(audioSource));
        }
        public void PlayAudioClipPreDefinedSource(AudioClip clip, AudioSource audioSource, PitchType type = PitchType.defaultPitch, bool oneShoot = false)
        {
            audioSource.pitch = GetPitchType(type);
            if (clip == null)
                return;
            if (!oneShoot)
            {
                audioSource.clip = clip;
                audioSource.Play();
            }
            else
            {
                audioSource.PlayOneShot(clip);
            }
        }




        public float AudioClipTunedRandomPitch()
        {
            float pitch = 1;
            int x = UnityEngine.Random.Range(0, pentatonicSemitones.Length);
            for (int i = 0; i < x; i++)
            {
                pitch *= pitchFactor;
            }
            return pitch;
        }
        public float AudioClipRandomPitch()
        {
            float pitch = UnityEngine.Random.Range(0.8f, 1.2f);
            int x = UnityEngine.Random.Range(0, pentatonicSemitones.Length);
            for (int i = 0; i < x; i++)
            {
                pitch *= pitchFactor;
            }
            return pitch;
        }

        private float GetPitchType(PitchType type)
        {
            switch (type)
            {
                case PitchType.defaultPitch:
                    return 1;
                case PitchType.random:
                    return AudioClipRandomPitch();
                case PitchType.tuned:
                    return AudioClipTunedRandomPitch();
                default:
                    return 1;
            }
        }

        public void SetMixer()
        {
            if (AudioMixer == null)
                return;
                AudioMixer.SetFloat("SFXVolume",Mathf.Lerp(-80,0, Settings.keysFloatValues[nameof(SettingType.Sfx)]));
                AudioMixer.SetFloat("MusicVolume",Mathf.Lerp(-80,0, Settings.keysFloatValues[nameof(SettingType.Sfx)]));


        }

        public void TogglePauseAudio(bool pause)
        {
            if(pause && Settings.keysValues[nameof(SettingType.Music)] == 1)
                AudioMixer.SetFloat("MusicVolume", -30);
            else
                SetMixer();
        }
        
        

        IEnumerator DestroyAudioSourceOnFinish(AudioSource source)
        {
            yield return new WaitForSeconds(source.clip.length);
            pool.Release(source);
        }
    }
}
