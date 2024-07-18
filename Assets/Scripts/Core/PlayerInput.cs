using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class PlayerInput : MonoBehaviour
    {
        Camera cam;
        Vector3 pos = new Vector3(200, 200, 0);

        public delegate void objectClicked(GameObject clickedObject);
        public event objectClicked onObjectClicked;

        void Update()
        {
            // Check for mouse input

#if UNITY_STANDALONE_WIN
            if (Input.GetButtonDown("Fire1"))
            {
                HandleInput(Input.mousePosition);
            }
#endif
#if UNITY_IOS || UNITY_ANDROID
            // Check for touch input
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    HandleInput(touch.position);
                }
            }
#endif
        }

        void HandleInput(Vector3 inputPosition)
        {
            Ray ray = Camera.main.ScreenPointToRay(inputPosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                onObjectClicked?.Invoke(hit.collider.gameObject);
                Debug.Log(hit.collider.gameObject.name);
                //Instantiate(particle, hit.point, transform.rotation); // Create a particle if hit
            }
        }
    }
}
