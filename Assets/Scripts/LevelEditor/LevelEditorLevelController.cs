using Gameplay;
using UnityEngine;
using Utility;

namespace LevelEditor
{
    public class LevelEditorLevelController : LevelController
    {
        public delegate void GameSimulationEvent();
        public event GameSimulationEvent OnGameSimulationOver = null;


        protected override void OnEnable()
        {
            characterStateController.OnDeath += GameOver;
            characterStateController.OnVictory += GameWin;
        }

        protected override void OnDisable()
        {
            characterStateController.OnDeath -= GameOver;
            characterStateController.OnVictory -= GameWin;
        }

        public override void StartGame()
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
        }

        protected override void GameOver()
        {
            characterStateController.transform.position += Vector3.down * 10;
            OnGameSimulationOver?.Invoke();
        }

        protected override void GameWin()
        {
            characterStateController.transform.position += Vector3.down * 10;
            OnGameSimulationOver?.Invoke();
        }
    }
}
