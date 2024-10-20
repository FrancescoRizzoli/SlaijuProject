using Cinemachine;
using Core;
using Cysharp.Threading.Tasks;
using Grid;
using Grid.Cell;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LevelEditor
{
    public enum LevelColor
    {
        Green,
        Pink,
        Blue,
        Red
    }

    public class LevelEditorController : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField] private bool ignoreSceneLoading = false;
#endif
        public LevelEditorUIController uiController = null;
        public LevelEditorSpawner cellSpawner = null;
        [SerializeField] private LevelEditorGridPlayerInput playerInput = null;
        [Header("Actions")]
        [SerializeField] private LevelEditorRotateCellAction rotateCellAction = null;
        public LevelEditorPositionCellAction positionCellAction = null;
        public LevelEditorDeleteCellAction deleteCellAction = null;
        [Header("Grid Prefabs")]
        [SerializeField] private LevelEditorGridComponent largeGridPrefab = null;
        [SerializeField] private LevelEditorGridComponent smallGridPrefab = null;
        [Header("Grid External Frames")]
        [SerializeField] private GameObject smallFrame = null;
        [SerializeField] private GameObject largeFrame = null;
        [Header("Simulation")]
        [SerializeField] private GridControls gridControls = null;
        [SerializeField] private LevelEditorLevelController levelController = null;
        [Header("Cameras")]
        [SerializeField] private CinemachineVirtualCamera smallGridCamera = null;
        [SerializeField] private CinemachineVirtualCamera largeGridCamera = null;
        [Header("Cameras")]
        [SerializeField] private GameObject editorAudio = null;

        public LevelEditorGridComponent currentGrid { get; set; } = null;
        public ALevelEditorAction currentAction { get; private set; } = null;

        public BaseCell newSelectedBaseCell { get; set; } = null;

        private bool inputEnabled = false;
        private LevelEditorGridComponent simulationGrid = null;
        private CustomLevelSave customLevelSave = new CustomLevelSave(SAVE_PATH);
        private CustomGrid customGrid;

        public const string SAVE_PATH = "/SlaijuReborn_CustomLevels.save";

        private void Start()
        {
#if UNITY_EDITOR
            if (ignoreSceneLoading)
                Initialize().Forget();
#endif
        }

        private void Update()
        {
            if (currentAction == null || !inputEnabled)
                return;

            playerInput.ProcessInput();
        }

        private void OnEnable()
        {
            SceneLoader.OnLoadingCompleted += InitEditor;
        }
        private void OnDisable()
        {
            SceneLoader.OnLoadingCompleted -= InitEditor;
        }

        private void InitEditor() => Initialize().Forget();

        private async UniTask Initialize()
        {
            customLevelSave.Load();

            await SpawnGridRequest();

            currentGrid.SetCellsColor((int)LevelEditorNewLevelSetup.levelColor);
            currentGrid.OnGridReady += ToggleInput;
            uiController.Init(this);
            playerInput.Init(this);
            if (customGrid.gridName != null)
                cellSpawner.NotifyInsertedCellsToUI();
            levelController.OnGameSimulationOver += StopSimulation;
        }


        private async UniTask SpawnGridRequest()
        {
            foreach (CustomGrid savedGrid in customLevelSave.customGrids)
                if (savedGrid.gridName == LevelEditorNewLevelSetup.levelName)
                {
                    customGrid = savedGrid;
                    if (!savedGrid.isSmallGrid)
                    {
                        SetLargeCamera();
                        SetLargeExternalFrame();
                    }
                    await cellSpawner.Init(this, savedGrid.isSmallGrid ? smallGridPrefab : largeGridPrefab, savedGrid);
                    return;
                }

            if (!LevelEditorNewLevelSetup.isSmallGrid)
            {
                SetLargeCamera();
                SetLargeExternalFrame();
            }
            cellSpawner.Init(this, LevelEditorNewLevelSetup.isSmallGrid ? smallGridPrefab : largeGridPrefab);
        }

        private void SetLargeCamera()
        {
            smallGridCamera.Priority--;
            largeGridCamera.Priority += 2;
        }

        private void SetLargeExternalFrame()
        {
            smallFrame.SetActive(false);
            largeFrame.SetActive(true);
        }

        public void RequestNewCellPositioning(BaseCell prefab, LevelEditorCellButton buttonClicked)
        {
            uiController.SetLastSelectedButton(buttonClicked);

            if (newSelectedBaseCell != null)
                Destroy(newSelectedBaseCell.gameObject);

            newSelectedBaseCell = cellSpawner.SpawnCell(prefab);

            if (prefab != null)
            {
                uiController.trashButton.clicked = false;
                uiController.rotateButton.clicked = false;
                currentAction = positionCellAction;
            }
            else
            {
                uiController.rotateButton.clicked = false;
                currentAction = deleteCellAction;
            }

            uiController.rotateButton.ResetButtonAnimation();
            currentAction.previousSelectedCell = null;
        }

        public void GoToRotateAction()
        {
            currentAction = rotateCellAction;
            uiController.trashButton.clicked = false;
            uiController.trashButton.ResetButtonAnimation();
        }

        public void GoToStandardAction() => currentAction = null;

        public void ResetLevel()
        {
            currentGrid.OnGridReady -= ToggleInput;
            Destroy(currentGrid.gameObject);
            if (customGrid.gridName != null)
                currentGrid = cellSpawner.SpawnGrid(customGrid.isSmallGrid? smallGridPrefab : largeGridPrefab);
            else
                currentGrid = cellSpawner.SpawnGrid(LevelEditorNewLevelSetup.isSmallGrid? smallGridPrefab : largeGridPrefab);
            uiController.ResetUI();
            currentAction = null;
            if (newSelectedBaseCell != null)
                Destroy(newSelectedBaseCell.gameObject);
            currentGrid.OnGridReady += ToggleInput;
        }

        public void ToggleInput() => inputEnabled = !inputEnabled;

        public void StartSimulation()
        {
            currentAction = null;
            uiController.ToggleSimulation(true);
            editorAudio.SetActive(false);

            simulationGrid =  cellSpawner.SpawnGrid(currentGrid);
            simulationGrid.InitializeGrid();
            ((StartCell)simulationGrid.GetCellsByID(CellID.Start)[0]).OnCharacterReturn.AddListener(StopSimulation);
            currentGrid.EnableLevelButtonsGrayBox();
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
            currentGrid.TurnOffVisualCells();
            currentGrid.gameObject.SetActive(true);
            gridControls.transform.gameObject.SetActive(false);
            uiController.ToggleSimulation(false);
            currentAction = rotateCellAction;
            editorAudio.SetActive(true);
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

            customLevelSave.customGrids.RemoveAll(x => x.gridName == LevelEditorNewLevelSetup.levelName);
            customLevelSave.customGrids.Add(new CustomGrid { gridName = LevelEditorNewLevelSetup.levelName,
                                                             gridComplete = uiController.simulateButton.interactable,
                                                             isSmallGrid = LevelEditorNewLevelSetup.isSmallGrid, 
                                                             gridColor = LevelEditorNewLevelSetup.levelColor,
                                                             gridCells = currentGridCells });

            customLevelSave.Save();
        }
    }
}
