using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Utility
{
    public class ChangeCameraCinemachine : MonoBehaviour
    {
        [SerializeField]
        CinemachineVirtualCamera cam1;
        [SerializeField]
        CinemachineVirtualCamera cam2;

        private void Awake()
        {
            if(Settings.keysValues[nameof(SettingType.Camera)] ==1)
                Switch();

        }
        public void Switch()
        {
            if (cam1 == null || cam2 == null)
            {
                return;
            }
            int priority = cam1.Priority;
            cam1.Priority = cam2.Priority;
            cam2.Priority = priority;
        }
    }
}
