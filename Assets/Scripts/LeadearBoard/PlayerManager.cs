using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
using Cysharp.Threading.Tasks; // Add UniTask namespace

namespace LeaderBoard
{
    public class PlayerManager : MonoBehaviour
    {
        public LeaderBoardData leaderBoard;

        private async void Start()
        {
            await SetUpRoutine();
        }

        private async UniTask SetUpRoutine()
        {
            await LoginRoutine();
            await leaderBoard.SetUpScoreDictionarys();
        }

        private async UniTask LoginRoutine()
        {
            var loginCompletionSource = new UniTaskCompletionSource<bool>();

            LootLockerSDKManager.StartGuestSession((response) =>
            {
                if (response.success)
                {
                    Debug.Log("Player Logged In");
                    PlayerPrefs.SetString("PlayerID", response.player_id.ToString());
                    loginCompletionSource.TrySetResult(true);
                }
                else
                {
                    Debug.Log("Could Not Start Session");
                    loginCompletionSource.TrySetResult(true);
                }
            });

            await loginCompletionSource.Task;
        }
    }
}
