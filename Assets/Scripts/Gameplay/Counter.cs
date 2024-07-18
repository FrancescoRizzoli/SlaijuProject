using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks; 

namespace Gameplay
{
    public class Counter : MonoBehaviour
    {
        [SerializeField]
        private PlayerInput playerInput;
        [SerializeField]
        private GameObject CounterCanvas;
        [SerializeField]
        private int startValue;
        [SerializeField]
        private Text text;

        public delegate void CounterEnd();
        public event CounterEnd OnCounterEnd;


        public async UniTaskVoid StartCounter()
        {
            
            playerInput.enabled = false;
            CounterCanvas.SetActive(true);

            
            for (int i = startValue; i >= 0; i--)
            {
                text.text = i.ToString();
                await UniTask.Delay(1000, ignoreTimeScale: true); 
            }

            playerInput.enabled = true;
            CounterCanvas.SetActive(false);
            OnCounterEnd?.Invoke();
        }
    }
}
