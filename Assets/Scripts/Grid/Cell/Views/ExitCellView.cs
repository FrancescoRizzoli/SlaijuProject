using Architecture;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Grid.Cell
{
    public class ExitCellView : AView
    {
        [SerializeField] private Animator doorAnimator = null;

        private const string DOOR_ANIMATION_TRIGGER = "OpenDoor";

        public override async UniTask ChangeView()
        {
            doorAnimator.SetTrigger(DOOR_ANIMATION_TRIGGER);
            await UniTask.NextFrame();
        }
    }
}
