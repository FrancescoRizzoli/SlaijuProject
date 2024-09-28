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

            transform.DORotateQuaternion(originalRotation, duration)
                .OnComplete(() =>
                {
                    
                    sphereRigidbody.isKinematic = false;
                });
        }
    }
}
