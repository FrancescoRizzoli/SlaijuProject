using Grid;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor
{
    [RequireComponent(typeof(Button))]
    public class LevelEditorCellButton : MonoBehaviour
    {
        private Button button = null;
        private BaseCell cellPrefab = null;
        private LevelEditorSpawner cellSpawner;

        public void Init(EditorCellFilterType cellType, EditorCell cellData, LevelEditorController editorController, LevelEditorSpawner cellSpawner)
        {
            button = GetComponent<Button>();
            button.image.sprite = cellData.cellSprite;
            cellPrefab = cellData.cellPrefab;
            this.cellSpawner = cellSpawner;
            SetOnClickEvents(cellType, editorController);
        }

        private void SpawnRequested() => cellSpawner.SpawnCell(cellPrefab);

        private void SetOnClickEvents(EditorCellFilterType cellType, LevelEditorController editorController)
        {
            switch (cellType)
            {
                case EditorCellFilterType.Environment:
                    button.onClick.AddListener(editorController.currentGrid.EnvironmentCellSelected);
                    break;
                case EditorCellFilterType.Objective:
                    if (cellPrefab.ID == CellID.Start)
                        button.onClick.AddListener(editorController.currentGrid.StartCellSelected);
                    else if (cellPrefab.ID == CellID.Exit)
                        button.onClick.AddListener(editorController.currentGrid.ExitCellSelected);
                    else
                        button.onClick.AddListener(editorController.currentGrid.RoadCellSelected);
                    break;
                case EditorCellFilterType.Road:
                    button.onClick.AddListener(editorController.currentGrid.RoadCellSelected);
                    break;
            }

            button.onClick.AddListener(SpawnRequested);
        }
    }
}
