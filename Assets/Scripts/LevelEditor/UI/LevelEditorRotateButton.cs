namespace LevelEditor
{
    public class LevelEditorRotateButton : LevelEditorActionButton
    {
        public override void ToggleButton()
        {
            button.interactable = controller.currentGrid.emptyCellsCounter < controller.currentGrid.maxEmptyCellNumber;
        }

        protected override void ButtonClicked()
        {
            if (clicked)    // exit rotate mode
                controller.GoToStandardAction();
            else
            {
                controller.currentGrid.TurnOffVisualCells();
                controller.GoToRotateAction();
            }

            clicked = !clicked;
            animator.SetBool("Clicked", clicked);
        }
    }
}
