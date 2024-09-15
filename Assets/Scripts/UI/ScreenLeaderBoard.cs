using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LeaderBoard;
namespace UnityEngine.UI
{

    
    public class ScreenLeaderBoard : MonoBehaviour
    {
        [SerializeField]
        LeaderBoardData data;

        public void printInfo()
        {
            Debug.Log(data.playerInfo["Level1"].score);
        }
    }
}
