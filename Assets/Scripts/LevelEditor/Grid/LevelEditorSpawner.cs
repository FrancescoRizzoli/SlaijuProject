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
            controller.currentGrid = SpawnGrid(gridPrefab);
        }

        public LevelEditorGridComponent SpawnGrid(LevelEditorGridComponent gridPrefab)
        {
            LevelEditorGridComponent grid = Instantiate<LevelEditorGridComponent>(gridPrefab);
            grid.gameObject.SetActive(true);
            return grid;
        }

        public BaseCell SpawnCell(BaseCell cellPrefab)
        {
            BaseCell cell = Instantiate(cellPrefab != null ? cellPrefab : editorEmptyCellPrefab);
            cell.gameObject.SetActive(false);
            return cell;
        }
    }
}
