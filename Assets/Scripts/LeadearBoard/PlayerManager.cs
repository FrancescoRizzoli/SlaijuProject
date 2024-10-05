using UnityEngine;
using LootLocker.Requests;
using Cysharp.Threading.Tasks;

namespace LeaderBoard
{
    public class PlayerManager : MonoBehaviour
    {
        public LeaderBoardData leaderBoard;
        private string loginMessage = "Logging in...";
        private GUIStyle style;
        [SerializeField] private bool showMessage = true;

        private async void Start()
        {
            style = new GUIStyle();
            style.fontSize = 32;
            style.normal.textColor = Color.white;
            style.alignment = TextAnchor.MiddleRight;

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
                    loginMessage = "Player Logged In";
                    PlayerPrefs.SetString("PlayerID", response.player_id.ToString());
                    loginCompletionSource.TrySetResult(true);
                }
                else
                {
                    loginMessage = "Could Not Start Session";
                    loginCompletionSource.TrySetResult(true);
                }
            });

            await loginCompletionSource.Task;
        }

        private void OnGUI()
        {
            if (!showMessage) return;

            float width = 300;
            float height = 50;
            Rect rect = new Rect(10, Screen.height - height - 10, width, height);

            GUI.Label(rect, loginMessage, style);
        }
    }
}
