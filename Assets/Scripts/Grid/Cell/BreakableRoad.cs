using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Grid.Cell
{
    public class BreakableRoad : CurveCell
    {
        [SerializeField] private int health = 0;
        [SerializeField] private BreakableRoadView view = null;
        public delegate void DestructionRoadEvent();
        public event DestructionRoadEvent OnDestructionRoad = null;


#if UNITY_EDITOR
        [ContextMenu("TestOnCellExit")]
        private void TestOnCellExit() => OnCellExit();
#endif


        public override void OnCellExit()
        {
            health--;

            if (health == 0)
            {
                OnDestructionRoad?.Invoke();
                view.ChangeView().Forget();
                SetCellAsEmpty();
            }
            else
                view.DamageCell(health).Forget();
        }

        private void SetCellAsEmpty()
        {
            ID = CellID.Empty;
            safeSide.Clear();
        }
    }
}
