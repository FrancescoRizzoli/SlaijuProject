using Grid;
using Cysharp.Threading.Tasks;

namespace LevelEditor
{
    public class LevelEditorPositionCellAction : ALevelEditorAction
    {
        public delegate void PositionCellEvent();
        public event PositionCellEvent OnCellPositioning = null;

        public override void HandleClick(BaseCell targetCell)
        {
            controller.currentGrid.PositionCell(controller.newSelectedBaseCell, targetCell).Forget();
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
            controller.currentGrid.visualCell[previousSelectedCellPosition.x, previousSelectedCellPosition.y].ToggleSelectCubes(value);
        }
    }
}
