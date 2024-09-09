using Grid;
using UnityEngine;

namespace LevelEditor
{
    public class LevelEditorController : MonoBehaviour
    {
        [SerializeField] private LevelEditorUIController uiController = null;
        public LevelEditorSpawner cellSpawner = null;
        [SerializeField] private LevelEditorGridPlayerInput playerInput = null;
        [Header("Actions")]
        [SerializeField] private LevelEditorRotateCellAction rotateCellAction = null;
        public LevelEditorPositionCellAction positionCellAction = null;
        public LevelEditorDeleteCellAction deleteCellAction = null;
        [Header("Grid Prefabs")]
        [SerializeField] private LevelEditorGridComponent largeGridPrefab = null;
        [SerializeField] private LevelEditorGridComponent smallGridPrefab = null;

        public LevelEditorGridComponent currentGrid { get; set; } = null;
        public ALevelEditorAction currentAction { get; private set; } = null;

        public BaseCell newSelectedBaseCell { get; set; } = null;

        private bool inputEnabled = false;

        private void Start()
        {
            cellSpawner.Init(this, largeGridPrefab);    // TODO: logic for the correct grid
            currentGrid.OnGridReady += ToggleInput;
            uiController.Init(this, cellSpawner);
            playerInput.Init(this);
            currentAction = rotateCellAction;
        }


        private void Update()
        {
            if (currentAction == null || !inputEnabled)
                return;

            playerInput.ProcessInput();
        }

        public void RequestNewCellPositioning(BaseCell prefab)
        {
            if (newSelectedBaseCell != null)
                Destroy(newSelectedBaseCell.gameObject);

            newSelectedBaseCell = cellSpawner.SpawnCell(prefab);

            if (prefab != null)
                currentAction = positionCellAction;
            else
                currentAction = deleteCellAction;
        }

        public void GoToStandardAction() => currentAction = rotateCellAction;

        public void ResetLevel()
        {
            currentGrid.OnGridReady -= ToggleInput;
            Destroy(currentGrid.gameObject);
            currentGrid = cellSpawner.SpawnGrid(largeGridPrefab);       // TODO: logic for the correct grid
            uiController.ResetUI();
            currentGrid.OnGridReady += ToggleInput;
        }

        public void ToggleInput() => inputEnabled = !inputEnabled;
    }
}
