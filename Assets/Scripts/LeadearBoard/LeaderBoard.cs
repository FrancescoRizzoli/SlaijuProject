using LootLocker.Requests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.Experimental.GraphView;

namespace LeaderBoard
{
    public class LeaderBoard : MonoBehaviour
    {
        string leaderboardKey = "level1";
        public TMP_Text name;
        public TMP_Text score;

        public void UploadScore(int score)
        {
            StartCoroutine(SubmitScoreRoutine(score));
        }
        IEnumerator SubmitScoreRoutine(int scoreToUpload)
        {
            bool done = false;
            string playerID = PlayerPrefs.GetString("PlayerID");
            LootLockerSDKManager.SubmitScore(playerID, scoreToUpload, leaderboardKey, (response) =>
            {
                if (response.success)
                {
                    Debug.Log("successfullyUploaded Score");
                    done = true;
                }
                else
                {
                    Debug.Log("Failed" + response.errorData.message);
                    done = true;
                }
            });
            yield return new WaitWhile(() => done == false);

        }
        public IEnumerator FectTopHighScore()
        {
            bool done = false;
            LootLockerSDKManager.GetScoreList(leaderboardKey, 10, 0, (response) =>
            {
                if (response.success)
                {
                    string temPlayerNames = "Names\n";
                    string tempPlayerScores = "Score\n";
                    LootLockerLeaderboardMember[] members = response.items;

                    for (int i = 0; i < members.Length; i++)
                    {
                        temPlayerNames += members[i].rank + ".";

                        if (members[i].player.name != "")
                        {
                            temPlayerNames += members[i].player.name;
                        }
                        else
                        {
                            temPlayerNames += members[i].player.id;
                        }
                        tempPlayerScores += members[i].score +"\n";
                        temPlayerNames += "\n";
                        done = true;
                        name.text = temPlayerNames;
                        score.text = tempPlayerScores;
                       
                    }
                }
                else
                {
                    Debug.Log("failed" + response.errorData.message);
                    done = true;
                }

            });
            yield return new WaitWhile(() => done == false);
        }
    }
}
