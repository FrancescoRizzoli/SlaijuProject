using Character;
using Grid;
using Core;
using UnityEngine;
using System.Collections.Generic;
using Utility;
using Grid.Cell;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine.Audio;
using LeaderBoard;
using UnityEngine.SceneManagement;


namespace Gameplay
{
    public class LevelController : MonoBehaviour
    {
        public GridComponent gridComponent;
        public CharacterStateController characterStateController;
        public GridPositionController positionController;
        [SerializeField]
        LevelControllerScene levelControllerScene;
        [SerializeField]
        protected LevelSpeed levelSpeed;
        [SerializeField]
        GridControls gridcontrols;
        [SerializeField]
        LevelUiController levelUiController;
        [SerializeField]
        PlayerInput playerInput;
        [SerializeField]
        TimeCounter timeCounter;
        [SerializeField]
        Counter counter;
        [SerializeField]
        ScoreSender sender;
        protected Vector3 monsterStartOffset = new Vector3(0.0f, 1.09f, 0.0f);
        private int cityNumber = 0;
        private int numberOfCity = 0;

        [Header("Audio")]
        [SerializeField]
        AudioMixerGroup sfx;
        [SerializeField]
        AudioClip win;
        [SerializeField]
        AudioClip loose;
        [SerializeField]
        protected AudioComponent audioComponent;
        
        private void Start()
        {
            Debug.Log(levelControllerScene.SceneName.ToString());
        }

        [ContextMenu("Start Counter")]
        public void StartCounter()
        {
            counter.StartCounter().Forget();
        }


        [ContextMenu("start game")]
        public virtual void StartGame()
        {
           
            if (!Application.isPlaying)
                return;
            audioComponent.PlayAudioWithPredefinedSourceEnum(AudioIdentifier.audio1);
            Transform startCellTransform = gridComponent.GetStartCell().transform;
            characterStateController.transform.position = startCellTransform.position + monsterStartOffset;
            characterStateController.transform.forward = startCellTransform.right;
            positionController.Init(characterStateController, gridComponent);
            levelSpeed.SetLevelSpeed();
            SetUpCity();
            timeCounter.StartStopwatchAsync().Forget();

        }

        public void PuaseGame()
        {
            levelSpeed.Freeze();
        }
        public void PlayGame()
        {
            levelSpeed.SetLevelSpeed();
        }
        protected virtual void OnEnable()
        {
            counter.OnCounterEnd += StartGame;
            SceneLoader.OnLoadingCompleted += StartCounter;
            characterStateController.OnDeath += GameOver;
            characterStateController.OnVictory += GameWin;
            gridcontrols.onPlayerMoves += UpdateMoves;
        }
        protected virtual void OnDisable()
        {
            counter.OnCounterEnd -= StartGame;
            SceneLoader.OnLoadingCompleted -= StartCounter;
            characterStateController.OnDeath -= GameOver;
            characterStateController.OnVictory -= GameWin;
            gridcontrols.onPlayerMoves -= UpdateMoves;
        }

        protected void SetUpCity()
        {

            List<BaseCell> city = gridComponent.GetCellsByID(CellID.City);
            if (city == null)
                Debug.LogError("Missing city in grid");

            cityNumber = city.Count;
            numberOfCity = city.Count;
            levelUiController.UpdateCityText(cityNumber, numberOfCity);
            foreach (CityCell cityCell in city)
            {
                cityCell.OnDestruction += CityDestroyed;
            }

            List<BaseCell> generator = gridComponent.GetCellsByID(CellID.Generator);


            if (generator == null)
                return;
            foreach (GeneratorCell generatorCell in generator)
            {
                generatorCell.OnDestruction += GeneratorDestruction;
            }
        }
        [ContextMenu("destroy City")]
        private void CityDestroyed()
        {
            if (!Application.isPlaying)
                return;
            cityNumber--;
            levelUiController.UpdateCityText(cityNumber,numberOfCity);
            gridcontrols.ResetCrossSelect();
            if (cityNumber <= 0)
            {
                ((ExitCell)gridComponent.GetExitCell()).ActivateExit().Forget();
                Debug.Log("allCityDestroyed");
            }

        }

        private void GeneratorDestruction()
        {
            gridcontrols.ResetCrossSelect();
            Debug.Log("generator Destroyed");
        }


        protected virtual void GameWin()
        {
            playerInput.enabled = false;
            if (SceneManager.GetActiveScene().buildIndex != (int)SceneName.CustomLevelPlay)
            {
                Settings.UpdateLevelReached((int)levelControllerScene.SceneName +1);
                sender.UploadScore(gridcontrols.move, timeCounter.elapsedTime, levelControllerScene.SceneName.ToString());
            }
            AudioManager.instance.PlayAudioClip(win, sfx);
            levelUiController.GameWin();
            timeCounter.StopStopwatch();
            levelSpeed.Freeze();
        }
        protected virtual void GameOver()
        {
            playerInput.enabled = false;
            AudioManager.instance.PlayAudioClip(loose, sfx);
            CameraShake.ShakeCinemachineTransform();
            levelUiController.GameOver();
            timeCounter.StopStopwatch();
            levelSpeed.Freeze();
            
        }

        private void UpdateMoves(int move)
        {
             levelUiController.UpdateMoves(move);
        }
        private void TimeCounter(int move)
        {
            levelUiController.UpdateMoves(move);
        }
    }
}
