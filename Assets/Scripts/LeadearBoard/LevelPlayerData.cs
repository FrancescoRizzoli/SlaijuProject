using LootLocker.Requests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace LeaderBoard
{
    public class LevelPlayerData : MonoBehaviour
    {
        LootLockerGetMemberRankResponse playerLvelInfo;
        public string movesText {  get; set; }
        public string timeText {  get; set; }
        [SerializeField]
        string missingScoreText = "Unranked";

        public async UniTask GetScore(string key)
        {
            Debug.Log(key);
            var fetchCompletionSource = new UniTaskCompletionSource<bool>();
            string playerID = PlayerPrefs.GetString("PlayerID");

            LootLockerSDKManager.GetMemberRank(key, playerID, (response) =>
            {
                if (response.success)
                {

                    playerLvelInfo = response;
                    fetchCompletionSource.TrySetResult(true);
                    movesText = playerLvelInfo.score.ToString();
                    timeText = playerLvelInfo.metadata;

                }
                else
                {
                    Debug.LogWarning("Failed: " + response.errorData.message);
                    Debug.LogWarning(key);
                    fetchCompletionSource.TrySetResult(true);
                    playerLvelInfo = response;
                    movesText = missingScoreText;
                    timeText = missingScoreText;

                }
            });



            await fetchCompletionSource.Task;
        }

    }


}
