using Architecture;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Character
{
	public class TurningState : AState
	{
		[SerializeField] private AState exitState = null;

		public Transform target { get; set; } = null;
		public Vector3 controlPoint { get; set; } = Vector3.zero;

		private float movementSpeed = 1.0f;
		private Tween pathTween = null;
		private Tween rotateTween = null;

		public override void Init(AStateController controller)
		{
			base.Init(controller);

			movementSpeed = ((CharacterStateController)controller).characterData.movementSpeed;
		}

		public override async UniTask Enter()
        {
            TurningTask().Forget();
            await UniTask.NextFrame();
        }

        private async UniTask TurningTask()
        {
            Vector3 startingPosition = controller.transform.position;

            pathTween = controller.transform.DOPath(new Vector3[] { target.position, startingPosition, controlPoint }, 1.0f, PathType.CubicBezier)
                                                           .SetEase(Ease.Linear).Pause();
            pathTween.ForceInit();

            rotateTween = controller.transform.DORotate(target.rotation.eulerAngles, 1.0f, RotateMode.Fast)
                                                           .SetEase(Ease.Linear).Pause();
            rotateTween.ForceInit();

            SetDOTweenTimescale();

            UniTask taskPath = pathTween.Play().ToUniTask();
            UniTask taskRotate = rotateTween.Play().ToUniTask();

            await UniTask.WhenAll(taskPath, taskRotate);

            RequestChangeState(exitState);
        }

        private void SetDOTweenTimescale()
        {
            float pathLength = pathTween.PathLength();
            pathTween.timeScale = movementSpeed / pathLength;
            rotateTween.timeScale = movementSpeed / pathLength;
        }

        public override async UniTask Exit()
		{
			await UniTask.NextFrame();
		}

		public override void Tick() { } // do nothing


		public void SetTurningSpeed(float speed)
		{
            movementSpeed = speed;

            if (rotateTween.IsActive())
                SetDOTweenTimescale();
        }
    }
}
