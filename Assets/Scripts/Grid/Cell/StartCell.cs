using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Grid.Cell
{
    public class StartCell : BaseCell
    {
        [SerializeField] private StartCellView view = null;

        [Serializable] public sealed class CharacterReturnEvent : UnityEngine.Events.UnityEvent { };
        public CharacterReturnEvent OnCharacterReturn = new CharacterReturnEvent();

        private bool firstAppearance = true;

#if UNITY_EDITOR
        private void OnValidate()
        {
            ID = CellID.Start;
        }
#endif

        public void HandleCharacterAppearence()
        {
            if (firstAppearance)
            {
                view.ChangeView().Forget();
                firstAppearance = false;
            }
            else
                OnCharacterReturn?.Invoke();
        }
    }
}
