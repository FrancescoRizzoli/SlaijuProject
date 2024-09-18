using Cysharp.Threading.Tasks;
using LootLocker.Requests;
using UnityEngine;

namespace LeaderBoard
{
    public class ScoreSender : MonoBehaviour
    {
        public void UploadScore(int score)
        {
            SubmitScoreRoutine(score, "100", "Level1").Forget(); // Use UniTask's Forget() to handle without awaiting
        }
        public void UploadScore(int score, float time, string key)
        {
            string timetext = time.ToString();
            SubmitScoreRoutine(score, timetext, key).Forget(); // Use UniTask's Forget() to handle without awaiting
        }

        private async UniTask SubmitScoreRoutine(int scoreToUpload, string time, string key)
        {
            var scoreCompletionSource = new UniTaskCompletionSource<bool>();
            string playerID = PlayerPrefs.GetString("PlayerID");

            LootLockerSDKManager.SubmitScore(playerID, scoreToUpload, key, (response) =>
            {
                if (response.success)
                {
                    Debug.Log("Successfully Uploaded Score");
                    scoreCompletionSource.TrySetResult(true);
                }
                else
                {
                    Debug.Log("Failed: " + response.errorData.message);
                    scoreCompletionSource.TrySetResult(true);
                }
            });

            await scoreCompletionSource.Task;
        }
    }
}
