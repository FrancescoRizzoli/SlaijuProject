using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{
    public class PlanetRotation : MonoBehaviour
    {
        [SerializeField]
        float rotationSpeed = 10;
        bool drag = false;
        Rigidbody rb;
        private Vector3 lastMousePosition;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void OnMouseDrag()
        {
            drag = true;
            lastMousePosition = Input.mousePosition;
        }

        private void Update()
        {
            if(Input.GetMouseButtonUp(0))
            {
                drag = false;
            }
        }

        private void FixedUpdate()
        {
            if (drag)
            {
                Vector3 mouseDelta = Input.mousePosition - lastMousePosition;  
                lastMousePosition = Input.mousePosition;  

                float x = -mouseDelta.y * rotationSpeed * Time.fixedDeltaTime;
                float y = mouseDelta.x * rotationSpeed * Time.fixedDeltaTime;

                rb.AddTorque(-Vector3.up * y);  
                rb.AddTorque(-Vector3.right * x);
            }
        }
    }
}
