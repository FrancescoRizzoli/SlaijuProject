using Core;
using Cysharp.Threading.Tasks;
using LeaderBoard;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utility;


namespace Gameplay
{
    public class LevelUiController : MonoBehaviour
    {
        [SerializeField]
        PlayerInput playerInput;
        [SerializeField]
        ChangeCameraCinemachine changeCamera;
        [SerializeField]
        LevelControllerScene scene;
        [SerializeField]
        LevelSpeed levelSpeed;
        [SerializeField]
        LevelUIButton pauseButton;
        [SerializeField]
        LevelUIButton speedUp;
        [SerializeField]
        LevelUIButton cameraChange;
        [SerializeField]
        LevelUIButton reset;
        [SerializeField]
        TMP_Text cityText;
        [SerializeField]
        TMP_Text moves;
        [SerializeField]
        TimeCounter timeCounter;
        [Header("LeaderboardData")]
        [SerializeField]
        LevelPlayerData playerData;
        [Header("Win")]
        [SerializeField]
        TMP_Text timeCounterWin;
        [SerializeField]
        TMP_Text moveWin;
        
        [Header("Pause")]
        [SerializeField]
        TMP_Text timePause;
        [SerializeField]
        TMP_Text movesPause;
        [SerializeField]
        TMP_Text cityTextPause;
        [Header("Loose")]
        [SerializeField]
        TMP_Text moveLoose;
        [SerializeField]
        TMP_Text timeCounterLoose;
        [Header("LevelDataInfo")]
        [SerializeField]
        LevelInfoObject levelInfoObject;
        [SerializeField]
        TMP_Text levelNameText;



        bool pauseState = false;

        private void OnValidate()
        {
            
            if (cameraChange == null)
            {
                changeCamera = GameObject.FindGameObjectWithTag("CameraGroup").GetComponent<ChangeCameraCinemachine>();
            }
        }
        private void Start()
        {
            if (cityText == null || moves == null)
                return;
            cityText.text = "0";
            moves.text = "0";
            
            SetLevelName();
            playerData.GetScore(scene.SceneName.ToString()).Forget();
        }

        private void OnEnable()
        {
            if (playerInput != null)
                playerInput.onObjectClicked += ButtonClick;
        }

        private void OnDisable()
        {
            if (playerInput != null)
                playerInput.onObjectClicked -= ButtonClick;
        }

        [ContextMenu("Toggle pause")]
        public void TogglePause()
        {
            if (!Application.isPlaying)
                return;

           
            pauseButton.TogglePosition();
            pauseState = !pauseState;
            Debug.Log("pauseState" + pauseState);
            playerInput.enabled = !pauseState;
            UIManager.instance.ManageScreen(ScreenType.Pause, pauseState);
            timePause.text = timeCounter.elapsedTime.ToString("F2") +"/"+ playerData.timeText;
            movesPause.text = moves.text+ "/" + playerData.movesText;
            cityTextPause.text = cityText.text;



            if (pauseState)
                levelSpeed.Freeze();
            else
                levelSpeed.SetLevelSpeed();
        }

        public void GameWin()
        {
            playerInput.enabled = false;
            UIManager.instance.ManageScreen(ScreenType.GameWin, true);
            timeCounterWin.text = timeCounter.elapsedTime.ToString("F2") + "/" + playerData.timeText;
            moveWin.text = moves.text + "/" + playerData.movesText;
        }

        public void GameOver()
        {
            
            playerInput.enabled = false;
            Debug.Log(timeCounterLoose.text = timeCounter.elapsedTime.ToString());
            timeCounterLoose.text = timeCounter.elapsedTime.ToString("F2") + "/" + playerData.timeText;
            UIManager.instance.ManageScreen(ScreenType.GameOver, true);
            moveLoose.text = moves.text + "/" + playerData.movesText;

        }

        public void SpeedUp()
        {
            speedUp.TogglePosition();
            levelSpeed.IncrementSpeed();

        }

        public void DestroyCity(int value)
        {
            cityText.text = value.ToString();
        }

        private void ButtonClick(GameObject objectClicked)
        {


            if (pauseButton != null && objectClicked == pauseButton.gameObject)
            {
                TogglePause();
            }
            else if (speedUp != null && objectClicked == speedUp.gameObject)
            {
                SpeedUp();
            }
            else if (cameraChange != null && objectClicked == cameraChange.gameObject)
            {
                CameraChange();
            }
            else if (reset != null && objectClicked == reset.gameObject)
            {
                ResetLevel();
            }

        }

        private void CameraChange()
        {
            cameraChange.TogglePosition();
            changeCamera.Switch();
            // Implement camera change logic
        }

        public void ResetLevel()
        {
            playerInput.enabled = false;
            reset.TogglePosition();
            scene.Reload();
        }
        public void UpdateCityText(int remainingCity, int numberOfcity)
        {
            if (cityText == null)
                return;

            remainingCity = numberOfcity - remainingCity;
            cityText.text = remainingCity.ToString() + "/" + numberOfcity.ToString();
        }
    
        public void UpdateMoves(int moveNumber)
        {
            if (moves == null)
                return;

            moves.text = moveNumber.ToString();
        }

        private void SetLevelName()
        {
            LevelInfo info = levelInfoObject.GetLevelInfoBySceneName(scene.SceneName);
            if (levelNameText != null && SceneManager.GetActiveScene().buildIndex != (int)SceneName.CustomLevelPlay)
                levelNameText.text = info.name;
        }

    }
}
