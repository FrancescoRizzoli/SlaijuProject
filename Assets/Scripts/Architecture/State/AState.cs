using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Architecture
{
    public abstract class AState : MonoBehaviour
    {
        protected AStateController controller = null;

        public delegate UniTask ChangeStateRequestEvent(AState targetState);
        public event ChangeStateRequestEvent OnChangeStateRequest = null;

        public virtual void Init(AStateController controller)
        {
            this.controller = controller;
        }

        public abstract UniTask Enter();
        public abstract void Tick();
        public abstract UniTask Exit();

        protected void RequestChangeState(AState targetState) => OnChangeStateRequest?.Invoke(targetState);
    }
}
