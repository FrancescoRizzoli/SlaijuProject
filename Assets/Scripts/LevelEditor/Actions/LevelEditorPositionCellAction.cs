using Grid;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using Utility;

namespace LevelEditor
{
    public class LevelEditorPositionCellAction : ALevelEditorAction
    {
        [SerializeField] private AudioClip[] positionAudioClip = new AudioClip[3];

        public delegate void PositionCellEvent();
        public event PositionCellEvent OnCellPositioning = null;

        public delegate void ReplacedCellEvent(Type cell);
        public event ReplacedCellEvent OnCellReplaced = null;

        private int clipIndex = 0;

        public override void HandleClick(BaseCell targetCell)
        {
            OnCellReplaced?.Invoke(targetCell.GetType());
            controller.currentGrid.PositionCell(controller.newSelectedBaseCell, targetCell).Forget();
            if (positionAudioClip.Length > 0)
            {
                AudioManager.instance.PlayAudioClip(positionAudioClip[clipIndex], mixerGroup, PitchType.random);
                clipIndex = (clipIndex + 1) % positionAudioClip.Length;
            }
            OnCellPositioning?.Invoke();
            if (!controller.uiController.lastSelectedButton.limitQuantity || (controller.uiController.lastSelectedButton.limitQuantity && controller.uiController.lastSelectedButton.currentQuantity > 0))
                controller.newSelectedBaseCell = controller.cellSpawner.SpawnCell(controller.newSelectedBaseCell);
            else
            {
                OnCellPositioning = null;
                controller.currentGrid.TurnOffVisualCells();
                controller.newSelectedBaseCell = null;
                controller.GoToStandardAction();
            }

        }

        protected override void ToggleVisuals(bool value)
        {
            controller.newSelectedBaseCell.gameObject.SetActive(!value);
            controller.currentGrid.gridArray[previousSelectedCellPosition.x, previousSelectedCellPosition.y].gameObject.SetActive(value);
            controller.currentGrid.visualCell[previousSelectedCellPosition.x, previousSelectedCellPosition.y].ToggleSelectCubes(value, controller.currentGrid.gridArray[previousSelectedCellPosition.x, previousSelectedCellPosition.y].ID != CellID.LevelEditorEmpty);
        }
    }
}
