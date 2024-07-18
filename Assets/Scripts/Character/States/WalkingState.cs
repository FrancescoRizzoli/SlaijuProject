using Architecture;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Character
{
    public class WalkingState : AState
    {
        public float movementSpeed { get; set; } = 1.0f;

        private const string WALK_ANIMATION_TRIGGER = "Walk";

        public override void Init(AStateController controller)
        {
            base.Init(controller);

            movementSpeed = ((CharacterStateController)controller).characterData.movementSpeed;
        }

        public override async UniTask Enter()
        {
            if (((CharacterStateController)controller).animator == null)
                return;

            ((CharacterStateController)controller).animator.SetTrigger(WALK_ANIMATION_TRIGGER);
           
            await UniTask.NextFrame();

            ((CharacterStateController)controller).animator.ResetTrigger(WALK_ANIMATION_TRIGGER);
        }

        public override async UniTask Exit()
        {
            await UniTask.NextFrame();
        }

        public override void Tick()
        {
            controller.transform.position += controller.transform.forward * movementSpeed * Time.deltaTime;
        }
    }
}
