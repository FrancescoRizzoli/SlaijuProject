using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor
{
    [RequireComponent(typeof(Button))]
    public class CustomLevelLoaderButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textArea = null;

        private CustomLevelLoaderController controller = null;
        private CustomLevelLoaderUIController uiController = null;
        private Button button = null;

        public CustomGrid customGrid { get; private set; }

        public void Init(CustomGrid grid, CustomLevelLoaderController controller, CustomLevelLoaderUIController uiController)
        {
            customGrid = grid;
            this.controller = controller;
            this.uiController = uiController;
            button = GetComponent<Button>();
            textArea.text = grid.gridName;
            button.onClick.AddListener(RequestCustomGrid);
            button.onClick.AddListener(TogglePlayButton);
        }

        private void RequestCustomGrid() => controller.ShowCustomGrid(customGrid);
        private void TogglePlayButton() => uiController.ToggleButtons(customGrid.gridComplete);
    }
}
