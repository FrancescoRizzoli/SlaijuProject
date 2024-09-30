using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{
    public class SphereRotationReset : MonoBehaviour
    {
        public Rigidbody sphereRigidbody; 
        public float duration = 2f;
        public float velocityThreshold = 0.01f; 
        public float angularVelocityThreshold = 0.01f;
        public GameObject resetButton;
        bool resetting = false;



        private Vector3 originalPosition;
        private Quaternion originalRotation; 

        void Start()
        {
            
            originalRotation = transform.rotation;
            originalPosition = transform.position;
        }

        
        public void ResetRotation()
        {
          
            sphereRigidbody.isKinematic = true;
            transform.position = originalPosition;
            resetButton.SetActive(false);
            transform.DORotateQuaternion(originalRotation, duration)
                .OnComplete(() =>
                {
                    
                    sphereRigidbody.isKinematic = false;
                    resetting = false;
                });
        }
        public bool IsMoving()
        {
        
            bool isLinearMoving = sphereRigidbody.velocity.magnitude > velocityThreshold;
            bool isAngularMoving = sphereRigidbody.angularVelocity.magnitude > angularVelocityThreshold;
            return isLinearMoving || isAngularMoving;
        }

        void FixedUpdate()
        {
            if (IsMoving() &&!resetting)
            {
                Debug.Log("Il rigidbody è in movimento!");
                resetting = true;
                resetButton.SetActive(true);
            }
            
        }

    }
}
