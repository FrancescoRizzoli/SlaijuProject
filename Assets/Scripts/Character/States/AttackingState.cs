using Architecture;
using Cysharp.Threading.Tasks;
using Grid.Cell;
using UnityEngine;

namespace Character
{
    public class AttackingState : AState
    {
        [SerializeField] private AState exitState = null;

        public DestructibleCell targetCell { get; set; } = null;

        private const string IDLE_ANIMATION_TRIGGER = "Idle";
        private const string ATTACK_ANIMATION_TRIGGER = "Attack";
        public override async UniTask Enter()
        {
            targetCell.OnDestruction += HandleCellDestruction;
            targetCell.OnZeroHealth += HandleCellZeroHealth;
            await UniTask.NextFrame();

            ((CharacterStateController)controller).animator.SetTrigger(ATTACK_ANIMATION_TRIGGER);
        }

        private void HandleCellZeroHealth() => ((CharacterStateController) controller).animator.SetTrigger(IDLE_ANIMATION_TRIGGER);

        public override async UniTask Exit()
        {
            await UniTask.NextFrame();
        }

        public override void Tick() { }     // do nothing

        private void HandleCellDestruction() => CellDestruction().Forget();

        private async UniTask CellDestruction()
        {
            await UniTask.WaitForSeconds(((CharacterStateController)controller).characterData.idleTimeAfterCellDestruction);
            RequestChangeState(exitState);
        }

        public void DamageTargetCell() => targetCell.GetDamage().Forget();
    }
}
