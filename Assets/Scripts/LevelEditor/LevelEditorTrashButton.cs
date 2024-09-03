using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor
{
    public class LevelEditorTrashButton : LevelEditorCellButton
    {

        public void Init(LevelEditorController editorController)
        {
            controller = editorController;
            button = GetComponent<Button>();
            button.onClick.AddListener(controller.currentGrid.CellRemoverSelected);
            button.onClick.AddListener(SpawnRequested);
            controller.currentGrid.OnCellPositioned += ToggleButton;
            ToggleButton();
        }

        private void ToggleButton()
        {
            button.interactable = controller.currentGrid.nonEmptyCellsCounter > 0;
        }
    }
}
