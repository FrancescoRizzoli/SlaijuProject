using Grid;
using Cysharp.Threading.Tasks;

namespace LevelEditor
{
    public class LevelEditorPositionCellAction : ALevelEditorAction
    {
        public override void HandleClick(BaseCell targetCell)
        {
            controller.currentGrid.PositionCell(controller.newSelectedBaseCell, targetCell).Forget();
            controller.currentGrid.TurnOffVisualCells();
            controller.newSelectedBaseCell = null;
            controller.GoToStandardAction();
        }

        protected override void ToggleVisuals(bool value)
        {
            controller.newSelectedBaseCell.gameObject.SetActive(!value);
            controller.currentGrid.gridArray[previousSelectedCellPosition.x, previousSelectedCellPosition.y].gameObject.SetActive(value);
            controller.currentGrid.visualCell[previousSelectedCellPosition.x, previousSelectedCellPosition.y].ToggleSelectCubes(value);
        }
    }
}
