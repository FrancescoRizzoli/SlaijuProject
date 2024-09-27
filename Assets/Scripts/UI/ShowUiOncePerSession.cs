using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{
    public class ShowUIOncePerSession : MonoBehaviour
    {
        
        private static bool hasUIBeenShown = false; // variabile statica che persiste nella sessione

        void Start()
        {
            
            if (!hasUIBeenShown)
            {
                UIManager.instance.ManageScreen(Utility.ScreenType.MainMenu, true);
                hasUIBeenShown = true;
            }
            else
            {
                UIManager.instance.ManageScreen(Utility.ScreenType.MainMenu, false);
            }
        }
    }
}
