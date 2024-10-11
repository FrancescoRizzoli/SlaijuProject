using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Gameplay
{
    public class TimeCounter : MonoBehaviour
    {
       
    public float elapsedTime = 0.0f;
        public bool isRunning = false;
        [SerializeField]
        LevelSpeed levelSpeed;
        [SerializeField]
        TMP_Text timeCounterUI;

        private void Start()
        {
           
            
        }

        public async UniTaskVoid StartStopwatchAsync()
        {
            isRunning = true;

            while (isRunning)
            {
               
                if (levelSpeed.GetGameSpeed() < 1)
                {
                    await UniTask.Yield();
                    continue;
                }
                elapsedTime += Time.unscaledDeltaTime;
                timeCounterUI.text = (elapsedTime.ToString("F2")); 
                await UniTask.Yield(); 
            }
        }

        public void StopStopwatch()
        {
            isRunning = false;
        }

        public void ResetStopwatch()
        {
            elapsedTime = 0.0f;
            
        }
    }
}
