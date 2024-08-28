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


namespace Gameplay
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField]
        GridComponent gridComponent;
        [SerializeField]
        CharacterStateController characterStateController;
        [SerializeField]
        GridPositionController positionController;
        [SerializeField]
        LevelControllerScene levelControllerScene;
        [SerializeField]
        LevelSpeed levelSpeed;
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
        private Vector3 monsterStartOffset = Vector3.zero;
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
        AudioComponent audioComponent;



        [ContextMenu("Start Counter")]
        public void StartCounter()
        {
            counter.StartCounter().Forget();
        }


        [ContextMenu("start game")]
        public void StartGame()
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
        private void OnEnable()
        {
            counter.OnCounterEnd += StartGame;
            SceneLoader.OnLoadingCompleted += StartCounter;
            characterStateController.OnDeath += GameOver;
            characterStateController.OnVictory += GameWin;
            gridcontrols.onPlayerMoves += UpdateMoves;
        }
        private void OnDisable()
        {
            counter.OnCounterEnd -= StartGame;
            SceneLoader.OnLoadingCompleted -= StartCounter;
            characterStateController.OnDeath -= GameOver;
            characterStateController.OnVictory -= GameWin;
            gridcontrols.onPlayerMoves -= UpdateMoves;

        }

        private void SetUpCity()
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


        private void GameWin()
        {
            playerInput.enabled = false;
            Settings.UpdateLevelReached((int)levelControllerScene.SceneName +1);
            AudioManager.instance.PlayAudioClip(win, sfx);
            levelUiController.GameWin();
            timeCounter.StopStopwatch();
            levelSpeed.Freeze();
        }
        private void GameOver()
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
