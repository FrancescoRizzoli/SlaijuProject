using Utility;
using Cysharp.Threading.Tasks;

namespace UnityEngine.UI
{
    public class ButtonUIController : MonoBehaviour
    {
        [SerializeField]
        private ScreenType screenType;
        [SerializeField]
        private bool open = true;

        private void Awake()
        {
            
        }
        public void OnButtonClick()
        {
            UIManager.instance.ManageScreen(screenType,open);
        }
        public async void OnButtonClick(float delayTime)
        {
            await UniTask.Delay((int)(delayTime * 1000));
            UIManager.instance.ManageScreen(screenType, open);
        }
        public void OpneUI(ScreenType screen)
        {
            UIManager.instance.ManageScreen(screen, true);
        }
        public void CloseUI(ScreenType screen)
        {
            UIManager.instance.ManageScreen(screen, false);
        }
        


    }
}
