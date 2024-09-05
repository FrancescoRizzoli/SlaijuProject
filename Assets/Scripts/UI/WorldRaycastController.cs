using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{
    public class WorldRaycastController : MonoBehaviour
    {
        [SerializeField]
        LayerMask layerMask;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                DetectClick();
            }
        }

        private void DetectClick()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;


            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Debug.Log(hit.collider.name);
                if ((layerMask.value & (1 << hit.collider.gameObject.layer)) != 0)
                {
                    hit.collider.gameObject.GetComponent<objectClick>().MouseClick();
                }
            }
        }
    }
}
