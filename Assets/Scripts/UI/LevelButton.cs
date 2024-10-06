using Core;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Utility;
using LeaderBoard;

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
        GameObject completitionFlower;
        
        [SerializeField]
        TMP_Text timeDev;
        [SerializeField]
        TMP_Text moveDev;
        [SerializeField]
        LeaderBoardData leaderBoard;
        [SerializeField]
        TMP_Text timePlayer;
        [SerializeField]
        TMP_Text movePlayer;
        [SerializeField]
        string emptyText = "-----";

        private void Awake()
        {
            if (CheckUnlock() || defaultUnlock)
            {
                Unlock();
                CheckComplete();
            }
            


            LevelInfo info = levelInfo.GetLevelInfoBySceneName(level);
            if (info == null)
                return;

            timeDev.text = info.time.ToString();
            moveDev.text = info.Moves.ToString();
            leaderBoard.OnDataCollect +=  SetPlayerData;
        }
        

        private bool CheckUnlock()
        {
            int levelReached = Settings.GetLevelReached();
            Debug.Log("leavle Reached"+ levelReached);
            return (int)level <= levelReached;
        }
        private void CheckComplete()
        {
            int levelReached = Settings.GetLevelReached();
          
            if((int)level < levelReached && completitionFlower!=null)
            {
                completitionFlower.SetActive(true);
            }
        }

        private void Unlock()
        {
            locked.SetActive(false);
            unlockedButton.interactable = true;
        }

        private void SetPlayerData()
        {
             if(leaderBoard.playerInfo[level.ToString()].metadata != null)
            {
               
               movePlayer.text = leaderBoard.playerInfo[level.ToString()].score.ToString();
               timePlayer.text = leaderBoard.playerInfo[level.ToString()].metadata.ToString();
            }
            else
            {
                movePlayer.text = emptyText;
                timePlayer.text = emptyText;

            }
           
        }
    }
}
