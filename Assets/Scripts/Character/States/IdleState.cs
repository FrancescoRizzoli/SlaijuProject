using Architecture;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Character
{
    public class IdleState : AState
    {
        private const string IDLE_ANIMATION_TRIGGER = "Idle";

        public override async UniTask Enter()
        {
            Debug.Log("Idle");

            ((CharacterStateController)controller).animator.SetTrigger(IDLE_ANIMATION_TRIGGER);
            await UniTask.NextFrame();
            ((CharacterStateController)controller).animator.ResetTrigger(IDLE_ANIMATION_TRIGGER);
        }

        public override async UniTask Exit()
        {
            await UniTask.NextFrame();
        }

        public override void Tick() { }
    }
}
