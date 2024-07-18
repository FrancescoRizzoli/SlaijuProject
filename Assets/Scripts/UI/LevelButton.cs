using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace UnityEngine.UI
{
    public class LevelButton : MonoBehaviour
    {
        [SerializeField]
        GameObject locked;
        [SerializeField]
        Button unlockedButton;
        [SerializeField]
        SceneName level;
        [SerializeField]
        bool defaultUnlock = false;

        private void Awake()
        {
            if (CheckUnlock() || defaultUnlock)
                Unlock();
        }

        private bool CheckUnlock()
        {
            int levelReached = Settings.GetLevelReached();
            Debug.Log("leavle Reached"+ levelReached);
            return (int)level <= levelReached;
        }

        private void Unlock()
        {
            locked.SetActive(false);
            unlockedButton.interactable = true;
        }
    }
}
