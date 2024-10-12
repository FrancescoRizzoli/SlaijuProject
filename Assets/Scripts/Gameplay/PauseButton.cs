using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class PauseButton : MonoBehaviour
    {
        public KeyCode pauseKey = KeyCode.Escape;
        [SerializeField]
        LevelUiController levelUiController;
       
        private bool isPaused = false;

        void Update()
        {
            
            if (Input.GetKeyDown(pauseKey))
            {
           
                if (isPaused)
                {
                    levelUiController.TogglePause();
                }
                else
                {
                    levelUiController.TogglePause();
                }
            }
        }
    }
}
