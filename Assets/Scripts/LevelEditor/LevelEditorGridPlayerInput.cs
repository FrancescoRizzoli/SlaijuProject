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
            {
                controller.currentGrid.PositionCell(controller.newSelectedBaseCell, currentSelectedGridBaseCell).Forget();

                if (controller.newSelectedBaseCell.ID == CellID.LevelEditorEmpty && controller.currentGrid.nonEmptyCellsCounter > 0)
                    controller.newSelectedBaseCell = controller.cellSpawner.SpawnCell(null);
                else
                {
                    currentSelectedGridBaseCell = null;
                    controller.newSelectedBaseCell = null;
                }
            }
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
