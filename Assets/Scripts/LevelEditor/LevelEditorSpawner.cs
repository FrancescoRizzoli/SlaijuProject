using Grid;
using UnityEngine;

namespace LevelEditor
{
    public class LevelEditorSpawner : MonoBehaviour
    {
        private LevelEditorController controller = null;

        public void Init(LevelEditorController editorController, LevelEditorGridComponent gridPrefab)
        {
            controller = editorController;
            SpawnGrid(gridPrefab);
        }

        private void SpawnGrid(LevelEditorGridComponent gridPrefab)
        {
            //TODO
        }

        public void SpawnCell(BaseCell cellPrefab)
        {
            if (controller.newSelectedBaseCell != null)
                Destroy(controller.newSelectedBaseCell.gameObject);

            BaseCell cell = Instantiate(cellPrefab);
            cell.gameObject.SetActive(false);
            controller.newSelectedBaseCell = cell;
        }
    }
}
