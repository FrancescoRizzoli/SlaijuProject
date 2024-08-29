using Architecture;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.VFX;
using Utility;

namespace Character
{
    public class CharacterView : AView
    {
        [SerializeField] private GameObject warningGraphic = null;
        [SerializeField] private float warningDuration = 0.5f;

        [SerializeField] private ParticleSystem leftFootParticle = null;
        [SerializeField] private ParticleSystem rightFootParticle = null;
        [SerializeField] private ParticleSystem beamParticle = null;

        [Header("Audio")]
        [SerializeField] private AudioMixerGroup mixerGroup = null;
        [SerializeField] private AudioClip laserAudioClip = null;
        [SerializeField] private AudioClip roarAudioClip = null;
        [SerializeField] private AudioClip walkAudioClip = null;
        [SerializeField] private AudioClip warningAudioClip = null;
        [SerializeField] private AudioClip deathRoarAudioClip = null;

        public override async UniTask ChangeView()
        {
            warningGraphic.SetActive(true);
            AudioManager.instance.PlayAudioClip(warningAudioClip, mixerGroup);
            await UniTask.WaitForSeconds(warningDuration);
            warningGraphic.SetActive(false);
        }

        private void PlayLeftFootParticle()
        {
            leftFootParticle.Play();
            AudioManager.instance.PlayAudioClip(walkAudioClip, mixerGroup, PitchType.random);
        }
        private void PlayRightFootParticle()
        {
            rightFootParticle.Play();
            AudioManager.instance.PlayAudioClip(walkAudioClip, mixerGroup, PitchType.random);
        }
        private void PlayBeamParticle()
        {
            beamParticle.Play();
            AudioManager.instance.PlayAudioClip(laserAudioClip, mixerGroup);
        }

        private void Roar() => AudioManager.instance.PlayAudioClip(roarAudioClip, mixerGroup);

        private void DeathRoar() => AudioManager.instance.PlayAudioClip(deathRoarAudioClip, mixerGroup);
    }
}
