using LootLocker.Requests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;
using Core; // Add UniTask namespace

namespace LeaderBoard
{
    public class LeaderBoardData : MonoBehaviour
    {
        [SerializeField]
        LevelInfoObject levelInfo;


        public Dictionary<string, LootLockerLeaderboardMember[]> leaderboardInfo = new Dictionary<string, LootLockerLeaderboardMember[]>();
        public Dictionary<string, LootLockerGetMemberRankResponse> playerInfo = new Dictionary<string, LootLockerGetMemberRankResponse>();

       

        public async UniTask SetUpScoreDictionarys()
        {
            leaderboardInfo.Clear();
            playerInfo.Clear();
            foreach(LevelInfo levels in levelInfo.levels)
            {
           
                await AddScoreToDictionaryLeaderboard(levels.sceneName.ToString());
             
                await AddScoreToDictionaryPlayer(levels.sceneName.ToString());

            }
        }
        /*
        public async UniTask FetchTopHighScore(string key)
        {
            var fetchCompletionSource = new UniTaskCompletionSource<bool>();

            LootLockerSDKManager.GetScoreList(key, 10, 0, (response) =>
            {
                if (response.success)
                {
                    string tempPlayerNames = "Names\n";
                    string tempPlayerScores = "Score\n";
                    LootLockerLeaderboardMember[] members = response.items;

                    for (int i = 0; i < members.Length; i++)
                    {
                        tempPlayerNames += members[i].rank + ".";

                        if (!string.IsNullOrEmpty(members[i].player.name))
                        {
                            tempPlayerNames += members[i].player.name;
                        }
                        else
                        {
                            tempPlayerNames += members[i].player.id;
                        }
                        tempPlayerScores += members[i].score + "\n";
                        tempPlayerNames += "\n";
                    }

                    name.text = tempPlayerNames;
                    score.text = tempPlayerScores;

                    fetchCompletionSource.TrySetResult(true);
                }
                else
                {
                    Debug.Log("Failed: " + response.errorData.message);
                    fetchCompletionSource.TrySetResult(true);
                }
            });

            await fetchCompletionSource.Task;
        }
        */
        public async UniTask AddScoreToDictionaryLeaderboard(string key)
        {
            var fetchCompletionSource = new UniTaskCompletionSource<bool>();
            LootLockerSDKManager.GetScoreList(key, 10, 0, (response) =>
            {
           
                if (response.success)
                {
                   
                    LootLockerLeaderboardMember[] members = response.items;
                    leaderboardInfo.Add(key, members);
                    fetchCompletionSource.TrySetResult(true);

                }
                else
                {
                    Debug.Log("Failed: " + response.errorData.message);
                    fetchCompletionSource.TrySetResult(true);
                    leaderboardInfo.Add(key, null);
                }
            });
           


            await fetchCompletionSource.Task;
        }
        public async UniTask AddScoreToDictionaryPlayer(string key)
        {
            var fetchCompletionSource = new UniTaskCompletionSource<bool>();
            string playerID = PlayerPrefs.GetString("PlayerID");
           
            LootLockerSDKManager.GetMemberRank(key,playerID, (response) =>
            {
                if (response.success)
                {
                    
                    playerInfo.Add(key, response);
                    fetchCompletionSource.TrySetResult(true);

                }
                else
                {
                    Debug.LogWarning("Failed: " + response.errorData.message);
                    Debug.LogWarning(key);
                    fetchCompletionSource.TrySetResult(true);
                    playerInfo.Add(key, response);

                }
            });



            await fetchCompletionSource.Task;
        }
    }
}
