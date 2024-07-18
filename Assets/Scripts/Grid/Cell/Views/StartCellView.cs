using Architecture;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.VFX;
using Utility;

namespace Grid.Cell
{
    public class StartCellView : AView
    {
        [Header("Lightning")]
        [SerializeField] private VisualEffect lightningVFX = null;
        [SerializeField] private AudioMixerGroup mixerGroup = null;
        [SerializeField] private AudioClip lightningSFX = null;
        [Header("Smoke")]
        [SerializeField] private ParticleSystem bigSmokeParticle = null;

        public override async UniTask ChangeView()
        {
            if (lightningVFX != null)
            {
                lightningVFX.Play();
                AudioManager.instance.PlayAudioClip(lightningSFX, mixerGroup);
                await UniTask.Delay(200);
            }

            bigSmokeParticle.Play();
            await UniTask.NextFrame();
        }
    }
}
