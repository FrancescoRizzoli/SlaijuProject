using Cysharp.Threading.Tasks;
using Grid;
using System;
using UnityEngine;
using Utility;

namespace LevelEditor
{
    public class LevelEditorDeleteCellAction : ALevelEditorAction
    {
        [SerializeField] private AudioClip deleteAudioClip = null;

        public delegate void DeleteCellEvent(Type cell);
        public event DeleteCellEvent OnCellDeleted = null;

        public delegate void DeleteCompletedEvent();
        public event DeleteCompletedEvent OnAllCellsDeleted = null;

        public override void HandleClick(BaseCell targetCell)
        {
            if (targetCell.ID == CellID.LevelEditorEmpty)
                return;

            OnCellDeleted?.Invoke(targetCell.GetType());
            controller.currentGrid.PositionCell(controller.newSelectedBaseCell, targetCell).Forget();
            if (deleteAudioClip != null)
                AudioManager.instance.PlayAudioClip(deleteAudioClip, mixerGroup, PitchType.random);
            controller.currentGrid.visualCell[previousSelectedCellPosition.x, previousSelectedCellPosition.y].ToggleDeleteCubes(false);
            controller.newSelectedBaseCell.gameObject.SetActive(true);
            controller.newSelectedBaseCell = null;

            if (controller.currentGrid.emptyCellsCounter < controller.currentGrid.maxEmptyCellNumber)
                controller.newSelectedBaseCell = controller.cellSpawner.SpawnCell(null);
            else
                OnAllCellsDeleted?.Invoke();
        }

        public override void HandleGridVisualization(BaseCell targetCell)
        {
            if (targetCell != null && targetCell.ID == CellID.LevelEditorEmpty)
                return;

            base.HandleGridVisualization(targetCell);
        }

        protected override void ToggleVisuals(bool value)
        {
            controller.currentGrid.visualCell[previousSelectedCellPosition.x, previousSelectedCellPosition.y].ToggleSelectCubes(value, controller.currentGrid.gridArray[previousSelectedCellPosition.x, previousSelectedCellPosition.y].ID != CellID.LevelEditorEmpty);
            controller.currentGrid.visualCell[previousSelectedCellPosition.x, previousSelectedCellPosition.y].ToggleDeleteCubes(!value);
        }
    }
}
