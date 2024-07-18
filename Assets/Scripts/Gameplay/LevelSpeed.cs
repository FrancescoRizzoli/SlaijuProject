using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class LevelSpeed : MonoBehaviour
    {
        [SerializeField]
        private float[] LevelSpeeds;
        private int currentSpeed = 0;
        private float currentLevelSpeed;

        private void Awake()
        {
            if (LevelSpeeds == null || LevelSpeeds.Length == 0)
            {
                Debug.LogError("Speed not Initialized");
                return;
            }
            currentLevelSpeed = LevelSpeeds[currentSpeed];
            SetLevelSpeed();
        }

        public void SetLevelSpeed()
        {
            Time.timeScale = currentLevelSpeed;
        }

        public void Freeze()
        {
            Time.timeScale = 0;
        }

        public void IncrementSpeed()
        {
            currentSpeed = (currentSpeed + 1) % LevelSpeeds.Length;
            currentLevelSpeed = LevelSpeeds[currentSpeed];
            SetLevelSpeed();
        }

        public void DecrementSpeed()
        {
            currentSpeed = (currentSpeed - 1 + LevelSpeeds.Length) % LevelSpeeds.Length;
            currentLevelSpeed = LevelSpeeds[currentSpeed];
            SetLevelSpeed();
        }
        public float GetCurrentSpeed()
        {
            return currentLevelSpeed;
        }
    }
}


