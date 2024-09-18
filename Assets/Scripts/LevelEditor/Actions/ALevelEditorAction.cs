using Grid;
using UnityEngine;

namespace LevelEditor
{
    [RequireComponent(typeof(LevelEditorController))]
    public abstract class ALevelEditorAction : MonoBehaviour
    {
        protected LevelEditorController controller = null;
        public BaseCell previousSelectedCell { get; set; } = null;
        protected Vector2Int previousSelectedCellPosition;

        private void Awake()
        {
            controller = GetComponent<LevelEditorController>();
        }

        public abstract void HandleClick(BaseCell targetCell);
        public virtual void HandleGridVisualization(BaseCell targetCell)
        {
            if (previousSelectedCell != null)
                ToggleVisuals(true);

            if (targetCell == null)
                return;

            previousSelectedCell = targetCell;
            previousSelectedCellPosition = controller.currentGrid.GetCellIndexes(previousSelectedCell);
            if (controller.newSelectedBaseCell != null)
                controller.newSelectedBaseCell.transform.position = previousSelectedCell.transform.position;
            ToggleVisuals(false);
        }

        protected abstract void ToggleVisuals(bool value);
    }
}
