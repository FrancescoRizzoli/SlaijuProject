using Character;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Grid
{
    [RequireComponent(typeof(GridComponent))]
    public class GridPositionController : MonoBehaviour
    {
        [SerializeField]
        private CharacterStateController objectToCheck;
        [SerializeField]
        private GridComponent gridComponent;
        public Vector2Int currentGridPosition;
        private Vector2Int previousGridPosition;
        [SerializeField, Range(0f, 1f)]
        private float boundaryWidthPercentage = 0.2f;

        private bool isWithinBoundary;
        private bool isOutOfGrid;
        public bool init = false;
        

        public void Init(CharacterStateController character, GridComponent grid)
        {
            objectToCheck = character;
            gridComponent = grid;
            if (objectToCheck != null && gridComponent != null)
            {
                currentGridPosition = gridComponent.WorldToGridPosition(objectToCheck.transform.position);
                previousGridPosition = currentGridPosition;
                objectToCheck.HandleCell(gridComponent.gridArray[currentGridPosition.x, currentGridPosition.y]).Forget();
                Debug.Log(gridComponent.gridArray[currentGridPosition.x, currentGridPosition.y].name);
                init = true;
            }
        }

        void Update()
        {
            if (objectToCheck == null || gridComponent == null || !init)
                return;

            UpdateGridPosition();
        }

        public void UpdateGridPosition()
        {
            Vector2Int newGridPosition = gridComponent.WorldToGridPosition(objectToCheck.transform.position);

            if (newGridPosition != currentGridPosition)
            {

                if (IsOutOfGrid(newGridPosition) && !isOutOfGrid)
                {
                    Debug.Log($"Exited the grid from cell {currentGridPosition}");
                    isOutOfGrid = true;
                }
                else if (!IsOutOfGrid(newGridPosition) && isOutOfGrid)
                {
                    isOutOfGrid = false;
                }
                gridComponent.gridArray[currentGridPosition.x, currentGridPosition.y].OnCellExit();
                previousGridPosition = currentGridPosition;
                currentGridPosition = newGridPosition;
                //Debug.Log( gridComponent.gridArray[currentGridPosition.x, currentGridPosition.y].ID +" entered");
                objectToCheck.HandleCell(gridComponent.gridArray[currentGridPosition.x,currentGridPosition.y]).Forget();
            }
            else
            {
                if (!isWithinBoundary && IsWithinBoundary(objectToCheck.transform.position, currentGridPosition))
                {
                    Vector2Int boundaryCell = GetBoundaryCell(objectToCheck.transform.position, currentGridPosition);
                    if (IsOutOfGrid(boundaryCell))
                    {
                       // Debug.Log($"Approaching edge of the grid from cell {currentGridPosition}");
                    }
                    else
                    {
                       // Debug.Log($"Entered boundary of cell {currentGridPosition} near cell {boundaryCell}");
                        BaseCell cell = gridComponent.gridArray[boundaryCell.x, boundaryCell.y];
                        objectToCheck.Warn(cell);
                        if (cell.ID == CellID.City || cell.ID == CellID.Generator)
                            objectToCheck.HandleCell(cell).Forget();

                    }
                    isWithinBoundary = true;
                }
                else if (!IsWithinBoundary(objectToCheck.transform.position, currentGridPosition))
                {
                    isWithinBoundary = false;
                }

                if (IsOutOfGrid(newGridPosition) && !isOutOfGrid)
                {
                   // Debug.Log($"Exited the grid from cell {currentGridPosition}");
                    isOutOfGrid = true;
                }
                else if (!IsOutOfGrid(newGridPosition) && isOutOfGrid)
                {
                    isOutOfGrid = false;
                }
            }
        }

        private bool IsWithinBoundary(Vector3 position, Vector2Int currentCell)
        {
            Vector3 cellCenter = gridComponent.GridToWorldPosition(currentCell);
            float halfCellSize = gridComponent.cellSize / 2;
            float boundaryWidth = gridComponent.cellSize * boundaryWidthPercentage;

            bool withinXBoundary = Mathf.Abs(position.x - cellCenter.x) > (halfCellSize - boundaryWidth);
            bool withinZBoundary = Mathf.Abs(position.z - cellCenter.z) > (halfCellSize - boundaryWidth);

            return withinXBoundary || withinZBoundary;
        }

        private Vector2Int GetBoundaryCell(Vector3 position, Vector2Int currentCell)
        {
            Vector3 cellCenter = gridComponent.GridToWorldPosition(currentCell);
            float halfCellSize = gridComponent.cellSize / 2;
            float boundaryWidth = gridComponent.cellSize * boundaryWidthPercentage;

            Vector2Int boundaryCell = currentCell;

            if (Mathf.Abs(position.x - cellCenter.x) > (halfCellSize - boundaryWidth))
            {
                if (position.x > cellCenter.x)
                {
                    boundaryCell.x = currentCell.x + 1;
                }
                else
                {
                    boundaryCell.x = currentCell.x - 1;
                }
            }

            if (Mathf.Abs(position.z - cellCenter.z) > (halfCellSize - boundaryWidth))
            {
                if (position.z > cellCenter.z)
                {
                    boundaryCell.y = currentCell.y + 1;
                }
                else
                {
                    boundaryCell.y = currentCell.y - 1;
                }
            }

            return boundaryCell;
        }

        private bool IsOutOfGrid(Vector2Int gridPosition)
        {
            return gridPosition.x < 0 || gridPosition.x >= gridComponent.width ||
                   gridPosition.y < 0 || gridPosition.y >= gridComponent.height;
        }

        public Vector3 GetWorldPosition()
        {
            return gridComponent.GridToWorldPosition(currentGridPosition);
        }

        private Vector2Int ClampGridPosition(Vector2Int gridPosition)
        {
            gridPosition.x = Mathf.Clamp(gridPosition.x, 0, gridComponent.width - 1);
            gridPosition.y = Mathf.Clamp(gridPosition.y, 0, gridComponent.height - 1);
            return gridPosition;
        }
    }
}
