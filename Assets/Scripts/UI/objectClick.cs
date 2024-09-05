using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UnityEngine.UI
{
    public class objectClick : MonoBehaviour
    {
        
        public UnityEvent click;

        public void MouseClick()
        {
            click?.Invoke();
        }
    }
}
