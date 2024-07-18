using Utility;

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
