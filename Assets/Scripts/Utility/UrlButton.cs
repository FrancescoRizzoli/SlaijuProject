using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Utility
{


    public class UrlButton : MonoBehaviour
    {
        public string link;
        // Start is called before the first frame update
        public void OpenURL()
        {
            Application.OpenURL(link);
            Debug.Log("is this working?");
        }
    }
}
