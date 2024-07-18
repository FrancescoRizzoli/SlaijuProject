using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Grid.Cell
{
    public class StartCell : BaseCell
    {
        [SerializeField] private StartCellView view = null;

#if UNITY_EDITOR
        private void OnValidate()
        {
            ID = CellID.Start;
        }
#endif

        public void HandleCharacterAppearence() => view.ChangeView().Forget();
    }
}
