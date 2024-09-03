using Grid;
using UnityEngine;

namespace LevelEditor
{
    public class LevelEditorSpawner : MonoBehaviour
    {
        [SerializeField] private BaseCell editorEmptyCellPrefab = null;

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

        public BaseCell SpawnCell(BaseCell cellPrefab)
        {
            BaseCell cell = Instantiate(cellPrefab != null ? cellPrefab : editorEmptyCellPrefab);
            cell.gameObject.SetActive(false);
            return cell;
        }
    }
}
