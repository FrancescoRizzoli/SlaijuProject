using Architecture;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Utility;
using Random = UnityEngine.Random;

namespace Grid.Cell
{
    public class CellDestructionView : AView
    {
        [Header("Smoke")]
        [SerializeField] private List<VisualEffect> smokeEffect = new List<VisualEffect>();
        [SerializeField, Min(1.0f)] private float smokeEffectPlayRateOnStop = 10.0f;
        [SerializeField, Min(0.0f)] private float lastSmokeWaitTime = 0.5f;
        [Header("Target Graphic")]
        [SerializeField] private GameObject toBeDestroyedGraphic = null;
        [SerializeField] private GameObject cityGroundGameObject = null;
        [SerializeField] private GameObject frontGrassGameObject = null;
        [SerializeField] private ParticleSystem cityToGrassParticle = null;
        [Header("Shake")]
        [SerializeField] private float envShakeMagnitude = 0.5f;
        [Header("Total destruction")]
        [SerializeField] private float collapseDistance = 5.0f;
        [SerializeField] private float collapseSpeed = 1.5f;
        [SerializeField] private ParticleSystem totalDestructionParticle = null;
        [SerializeField] private AudioClip collapseAudioClip = null;
        //[SerializeField, Min(0.0f)] private float particlePerSeconds = 2.0f;
        [Header("Shake - Single Damage Only")]
        //[SerializeField] private ParticleSystem[] particle = Array.Empty<ParticleSystem>();
        [SerializeField] private float envShakeDuration = 0.75f;
        [Header("Destruction completed")]
        [SerializeField] private ParticleSystem destructionCompletedParticle = null;

        //private CancellationTokenSource particleCancellationTokenSource = new CancellationTokenSource();

        public override async UniTask ChangeView()
        {
            if (smokeEffect.Count > 0)
            {
                smokeEffect[0].Stop();
                smokeEffect[0].playRate = smokeEffectPlayRateOnStop;
                await UniTask.WaitForSeconds(lastSmokeWaitTime);
            }

            Vector3 targetPosition = toBeDestroyedGraphic.transform.position + Vector3.down * collapseDistance;
            Vector3 initialPosition = toBeDestroyedGraphic.transform.position;

            //ParticleRoutine(particleCancellationTokenSource.Token).Forget();
            totalDestructionParticle.gameObject.SetActive(true);
            AudioManager.instance.PlayAudioClipPreDefinedSource(collapseAudioClip, toBeDestroyedGraphic.GetComponent<AudioSource>());

            while (true)
            {
                toBeDestroyedGraphic.transform.position = Vector3.MoveTowards(toBeDestroyedGraphic.transform.position, targetPosition, collapseSpeed * Time.deltaTime);

                ShakeEnv(initialPosition);

                if (toBeDestroyedGraphic.transform.position.y == targetPosition.y)
                    break;

                await UniTask.NextFrame();
            }

            //particleCancellationTokenSource.Cancel();
            toBeDestroyedGraphic.transform.position = targetPosition;
            await UniTask.NextFrame();

            toBeDestroyedGraphic.SetActive(false);
            totalDestructionParticle.gameObject.SetActive(false);
            
            if (cityToGrassParticle != null)
            {
                cityToGrassParticle.Play();
                await UniTask.WaitForSeconds(cityToGrassParticle.main.duration / 2);
            }
            cityGroundGameObject.SetActive(false);
            frontGrassGameObject.SetActive(true);
        }

        /*
        private async UniTask ParticleRoutine(CancellationToken token)
        {
            if (particlePerSeconds == 0.0f)
                return;

            while (true)
            {
                if (token.IsCancellationRequested)
                    break;

                PlayRandomDamageParticle();

                await UniTask.WaitForSeconds(1 / particlePerSeconds);
            }
        }

        private void PlayRandomDamageParticle()
        {
            if (particle.Length == 0)
                return;

            particle[Random.Range(0, particle.Length)].Play();
        }
         */

        public void SingleDamage()
        {
            //PlayRandomDamageParticle();

            if (smokeEffect.Count > 0)
            {
                int index = Random.Range(0, smokeEffect.Count);
                smokeEffect[index].Stop();
                smokeEffect[index].playRate = smokeEffectPlayRateOnStop;
                smokeEffect.RemoveAt(index);
            }

            ShakeEnv(envShakeDuration).Forget();
        }

        private async UniTask ShakeEnv(float duration)
        {
            Vector3 initialPosition = toBeDestroyedGraphic.transform.position;
            float currentTime = 0.0f;

            while (true)
            {
                ShakeEnv(initialPosition);

                currentTime += Time.deltaTime;
                if (currentTime >= duration)
                    break;

                await UniTask.NextFrame();
            }

            toBeDestroyedGraphic.transform.position = initialPosition;
            await UniTask.NextFrame();
        }

        private void ShakeEnv(Vector3 initialPosition)
        {
            float xOffset = Random.Range(-0.5f, 0.5f) * envShakeMagnitude;
            float zOffset = Random.Range(-0.5f, 0.5f) * envShakeMagnitude;

            toBeDestroyedGraphic.transform.position = new Vector3(initialPosition.x + xOffset,
                                                                  toBeDestroyedGraphic.transform.position.y,
                                                                  initialPosition.z + zOffset);
        }

        public void SetDestructionCompleted()
        {
            if (destructionCompletedParticle == null)
                return;

            destructionCompletedParticle.Play();
        }
    }
}
