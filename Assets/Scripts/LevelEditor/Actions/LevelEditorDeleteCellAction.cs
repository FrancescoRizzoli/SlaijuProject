using Cysharp.Threading.Tasks;
using Grid;

namespace LevelEditor
{
    public class LevelEditorDeleteCellAction : ALevelEditorAction
    {
        public override void HandleClick(BaseCell targetCell)
        {
            controller.currentGrid.PositionCell(controller.newSelectedBaseCell, targetCell).Forget();
            controller.currentGrid.visualCell[previousSelectedCellPosition.x, previousSelectedCellPosition.y].ToggleDeleteCubes(false);
            controller.newSelectedBaseCell.gameObject.SetActive(true);

            if (controller.currentGrid.nonEmptyCellsCounter > 0)
                controller.newSelectedBaseCell = controller.cellSpawner.SpawnCell(null);
            else
            {
                controller.currentGrid.TurnOffVisualCells();
                controller.newSelectedBaseCell = null;
                controller.GoToStandardAction();
            }
        }

        public override void HandleGridVisualization(BaseCell targetCell)
        {
            if (targetCell != null && targetCell.ID == CellID.LevelEditorEmpty)
                return;

            base.HandleGridVisualization(targetCell);
        }

        protected override void ToggleVisuals(bool value)
        {
            controller.currentGrid.visualCell[previousSelectedCellPosition.x, previousSelectedCellPosition.y].ToggleSelectCubes(value);
            controller.currentGrid.visualCell[previousSelectedCellPosition.x, previousSelectedCellPosition.y].ToggleDeleteCubes(!value);
        }
    }
}
