using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public class BirdOrbit : MonoBehaviour
    {
        [SerializeField]
        Transform orbitCenter;
        [SerializeField]
        float orbitRadius = 5f;
        [SerializeField]
        float orbitDuration = 5f;
        [SerializeField]
        int orbitDirection = 1;

        void Start()
        {
            Vector3 startPosition = orbitCenter.position + new Vector3(orbitRadius, 0, 0);
            transform.position = startPosition;
            Vector3 initialOffset = transform.position - orbitCenter.position;
            initialOffset = Quaternion.Euler(0, orbitDirection * 360f * (Time.deltaTime / orbitDuration), 0) * initialOffset;
            Vector3 initialPosition = orbitCenter.position + initialOffset;

            transform.position = initialPosition;
            Tweener orbitTween = DOTween.To(() => 0f, x =>
            {
                float angle = x * 360f * orbitDirection;
                float radians = angle * Mathf.Deg2Rad;
                Vector3 offset = new Vector3(Mathf.Cos(radians), 0, Mathf.Sin(radians)) * orbitRadius;
                Vector3 newPosition = orbitCenter.position + offset;

                Vector3 direction = newPosition - transform.position;
                transform.position = newPosition;


                if (direction != Vector3.zero)
                {
                    Quaternion lookRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
                }

            }, 1f, orbitDuration).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
        }
    }
}
