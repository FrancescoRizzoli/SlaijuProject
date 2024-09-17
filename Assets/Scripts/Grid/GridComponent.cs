using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    public class GridComponent : MonoBehaviour
    {
        public int width;
        public int height;
        public float cellSize;
        public BaseCell[,] gridArray;

        protected virtual void Start()
        {
            InitializeGrid();
        }

        public void InitializeGrid()
        {
            gridArray = new BaseCell[width, height];

            // List to hold all child objects
            List<Transform> children = new List<Transform>();
            foreach (Transform child in transform)
            {
                children.Add(child);
            }

            // Sort the children based on their world position
            children.Sort((a, b) =>
            {
                Vector2Int aPos = WorldToGridPosition(a.position);
                Vector2Int bPos = WorldToGridPosition(b.position);
                if (aPos.y != bPos.y) return bPos.y.CompareTo(aPos.y); // Compare y first for top to bottom
                return aPos.x.CompareTo(bPos.x); // Compare x next for left to right
            });

            // Reorder children in the hierarchy
            for (int i = 0; i < children.Count; i++)
            {
                children[i].SetSiblingIndex(i);
            }

            foreach (Transform child in transform)
            {
                Vector2Int gridPosition = WorldToGridPosition(child.position);
                gridPosition = ClampGridPosition(gridPosition);
                Vector3 correctWorldPosition = GridToWorldPosition(gridPosition);

                // Move the cell to the correct position
                child.position = correctWorldPosition;

                if (child.gameObject.TryGetComponent<BaseCell>(out BaseCell baseCell))
                {
                    gridArray[gridPosition.x, gridPosition.y] = baseCell;
                }
                else
                {
                    Debug.LogWarning($"GameObject {child.gameObject.name} at position {gridPosition} is missing a BaseCell component.");
                }
            }

        }

        public Vector2Int WorldToGridPosition(Vector3 worldPosition)
        {
            int x = Mathf.FloorToInt(worldPosition.x / cellSize);
            int y = Mathf.FloorToInt(worldPosition.z / cellSize);
            return new Vector2Int(x, y);
        }
        public Vector2Int GetNextGridPosition(Vector3 worldPosition, Vector3 forward)
        {

            Vector2Int currentGridPosition = WorldToGridPosition(worldPosition);


            Vector2Int gridDirection = new Vector2Int(
                Mathf.RoundToInt(forward.x),
                Mathf.RoundToInt(forward.z)
            );


            Vector2Int nextGridPosition = currentGridPosition + gridDirection;

            if (nextGridPosition.x >= width)
            {
                nextGridPosition.x = width;
            }
            if (nextGridPosition.y >= height)
            { nextGridPosition.y = height; }


            return nextGridPosition;
        }

        public Vector3 GridToWorldPosition(Vector2Int gridPosition)
        {
            float x = gridPosition.x * cellSize + cellSize / 2;
            float z = gridPosition.y * cellSize + cellSize / 2;
            return new Vector3(x, 0, z); // Assuming y=0 for ground level, adjust if necessary
        }

        Vector2Int ClampGridPosition(Vector2Int gridPosition)
        {
            gridPosition.x = Mathf.Clamp(gridPosition.x, 0, width - 1);
            gridPosition.y = Mathf.Clamp(gridPosition.y, 0, height - 1);
            return gridPosition;
        }
        public bool IsValidPosition(Vector2Int gridPosition)
        {
            return gridPosition.x >= 0 && gridPosition.x < width && gridPosition.y >= 0 && gridPosition.y < height;
        }

        public BaseCell GetStartCell()
        {
            foreach (BaseCell cell in gridArray)
            {
                if (cell != null && cell.ID == CellID.Start)
                {
                    return cell;
                }
            }
            return null;
        }

        public BaseCell GetExitCell()
        {
            foreach (BaseCell cell in gridArray)
            {
                if (cell != null && cell.ID == CellID.Exit)
                {
                    return cell;
                }
            }
            return null;
        }
        public List<BaseCell> GetCellsByID(CellID id)
        {
            List<BaseCell> cells = new List<BaseCell>();

            foreach (BaseCell cell in gridArray)
            {
                if (cell != null && cell.ID == id)
                {
                    cells.Add(cell);
                }
            }
            if (cells.Count == 0)
                return null;
            else
                return cells;
        }

        /// <summary>
        /// Add a cell in the grid at a specific position and rotation.
        /// DOES NOT RE-INITIALIZE THE GRID
        /// </summary>
        /// <param name="newCell">the cell to add</param>
        /// <param name="targetPosition">the target position</param>
        /// <param name="cellForwardDirection">the cell's forward direction</param>
        public void AddCellInGrid(BaseCell newCell, Vector2Int targetPosition, Vector3 cellForwardDirection)
        {
            BaseCell toBeRemovedCell = gridArray[targetPosition.x, targetPosition.y];

            if (toBeRemovedCell != null)
            {
                newCell.transform.position = toBeRemovedCell.transform.position;
                Destroy(toBeRemovedCell.gameObject);
            }
            else
                newCell.transform.position = GridToWorldPosition(targetPosition);

            newCell.transform.parent = transform;
            newCell.transform.forward = cellForwardDirection;
            newCell.RecalculateSafeSides();
        }


        // Optional: To visualize the grid in the editor
        private void OnDrawGizmos()
        {
            if (gridArray != null)
            {
                Gizmos.color = Color.red;
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        Vector3 cellCenter = GridToWorldPosition(new Vector2Int(x, y));
                        Gizmos.DrawWireCube(cellCenter, new Vector3(cellSize, 0, cellSize));
                    }
                }
            }
        }
    }
}
