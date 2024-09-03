using Grid;
using System;
using UnityEngine;

namespace LevelEditor
{
    public class LevelEditorGridComponent : GridComponent
    {
        [SerializeField] private GameObject grayBoxPrefab = null;
        [SerializeField] private float grayBoxesHeight = 10.0f;
        [SerializeField] private BaseCell[] levelButtonArray = Array.Empty<BaseCell>(); 

        private GameObject[,] grayBox;

        protected override void Start()
        {
            base.Start();
            SpawnGrayBoxes();
        }

        private void SpawnGrayBoxes()
        {
            grayBox = new GameObject[width,height];
            GameObject grayBoxesParent = new GameObject();
            grayBoxesParent.name = "[Editor Gray Boxes]";

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    GameObject currentGrayBox = Instantiate<GameObject>(grayBoxPrefab);
                    currentGrayBox.transform.localScale = new Vector3(cellSize, 1.0f, cellSize);
                    currentGrayBox.transform.position = gridArray[i, j].transform.position + Vector3.up * grayBoxesHeight;
                    currentGrayBox.transform.parent = grayBoxesParent.transform;
                    currentGrayBox.name = $"Gray Box [{i},{j}]";
                    grayBox[i,j] = currentGrayBox;
                }
            }
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

        [ContextMenu("ResetVisualEditorElements")]
        public void ResetVisualEditorElements()
        {
            TurnOffGrayBoxes();
            TurnOffCellSelectedView();
        }

        private void TurnOffGrayBoxes()
        {
            for(int i = 0; i < width; i++)
                for (int j = 0;j < height; j++)
                    grayBox[i,j].SetActive(false);
        }

        private void TurnOffCellSelectedView()
        {
            for (int i =0; i < width; i++)
                for(int j = 0; j<height; j++)
                    if (gridArray[i, j].SelectDeselectView != null)
                        gridArray[i,j].SelectDeselectView.ToggleLevelEditorGraphic(false);
        }

        private void TurnOnCellSelectedView()
        {
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    if (!grayBox[i, j].activeSelf && gridArray[i, j].SelectDeselectView != null)
                        gridArray[i, j].SelectDeselectView.ToggleLevelEditorGraphic(true);
        }

        [ContextMenu("Test EnvironmentCellSelected")]
        public void EnvironmentCellSelected()
        {
            ResetVisualEditorElements();

            foreach (BaseCell levelButton in levelButtonArray)
                for( int i = 0; i < width;i++)
                    for (int j = 0; j < height; j++)
                        if (levelButton == gridArray[i,j])
                            grayBox[i,j].SetActive(true);

            TurnOnCellSelectedView();
        }

        [ContextMenu("Test RoadCellSelected")]
        public void RoadCellSelected()
        {
            ResetVisualEditorElements();

            for (int i = 0; i < width; i++)
            {
                grayBox[i, 0].SetActive(true);
                grayBox[i, height-1].SetActive(true);
            }

            for (int i = 1; i < height-1; i++)
            {
                grayBox[0, i].SetActive(true);
                grayBox[width - 1, i].SetActive(true);
            }

            TurnOnCellSelectedView();
        }

        [ContextMenu("Test StartCellSelected")]
        public void StartCellSelected()
        {
            ResetVisualEditorElements();

            for (int i = 0;i < width;i++)
                for(int j = 0;j < height;j++)
                    if (i != 0 || j == 0 || j == height-1)
                        grayBox[i,j].SetActive(true);

            TurnOnCellSelectedView();
        }

        [ContextMenu("Test ExitCellSelected")]
        public void ExitCellSelected()
        {
            ResetVisualEditorElements();

            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    if (i != width - 1 || j == 0 || j == height - 1)
                        grayBox[i, j].SetActive(true);

            TurnOnCellSelectedView();
        }
    }
}
