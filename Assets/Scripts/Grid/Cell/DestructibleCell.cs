using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Grid.Cell
{
    public class DestructibleCell : BaseCell
    {
        [SerializeField] protected int cellHealth = 1;
        [SerializeField] private CellDestructionView destructionView = null;

        protected int currentHealth = 0;

        public delegate void DestructionEvent();
        public event DestructionEvent OnDestruction = null;
        public event DestructionEvent OnZeroHealth = null;

        protected virtual void Start() => currentHealth = cellHealth;

        public virtual async UniTask GetDamage()
        {
            currentHealth--;

            if (currentHealth < 0)
                return;

            if (currentHealth == 0)
            {
                OnZeroHealth?.Invoke();
                await destructionView.ChangeView();
                SetCellAsEmpty();
                OnDestruction?.Invoke();
                destructionView.SetDestructionCompleted();
            }
            else
                destructionView.SingleDamage();
        }

        private void SetCellAsEmpty()
        {
            ID = CellID.Empty;
            safeSide.Clear();
        }



#if UNITY_EDITOR
        [ContextMenu("Damage")]
        private void DamageCell() => GetDamage().Forget();
#endif
    }
}
