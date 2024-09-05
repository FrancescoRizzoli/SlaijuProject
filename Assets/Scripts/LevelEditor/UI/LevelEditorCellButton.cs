using Grid;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor
{
    [RequireComponent(typeof(Button))]
    public class LevelEditorCellButton : MonoBehaviour
    {
        protected Button button = null;
        protected BaseCell cellPrefab = null;
        protected LevelEditorController controller = null;

        public void Init(EditorCellType cellType, EditorCell cellData, LevelEditorController editorController)
        {
            controller = editorController;
            button = GetComponent<Button>();
            button.image.sprite = cellData.cellSprite;
            cellPrefab = cellData.cellPrefab;
            SetOnClickEvents(cellType);
        }

        protected void SpawnRequested() => controller.RequestNewCellPositioning(cellPrefab);

        private void SetOnClickEvents(EditorCellType cellType)
        {
            switch (cellType)
            {
                case EditorCellType.Environment:
                    button.onClick.AddListener(controller.currentGrid.EnvironmentCellSelected);
                    break;
                case EditorCellType.Objective:
                    if (cellPrefab.ID == CellID.Start)
                        button.onClick.AddListener(controller.currentGrid.StartCellSelected);
                    else if (cellPrefab.ID == CellID.Exit)
                        button.onClick.AddListener(controller.currentGrid.ExitCellSelected);
                    else
                        button.onClick.AddListener(controller.currentGrid.RoadCellSelected);
                    break;
                case EditorCellType.Road:
                    button.onClick.AddListener(controller.currentGrid.RoadCellSelected);
                    break;
            }

            button.onClick.AddListener(SpawnRequested);
        }
    }
}
