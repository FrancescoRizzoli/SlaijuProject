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

        private Quaternion originalRotation; 

        void Start()
        {
            
            originalRotation = transform.rotation;
        }

        
        public void ResetRotation()
        {
          
            sphereRigidbody.isKinematic = true;

          
            transform.DORotateQuaternion(originalRotation, duration)
                .OnComplete(() =>
                {
                    
                    sphereRigidbody.isKinematic = false;
                });
        }
    }
}
