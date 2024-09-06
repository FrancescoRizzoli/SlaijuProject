using UnityEngine.UI;

namespace LevelEditor
{
    public class LevelEditorTrashButton : LevelEditorCellButton
    {
        private bool clicked = false;
        
        public override void Init(LevelEditorController editorController)
        {
            controller = editorController;
            button = GetComponent<Button>();
            button.onClick.AddListener(TrashButtonClicked);
            controller.currentGrid.OnCellPositioned += ToggleButton;
            controller.deleteCellAction.OnAllCellsDeleted += TrashButtonClicked;
            ToggleButton();
        }

        private void TrashButtonClicked()
        {
            if (clicked)    // exit delete mode
            {
                controller.currentGrid.TurnOffVisualCells();
                controller.newSelectedBaseCell = null;
                controller.GoToStandardAction();
            }
            else
            {
                controller.currentGrid.CellRemoverSelected();
                SpawnRequested();
            }

            clicked = !clicked;
        }

        private void ToggleButton()
        {
            button.interactable = controller.currentGrid.nonEmptyCellsCounter > 0;
        }
    }
}
