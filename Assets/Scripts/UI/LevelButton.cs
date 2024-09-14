using Core;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Utility;

namespace UnityEngine.UI
{
    public class LevelButton : MonoBehaviour
    {
        [SerializeField]
        LevelInfoObject levelInfo;
        [SerializeField]
        GameObject locked;
        [SerializeField]
        Button unlockedButton;
        [SerializeField]
        SceneName level;
        [SerializeField]
        bool defaultUnlock = false;
        [SerializeField]
        TMP_Text timeDev;
        [SerializeField]
        TMP_Text moveDev;

        private void Awake()
        {
            if (CheckUnlock() || defaultUnlock)
                Unlock();
            
            LevelInfo info = levelInfo.GetLevelInfoBySceneName(level);
            if (info == null)
                return;

            timeDev.text = info.time.ToString();
            moveDev.text = info.Moves.ToString();
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
