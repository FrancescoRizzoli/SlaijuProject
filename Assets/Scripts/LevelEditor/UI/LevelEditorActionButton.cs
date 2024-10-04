using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor
{
    public abstract class LevelEditorActionButton : LevelEditorCellButton
    {

        [SerializeField] protected Animator animator = null;

        public bool clicked { get; set; } = false;

        public override void Init(LevelEditorController editorController)
        {
            controller = editorController;
            button = GetComponent<Button>();
            button.onClick.AddListener(ButtonClicked);
            SubscribeToCurrentGrid();
            ToggleButton();
        }

        public void SubscribeToCurrentGrid()
        {
            controller.currentGrid.OnCellPositioned += ToggleButton;
        }

        protected abstract void ButtonClicked();

        public abstract void ToggleButton();
    }
}
