using Cysharp.Threading.Tasks;
using Grid;
using UnityEngine;

namespace LevelEditor
{
    public class LevelEditorGridPlayerInput : MonoBehaviour
    {
        private LevelEditorController controller = null;
        private Ray ray;
        private RaycastHit hit;
        private BaseCell currentSelectedGridBaseCell = null;
        private BaseCell targetCell = null;

        public void Init(LevelEditorController levelEditorController)
        {
            controller = levelEditorController;
        }

        public void ProcessInput()
        {
            VisualizeSelectedCell();

            if (Input.GetButtonDown("Fire1") && controller.newSelectedBaseCell != null && currentSelectedGridBaseCell != null)
                PositionCell().Forget();
        }

        private async UniTask PositionCell()
        {
            //Vector2Int cellGridIndexes = controller.currentGrid.GetCellIndexes(currentSelectedGridBaseCell);
            controller.newSelectedBaseCell.transform.parent = currentSelectedGridBaseCell.transform.parent;
            //controller.currentGrid.gridArray[cellGridIndexes.x, cellGridIndexes.y] = controller.newSelectedBaseCell;
            Destroy(currentSelectedGridBaseCell.gameObject);

            await UniTask.NextFrame();

            controller.newSelectedBaseCell = null;
            currentSelectedGridBaseCell = null;
            controller.currentGrid.InitializeGrid();
            controller.currentGrid.ResetVisualEditorElements();
        }

        private void VisualizeSelectedCell()
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit) && hit.transform.TryGetComponent<BaseCell>(out targetCell))
            {
                if (targetCell != currentSelectedGridBaseCell && targetCell != controller.newSelectedBaseCell)
                {
                    DisableNewSelectedCell();
                    currentSelectedGridBaseCell = targetCell;
                    currentSelectedGridBaseCell.gameObject.SetActive(false);
                    controller.newSelectedBaseCell.transform.position = currentSelectedGridBaseCell.transform.position;
                    controller.newSelectedBaseCell.gameObject.SetActive(true);
                }
            }
            else
                DisableNewSelectedCell();
        }

        private void DisableNewSelectedCell()
        {
            if (currentSelectedGridBaseCell != null)
            {
                currentSelectedGridBaseCell.gameObject.SetActive(true);
                currentSelectedGridBaseCell = null;
            }

            controller.newSelectedBaseCell.gameObject.SetActive(false);
        }
    }
}
