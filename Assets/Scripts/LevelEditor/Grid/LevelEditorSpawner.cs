using Cysharp.Threading.Tasks;
using Grid;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LevelEditor
{
    public class LevelEditorSpawner : CustomGridSpawner
    {
        [SerializeField] private BaseCell editorEmptyCellPrefab = null;

        private LevelEditorController controller = null;
        private List<Type> savedCellsType = new List<Type>();

        public void Init(LevelEditorController editorController, LevelEditorGridComponent gridPrefab)
        {
            controller = editorController;
            controller.currentGrid = SpawnGrid(gridPrefab);
        }

        public async UniTask Init(LevelEditorController editorController, LevelEditorGridComponent gridPrefab, CustomGrid savedGrid)
        {
            controller = editorController;
            controller.currentGrid = await SpawnGrid(gridPrefab, savedGrid);
        }

        public LevelEditorGridComponent SpawnGrid(LevelEditorGridComponent gridPrefab)
        {
            LevelEditorGridComponent grid = Instantiate<LevelEditorGridComponent>(gridPrefab);
            grid.gameObject.SetActive(true);
            return grid;
        }

        public async UniTask<LevelEditorGridComponent> SpawnGrid(LevelEditorGridComponent gridPrefab, CustomGrid savedGrid)
        {
            LevelEditorGridComponent grid = Instantiate<LevelEditorGridComponent>(gridPrefab);
            savedCellsType.Clear();

            await PopulateGrid(grid, savedGrid);

            grid.UpdateGridStatus();
            grid.gameObject.SetActive(true);
            return grid;
        }

        public void NotifyInsertedCellsToUI()
        {
            foreach (Type t in savedCellsType)
                controller.uiController.HandleCellInserted(t);
        }

        protected override void SpawnCellIntoCurrentGrid(GridComponent grid, CustomGridCell cgc)
        {
            base.SpawnCellIntoCurrentGrid(grid, cgc);
            savedCellsType.Add(newCell.GetType());
        }

        public BaseCell SpawnCell(BaseCell cellPrefab)
        {
            BaseCell cell = Instantiate(cellPrefab != null ? cellPrefab : editorEmptyCellPrefab);
            cell.name = cellPrefab != null ? cellPrefab.name : editorEmptyCellPrefab.name;
            cell.gameObject.SetActive(false);
            return cell;
        }
    }
}
