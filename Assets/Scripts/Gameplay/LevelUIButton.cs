using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Audio;
using Utility;

namespace Gameplay
{
    public class LevelUIButton : MonoBehaviour
    {
        bool isPressed = false;
        Vector3 initialPosition;

        [SerializeField]
        float moveDistance = 0.1f;
        [SerializeField]
        float moveDuration = 0.2f;

        [SerializeField]
        bool animate = true;

        [SerializeField]
        Material pressedMaterial;
        [SerializeField]
        Material normalMaterial;

        [SerializeField]
        AudioMixerGroup mixerGroup;
        [SerializeField]
        AudioClip buttonsfx;

        private Renderer objectRenderer;

        private void Start()
        {
            initialPosition = transform.position;
            objectRenderer = GetComponent<Renderer>();


            if (objectRenderer != null && normalMaterial != null)
            {
                objectRenderer.material = normalMaterial;
            }
        }

        public void TogglePosition()
        {
            if (isPressed)
            {
                if (animate)
                    transform.DOMove(initialPosition, moveDuration).SetUpdate(true);


                if (objectRenderer != null && normalMaterial != null)
                {
                    objectRenderer.material = normalMaterial;
                }
            }
            else
            {
                if (animate)
                    transform.DOMove(initialPosition + Vector3.down * moveDistance, moveDuration).SetUpdate(true);


                if (objectRenderer != null && pressedMaterial != null)
                {
                    objectRenderer.material = pressedMaterial;
                }
            }
            AudioManager.instance.PlayAudioClip(buttonsfx, mixerGroup);
            isPressed = !isPressed;
        }
    }
}
