using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Architecture
{
    public abstract class AStateController : MonoBehaviour
    {
        protected abstract void Init();
        protected abstract UniTask ChangeState(AState state);
    }
}
