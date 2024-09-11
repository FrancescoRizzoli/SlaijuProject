using Grid;
using System;
using System.Collections.Generic;
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
        [Header("Simulation")]
        [SerializeField] private GridControls gridControls = null;
        [SerializeField] private LevelEditorLevelController levelController = null;

        public LevelEditorGridComponent currentGrid { get; set; } = null;
        public ALevelEditorAction currentAction { get; private set; } = null;

        public BaseCell newSelectedBaseCell { get; set; } = null;

        private bool inputEnabled = false;
        private LevelEditorGridComponent simulationGrid = null;
        private CustomLevelSave customLevelSave = new CustomLevelSave("/SlaijuReborn_CustomLevels.save");

        private void Start()
        {
            customLevelSave.Load();
            cellSpawner.Init(this, largeGridPrefab);    // TODO: logic for the correct grid
            currentGrid.OnGridReady += ToggleInput;
            uiController.Init(this, cellSpawner);
            playerInput.Init(this);
            currentAction = rotateCellAction;
            levelController.OnGameSimulationOver += StopSimulation;
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

        public void StartSimulation()
        {
            currentAction = null;
            uiController.ToggleSimulation(true);

            simulationGrid =  cellSpawner.SpawnGrid(currentGrid);
            simulationGrid.InitializeGrid();
            currentGrid.TurnOffVisualCells();
            currentGrid.gameObject.SetActive(false);

            gridControls.gridComponent = simulationGrid;
            gridControls.positionController = simulationGrid.positionController;
            gridControls.transform.gameObject.SetActive(true);

            levelController.gridComponent = simulationGrid;
            levelController.positionController = simulationGrid.positionController;
            levelController.characterStateController.gameObject.SetActive(true);
            levelController.StartGame();
        }

        public void StopSimulation()
        {
            Destroy(simulationGrid.gameObject);
            levelController.characterStateController.gameObject.SetActive(false);
            levelController.characterStateController.ResetState();
            currentGrid.gameObject.SetActive(true);
            gridControls.transform.gameObject.SetActive(false);
            uiController.ToggleSimulation(false);
            currentAction = rotateCellAction;
        }

        public void SaveLevel()
        {
            List<CustomGridCell> currentGridCells = new List<CustomGridCell>();
            for (int i = 0; i < currentGrid.width; i++)
            {
                for (int j = 0; j < currentGrid.height; j++)
                {
                    if (currentGrid.gridArray[i, j].ID != CellID.LevelEditorEmpty && Array.IndexOf(currentGrid.levelButtonArray, currentGrid.gridArray[i, j]) == -1)
                    {
                        CustomGridCell currentGridCell = new CustomGridCell();
                        currentGridCell.cellName = currentGrid.gridArray[i, j].name;
                        currentGridCell.positionInGrid = new Vector2Int(i,j);
                        currentGridCell.forwardDirection = currentGrid.gridArray[i, j].transform.forward;
                        currentGridCells.Add(currentGridCell);
                    }
                }
            }

            customLevelSave.customGrids.Add(new CustomGrid { gridName = "PippoGrid", gridComplete = false, isLargeGrid = true, gridCells = currentGridCells });
            customLevelSave.Save();
        }
    }
}
