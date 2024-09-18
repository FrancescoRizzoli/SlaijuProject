namespace LevelEditor
{
    public class LevelEditorTrashButton : LevelEditorActionButton
    {        
        public override void Init(LevelEditorController editorController)
        {
            base.Init(editorController);
            controller.deleteCellAction.OnAllCellsDeleted += ButtonClicked;
        }

        protected override void ButtonClicked()
        {
            if (clicked)    // exit delete mode
            {
                controller.currentGrid.TurnOffVisualCells();
                if (controller.newSelectedBaseCell != null)
                    Destroy(controller.newSelectedBaseCell.gameObject);
                controller.GoToStandardAction();
            }
            else
            {
                controller.currentGrid.CellRemoverSelected();
                SpawnRequested();
            }

            clicked = !clicked;
        }

        public override void ToggleButton()
        {
            button.interactable = controller.currentGrid.emptyCellsCounter < controller.currentGrid.maxEmptyCellNumber;
        }
    }
}
