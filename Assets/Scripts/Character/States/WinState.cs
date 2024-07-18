using Architecture;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Character
{
    public class WinState : AState
    {
        [SerializeField] private Vector3 targetOffset = Vector3.zero;

        public float movementSpeed { get; set; } = 1.0f;

        private Vector3 _targetPosition = Vector3.zero;
        public Vector3 targetPosition { get { return _targetPosition; } set { _targetPosition = value + targetOffset; } }

        private const string WIN_ANIMATION_TRIGGER = "Win";

        public override void Init(AStateController controller)
        {
            base.Init(controller);
            movementSpeed = ((CharacterStateController)controller).characterData.movementSpeed;
        }

        public override async UniTask Enter()
        {           
            Debug.Log("Win state");

            await UniTask.NextFrame();
        }

        public override async UniTask Exit()
        {
            await UniTask.NextFrame();
        }

        public override void Tick() 
        {
            if (controller.transform.position == targetPosition)
                return;

            controller.transform.position = Vector3.MoveTowards(controller.transform.position, targetPosition, movementSpeed * Time.deltaTime);

            if (controller.transform.position == targetPosition)
            {
                ((CharacterStateController)controller).animator.SetTrigger(WIN_ANIMATION_TRIGGER);
                // call win method in state controller with animation event
            }
        }
    }
}
