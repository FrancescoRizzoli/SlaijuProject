using Cysharp.Threading.Tasks;
using Grid;
using Grid.Cell;
using System;
using UnityEngine;

namespace LevelEditor
{
    public class LevelEditorGridComponent : GridComponent
    {
        [SerializeField] private LevelEditorVisualCell visualCellPrefab = null;
        [SerializeField] private float visualCellsHeight = 10.0f;
        public BaseCell[] levelButtonArray = Array.Empty<BaseCell>(); 

        public LevelEditorVisualCell[,] visualCell { get; set; }
        public int emptyCellsCounter { get; private set; } = 0;
        public int maxEmptyCellNumber { get; private set; } = 0;

        public delegate void EditorGridEvent();
        public event EditorGridEvent OnCellPositioned = null;
        public event EditorGridEvent OnGridReady = null;

        public delegate void EditorGridSimulationConditionEvent(bool conditionValue);
        public event EditorGridSimulationConditionEvent OnSimulationCondition = null;

        private GameObject visualCellsParent = null;
        private GeneratorCell generatorCell = null;
        private ShieldedCityCell shieldedCityCell = null;
        private StartCell startCell = null;
        private ExitCell exitCell = null;

        protected override void Start()
        {
            maxEmptyCellNumber = (width * height) - levelButtonArray.Length;
            emptyCellsCounter = maxEmptyCellNumber;
            base.Start();
            SpawnVisualCells();
            OnGridReady?.Invoke();
        }

        private void OnDestroy()
        {
            Destroy(visualCellsParent);
        }

        private void SpawnVisualCells()
        {
            visualCell = new LevelEditorVisualCell[width,height];
            visualCellsParent = new GameObject();
            visualCellsParent.name = "[Editor Visual Cells]";

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    LevelEditorVisualCell currentVisualCell = Instantiate<LevelEditorVisualCell>(visualCellPrefab);
                    currentVisualCell.transform.localScale = new Vector3(cellSize, 1.0f, cellSize);
                    currentVisualCell.transform.position = gridArray[i, j].transform.position + Vector3.up * visualCellsHeight;
                    currentVisualCell.transform.parent = visualCellsParent.transform;
                    currentVisualCell.name = $"Visual Cell [{i},{j}]";
                    visualCell[i,j] = currentVisualCell;
                }
            }
        }

        public async UniTask PositionCell(BaseCell newCell, BaseCell toBeReplacedCell)
        {
            if (newCell.ID != CellID.LevelEditorEmpty && toBeReplacedCell.ID == CellID.LevelEditorEmpty)
                emptyCellsCounter--;
            else if (newCell.ID == CellID.LevelEditorEmpty)
                emptyCellsCounter++;

            newCell.transform.parent = toBeReplacedCell.transform.parent;
            Destroy(toBeReplacedCell.gameObject);

            if (newCell.ID == CellID.Generator)
                generatorCell = (GeneratorCell)newCell;
            else if (newCell.GetType() == typeof(ShieldedCityCell))
                shieldedCityCell = (ShieldedCityCell)newCell;

            if (generatorCell != null && shieldedCityCell != null)
                generatorCell.targetCity = shieldedCityCell;

            if (newCell.ID == CellID.Start)
                startCell = (StartCell)newCell;

            if (newCell.ID == CellID.Exit)
                exitCell = (ExitCell)newCell;

            await UniTask.NextFrame();

            InitializeGrid();

            if (emptyCellsCounter == maxEmptyCellNumber)
                TurnOffVisualCells();
            
            OnSimulationCondition?.Invoke(emptyCellsCounter == 0 && startCell != null && exitCell != null && ((generatorCell == null && shieldedCityCell == null) || (generatorCell != null && shieldedCityCell != null)));
            OnCellPositioned?.Invoke();
        }

        public Vector2Int GetCellIndexes(BaseCell targetCell)
        {
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    if (gridArray[i,j] == targetCell)
                        return new Vector2Int(i, j);

            return new Vector2Int(-1, -1);
        }

        [ContextMenu("Initialize Grid")]
        private void InitGrid() => InitializeGrid();

        [ContextMenu("TurnOffVisualCells")]
        public void TurnOffVisualCells()
        {
            for(int i = 0; i < width; i++)
                for (int j = 0;j < height; j++)
                {
                    visualCell[i,j].ToggleGrayBox(false);
                    visualCell[i,j].ToggleSelectCubes(false);
                }
        }

        [ContextMenu("Test EnvironmentCellSelected")]
        public void EnvironmentCellSelected()
        {
            TurnOffVisualCells();

            for( int i = 0; i < width;i++)
                for (int j = 0; j < height; j++)
                    if (Array.IndexOf<BaseCell>(levelButtonArray, gridArray[i, j]) != -1)
                        visualCell[i,j].ToggleGrayBox(true);
                    else
                        visualCell[i,j].ToggleSelectCubes(true);
        }

        [ContextMenu("Test RoadCellSelected")]
        public void RoadCellSelected()
        {
            TurnOffVisualCells();

            for (int i = 0; i < width; i++)
            {
                visualCell[i, 0].ToggleGrayBox(true);
                visualCell[i, height-1].ToggleGrayBox(true);
            }

            for (int i = 1; i < height-1; i++)
            {
                visualCell[0, i].ToggleGrayBox(true);
                visualCell[width - 1, i].ToggleGrayBox(true);
            }

            for (int i = 1; i < width - 1; i++)
                for (int j = 1; j < height-1; j++)
                    visualCell[i, j].ToggleSelectCubes(true);
        }

        [ContextMenu("Test StartCellSelected")]
        public void StartCellSelected()
        {
            TurnOffVisualCells();

            for (int i = 0;i < width;i++)
                for(int j = 0;j < height;j++)
                    if (i != 0 || j == 0 || j == height-1)
                        visualCell[i,j].ToggleGrayBox(true);
                    else
                        visualCell[i,j].ToggleSelectCubes(true);
        }

        [ContextMenu("Test ExitCellSelected")]
        public void ExitCellSelected()
        {
            TurnOffVisualCells();

            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    if (i != width - 1 || j == 0 || j == height - 1)
                        visualCell[i, j].ToggleGrayBox(true);
                    else
                        visualCell[i, j].ToggleSelectCubes(true);
        }

        public void CellRemoverSelected()
        {
            TurnOffVisualCells();

            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    if (Array.IndexOf<BaseCell>(levelButtonArray, gridArray[i, j]) != -1 || gridArray[i,j].ID == CellID.LevelEditorEmpty)
                        visualCell[i, j].ToggleGrayBox(true);
                    else
                        visualCell[i, j].ToggleSelectCubes(true);
        }
    }
}
