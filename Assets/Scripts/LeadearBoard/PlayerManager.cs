using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;

namespace LeaderBoard
{
    public class PlayerManager : MonoBehaviour
    {
        public LeaderBoard leaderBoard;
        private void Start()
        {
            StartCoroutine(SetUpRoutine());
        }
        IEnumerator SetUpRoutine()
        {
            yield return LoginRoutine();
            yield return leaderBoard.FectTopHighScore();
        }
        IEnumerator LoginRoutine()
        {
            bool done = false;
            LootLockerSDKManager.StartGuestSession((response) =>
            {
                if (response.success)
                {
                    Debug.Log("player Loged In");
                    PlayerPrefs.SetString("PlayerID", response.player_id.ToString());
                    done = true;
                }

                else
                {
                    Debug.Log("Could Not Start Sessions");
                    done = true;
                }
            });
            yield return new WaitWhile(() => done == false);
        }

    }
}
