using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{
    [RequireComponent(typeof(WorldRaycastController))]
    public class WorldUiEnabler : MonoBehaviour
    {
     
        WorldRaycastController worldRaycastController;

        private void Awake()
        {
            worldRaycastController = GetComponent<WorldRaycastController>();
        }

        private void OnEnable()
        {
            UIManager.instance.onScreenChange += ToggleCOmponent;
        }
        private void OnDisable()
        {
            UIManager.instance.onScreenChange -= ToggleCOmponent;
        }

        private void ToggleCOmponent(int value)
        {
            if (value == 0)
                worldRaycastController.enabled = true;
            else
                worldRaycastController.enabled = false;
        }
    }
}
