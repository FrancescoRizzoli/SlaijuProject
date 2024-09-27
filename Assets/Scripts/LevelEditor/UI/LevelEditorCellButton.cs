using Grid;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor
{
    public class LevelEditorCellButton : MonoBehaviour
    {
        public Button button = null;
        [SerializeField] private TextMeshProUGUI quantityTextArea = null;

        private int maxQuantity = 1;

        protected LevelEditorController controller = null;
        protected LevelEditorRecentlyUsedCells recentCells = null;
        public BaseCell cellPrefab { get; set; } = null;
        public EditorCellType cellType { get; set; }
        public bool limitQuantity { get; private set; } = false;
        public int currentQuantity { get; private set; }

        public void Init(EditorCellType cellType, EditorCell cellData, LevelEditorController editorController, LevelEditorRecentlyUsedCells recentlyUsedCells)
        {
            this.cellType = cellType;
            currentQuantity = cellData.maxQuantity;
            limitQuantity = cellData.limited;
            controller = editorController;
            recentCells = recentlyUsedCells;
            if (cellType == EditorCellType.Environment || cellType == EditorCellType.Frame)
                button.image.sprite = cellData.cellSprite[0];
            else
                button.image.sprite = cellData.cellSprite[(int)LevelEditorNewLevelSetup.levelColor];
            cellPrefab = cellData.cellPrefab;
            SetOnClickEvents();

            if (limitQuantity)
                quantityTextArea.gameObject.SetActive(true);
        }

        public virtual void Init(LevelEditorController editorController)
        {
            controller = editorController;
        }

        protected void SpawnRequested() => controller.RequestNewCellPositioning(cellPrefab, this);

        protected void HandleRecentCellClick() => recentCells.AddCell(this);

        public void SetOnClickEvents()
        {
            button.onClick.RemoveAllListeners();

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
                case EditorCellType.Frame:
                    button.onClick.AddListener(controller.currentGrid.FrameCellSelected);
                    break;
            }

            button.onClick.AddListener(SpawnRequested);

            if (limitQuantity)
                button.onClick.AddListener(ListenCellPositioning);
            else if (recentCells != null)
                button.onClick.AddListener(HandleRecentCellClick);
        }

        private void ListenCellPositioning()
        {
            controller.positionCellAction.OnCellPositioning += HandleCellPositioned;
        }

        public void UnsubscribeCellPositioning()
        {
            controller.positionCellAction.OnCellPositioning -= HandleCellPositioned;
        }

        public void HandleCellPositioned()
        {
            if (--currentQuantity == 0)
            {
                button.interactable = false;
                UnsubscribeCellPositioning();
            }

            quantityTextArea.text = $"{currentQuantity}/{maxQuantity}";
        }

        public void IncrementQuantityAvailable()
        {
            if (currentQuantity == 0)
                button.interactable = true;

            currentQuantity++;
            quantityTextArea.text = $"{currentQuantity}/{maxQuantity}";
        }

        public void ResetQuantity()
        {
            currentQuantity = maxQuantity;
            quantityTextArea.text = $"{currentQuantity}/{maxQuantity}";
            button.interactable = true;
        }
    }
}
