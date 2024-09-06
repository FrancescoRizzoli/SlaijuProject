using Grid;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor
{
    public class LevelEditorCellButton : MonoBehaviour
    {
        [SerializeField] protected Button button = null;
        [SerializeField] private TextMeshProUGUI quantityTextArea = null;

        private bool limitQuantity = false;
        private int maxQuantity = 1;
        private int currentQuantity;

        protected LevelEditorController controller = null;
        public BaseCell cellPrefab { get; protected set; } = null;

        public void Init(EditorCellType cellType, EditorCell cellData, LevelEditorController editorController)
        {
            currentQuantity = cellData.maxQuantity;
            limitQuantity = cellData.limited;
            controller = editorController;
            button.image.sprite = cellData.cellSprite;
            cellPrefab = cellData.cellPrefab;
            SetOnClickEvents(cellType);

            if (limitQuantity)
                quantityTextArea.gameObject.SetActive(true);
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

            if (limitQuantity)
                button.onClick.AddListener(ListenCellPositioning);
        }

        private void ListenCellPositioning()
        {
            controller.positionCellAction.OnCellPositioning += HandleCellPositioned;
        }

        private void HandleCellPositioned()
        {
            if (--currentQuantity == 0)
                button.interactable = false;

            quantityTextArea.text = $"{currentQuantity}/{maxQuantity}";
        }

        public void IncrementQuantityAvailable()
        {
            if (currentQuantity == 0)
                button.interactable = true;

            currentQuantity++;
            quantityTextArea.text = $"{currentQuantity}/{maxQuantity}";
        }
    }
}
