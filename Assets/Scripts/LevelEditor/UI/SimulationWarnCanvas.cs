using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LevelEditor
{
    public class SimulationWarnCanvas : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup = null;
        [SerializeField, Min(0.01f)] private float canvasVisibleTime = 3.0f;
        [SerializeField, Min(0.01f)] private float fadeSpeed = 5.0f;

        private void OnEnable()
        {
            DisableCanvasTask().Forget();
        }

        private async UniTask DisableCanvasTask()
        {
            canvasGroup.alpha = 1.0f;

            await UniTask.WaitForSeconds(canvasVisibleTime);

            while (canvasGroup.alpha > 0.0f)
            {
                canvasGroup.alpha -= Time.deltaTime * fadeSpeed;
                await UniTask.NextFrame();
            }

            canvasGroup.alpha = 0.0f;
            gameObject.SetActive(false);
        }
    }
}
