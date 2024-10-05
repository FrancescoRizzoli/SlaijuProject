using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace UnityEngine.UI
{
    public class LevelButtonLeaderboard : MonoBehaviour
    {
        [SerializeField]
        SceneName level;
        public delegate void ButtonLoadLevel(SceneName level);
        public event ButtonLoadLevel OnButtonLoadLevel;
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(()=> OnButtonLoadLevel?.Invoke(level));
            
        }
        
    }
}
