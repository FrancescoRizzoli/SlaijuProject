using Cysharp.Threading.Tasks;
using Grid;
using Grid.Cell;
using UnityEngine;

namespace LevelEditor
{
    public class CustomGridSpawner : MonoBehaviour
    {
        [SerializeField] protected CellPrefabsList prefabList = null;

        private ShieldedCityCell shieldedCity = null;
        private GeneratorCell generatorCell = null;

        protected BaseCell newCell = null;

        public virtual async UniTask PopulateGrid(GridComponent grid, CustomGrid savedGrid)
        {
            shieldedCity = null;
            generatorCell = null;

            grid.InitializeGrid();      // make sure the grid is initialized
            foreach (CustomGridCell cgc in savedGrid.gridCells)
                SpawnCellIntoCurrentGrid(grid, cgc);

            await UniTask.NextFrame();      // wait one frame so that the cells removed in the grid are destroyed

            grid.InitializeGrid();
            grid.SetCellsColor((int)savedGrid.gridColor);
        }

        protected virtual void SpawnCellIntoCurrentGrid(GridComponent grid, CustomGridCell cgc)
        {
            BaseCell targetPrefab = null;
            foreach (BaseCell bc in prefabList.cellPrefabs)
                if (bc.name == cgc.cellName)
                {
                    targetPrefab = bc;
                    break;
                }

            newCell = Instantiate<BaseCell>(targetPrefab);
            newCell.name = cgc.cellName;

            if (newCell.ID == CellID.Generator)
                generatorCell = (GeneratorCell)newCell;

            if (newCell.GetType() == typeof(ShieldedCityCell))
                shieldedCity = (ShieldedCityCell)newCell;

            if (shieldedCity != null && generatorCell != null)
                generatorCell.targetCity = shieldedCity;

            grid.AddCellInGrid(newCell, cgc.positionInGrid, cgc.forwardDirection);
        }
    }
}
