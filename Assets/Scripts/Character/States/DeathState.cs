using Architecture;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Character
{
    public class DeathState : AState
    {
        private const string DEATH_ANIMATION_TRIGGER = "Death";

        public override async UniTask Enter()
        {
            Debug.Log("Death");

            ((CharacterStateController)controller).animator.SetTrigger(DEATH_ANIMATION_TRIGGER);
            // call game over with animation event
            await UniTask.NextFrame();
        }

        public override async UniTask Exit()
        {
            await UniTask.NextFrame();
        }

        public override void Tick() { }     // do nothing
    }
}
