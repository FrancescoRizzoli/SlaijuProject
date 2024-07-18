using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    public class Quit : MonoBehaviour
    {
        private void Awake()
        {
            Button button = GetComponent<Button>();
            if(button != null )
            {
                button.onClick.AddListener(OnButtonClick);
            }
        }
        public void OnButtonClick()
        {
            Application.Quit();
        } 
    }
}
