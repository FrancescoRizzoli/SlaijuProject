using Utility;
using System.Collections;
using System.Collections.Generic;

namespace UnityEngine.UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager instance { get; private set; }
        [SerializeField]
        private List<ScreenBase> openedScreen = new List<ScreenBase>();
        private Dictionary<ScreenType, ScreenBase> screenInScene = new Dictionary<ScreenType, ScreenBase>();

        private int currentSortOrder = 0;

        [SerializeField]
        private ScreenType startingScreen;

        private Camera mainCamera;

        private void Awake()
        {
            mainCamera = Camera.main;
            instance = this;

            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }

        /// <summary>
        /// populate the dictionary that contains the screen in scene
        /// </summary>
        /// <param name="type"></param>
        /// <param name="screen"></param>
        public void SetScreenInScene(ScreenType type, ScreenBase screen)
        {
            screenInScene[type] = screen;
            if (type == startingScreen || type == ScreenType.HUD)
                ManageScreen(type, true);


        }
        /// <summary>
        /// open or close the corresponding screen in the dictionary the id is used for a screen that can change based on the item that open it
        /// the id its an enum value
        /// </summary>
        /// <param name="type"></param>
        /// <param name="open"></param>
        /// <param name="id"></param>
        public void ManageScreen(ScreenType type, bool open, uint id = 0)
        {
            if (open)
            {
                screenInScene[type].OpenScreen(id);
            }
            else
            {
                screenInScene[type].CloseScreen();
            }

        }

        /// <summary>
        /// Get the first sort order aviable, and return it to the screen so that the screen can be on top of another
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="disableCameraRendering"></param>
        /// <returns></returns>
        public int ScreenOpening(ScreenBase screen, bool disableCameraRendering)
        {
            if (openedScreen.Contains(screen))
                return -1;


            openedScreen.Add(screen);
            int screenSort = currentSortOrder;
            currentSortOrder++;
            if (!disableCameraRendering)
                return screenSort;

            mainCamera.enabled = false;
            return screenSort;

        }

        /// <summary>
        /// Remove a screen from the list of opened screen and decrease currentSortOrder
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="enableCameraRendering"></param>
        public void ScreenClosing(ScreenBase screen, bool enableCameraRendering)
        {
            if (!openedScreen.Contains(screen))
                return;

            openedScreen.Remove(screen);
            currentSortOrder--;

            HandleCamera(enableCameraRendering);
        }
        /// <summary>
        /// used to anable the camera before the start of the closing animation of the screen base
        /// </summary>
        /// <param name="enableCameraRendering"></param>
        public void HandleCamera(bool enableCameraRendering)
        {
            if (!enableCameraRendering)
                return;
            mainCamera.enabled = true;
        }



        /// <summary>
        /// change the interactable status of the canvas group
        /// </summary>
        /// <param name="type"></param>
        /// <param name="Interactable"></param>
        public void ManageScreenInteractable(ScreenType type, bool Interactable)
        {
            screenInScene[type].SetInteractebale(Interactable);

        }
        public bool ScreenIsOpen(ScreenType screenToCheck)
        {
            return openedScreen.Contains(screenInScene[screenToCheck]);
        }

        public void RefresScreen()
        {
            foreach (ScreenBase screen in openedScreen)
                screen.Refresh();
        }
    }
}
