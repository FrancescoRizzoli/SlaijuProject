using Architecture;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using Utility;
using Random = UnityEngine.Random;

namespace Grid.Cell
{
    public class CellDestructionView : AView
    {
        [SerializeField] private GameObject toBeDestroyedGraphic = null;
        [Header("Total destruction")]
        [SerializeField] private float collapseDistance = 5.0f;
        [SerializeField] private float collapseSpeed = 1.5f;
        [SerializeField] private ParticleSystem totalDestructionParticle = null;
        [SerializeField] private AudioClip collapseAudioClip = null;
        //[SerializeField, Min(0.0f)] private float particlePerSeconds = 2.0f;
        [Header("Shake")]
        [SerializeField] private float envShakeMagnitude = 0.5f;
        [Header("Shake - Single Damage Only")]
        [SerializeField] private ParticleSystem[] particle = Array.Empty<ParticleSystem>();
        [SerializeField] private float envShakeDuration = 0.75f;
        [Header("Destruction completed")]
        [SerializeField] private ParticleSystem destructionCompletedParticle = null;

        //private CancellationTokenSource particleCancellationTokenSource = new CancellationTokenSource();

        public override async UniTask ChangeView()
        {
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
         */

        private void PlayRandomDamageParticle()
        {
            if (particle.Length == 0)
                return;

            particle[Random.Range(0, particle.Length)].Play();
        }

        public void SingleDamage()
        {
            PlayRandomDamageParticle();
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
