using Core;
using TMPro;
using UnityEngine;
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
            timePause.text = timeCounter.elapsedTime.ToString("F2");
            movesPause.text = moves.text;
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
            timeCounterWin.text = timeCounter.elapsedTime.ToString("F2");
            moveWin.text = moves.text;
        }

        public void GameOver()
        {
            
            playerInput.enabled = false;
            Debug.Log(timeCounterLoose.text = timeCounter.elapsedTime.ToString());
            timeCounterLoose.text = timeCounter.elapsedTime.ToString("F2");
            UIManager.instance.ManageScreen(ScreenType.GameOver, true);
            moveLoose.text = moves.text;
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


            if (objectClicked == pauseButton.gameObject)
            {
                TogglePause();
            }
            else if (objectClicked == speedUp.gameObject)
            {
                SpeedUp();
            }
            else if (objectClicked == cameraChange.gameObject)
            {
                CameraChange();
            }
            else if (objectClicked == reset.gameObject)
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
            reset.TogglePosition();
            scene.Reload();
        }
        public void UpdateCityText(int remainingCity, int numberOfcity)
        {
            remainingCity = numberOfcity - remainingCity;
            cityText.text = remainingCity.ToString() + "/" + numberOfcity.ToString();
        }
    
        public void UpdateMoves(int moveNumber)
        {
            moves.text = moveNumber.ToString();
        }


    }
}
