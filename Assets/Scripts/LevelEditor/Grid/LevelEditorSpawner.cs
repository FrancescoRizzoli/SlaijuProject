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
            return Instantiate<LevelEditorGridComponent>(gridPrefab);
        }

        public BaseCell SpawnCell(BaseCell cellPrefab)
        {
            BaseCell cell = Instantiate(cellPrefab != null ? cellPrefab : editorEmptyCellPrefab);
            cell.gameObject.SetActive(false);
            return cell;
        }
    }
}
