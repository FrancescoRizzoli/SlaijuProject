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

            if (Input.GetButtonDown("Fire1") && currentSelectedGridBaseCell != null)
                controller.currentAction.HandleClick(currentSelectedGridBaseCell);
        }

        private void VisualizeSelectedCell()
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit) && hit.transform.TryGetComponent<BaseCell>(out targetCell))
            {
                if (targetCell != currentSelectedGridBaseCell && targetCell != controller.newSelectedBaseCell)
                {
                    controller.currentAction.HandleGridVisualization(targetCell);
                    currentSelectedGridBaseCell = targetCell;
                }
            }
            else
            {
                controller.currentAction.HandleGridVisualization(null);
                currentSelectedGridBaseCell= null;
            }
        }
    }
}
