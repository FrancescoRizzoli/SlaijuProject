using Grid;
using System;
using UnityEngine;

namespace LevelEditor
{
    public class LevelEditorRotateCellAction : ALevelEditorAction
    {
        public override void HandleClick(BaseCell targetCell)
        {
            if (targetCell.ID == CellID.LevelEditorEmpty || targetCell.ID == CellID.Start || targetCell.ID == CellID.Exit || Array.IndexOf(controller.currentGrid.levelButtonArray, targetCell) != -1)
                return;
            
            targetCell.transform.Rotate(new Vector3(0.0f, 90.0f, 0.0f));
        }

        public override void HandleGridVisualization(BaseCell targetCell)
        {
            if (targetCell != null && (targetCell.ID == CellID.LevelEditorEmpty || targetCell.ID == CellID.Start || targetCell.ID == CellID.Exit || Array.IndexOf(controller.currentGrid.levelButtonArray, targetCell) != -1))
            {
                ToggleVisuals(true);
                return;
            }

            base.HandleGridVisualization(targetCell);
        }

        protected override void ToggleVisuals(bool value)
        {
            controller.currentGrid.visualCell[previousSelectedCellPosition.x, previousSelectedCellPosition.y].ToggleRotateView(!value);
        }
    }
}
