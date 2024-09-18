using Cysharp.Threading.Tasks;
using Grid;
using UnityEngine;

namespace LevelEditor
{
    public class CustomLevelPlayerHandler : MonoBehaviour
    {
        [Header("Small grid")]
        [SerializeField] private GameObject smallGridGO = null;
        [SerializeField] private GridComponent smallGridComponent = null;
        [Header("Large grid")]
        [SerializeField] private GameObject largeGridGO = null;
        [SerializeField] private GridComponent largeGridComponent = null;
        [Header("Spawner")]
        [SerializeField] private CustomGridSpawner spawner = null;

        private CustomLevelSave customLevelSave = new CustomLevelSave(LevelEditorController.SAVE_PATH);
        private CustomGrid savedGrid;

        private void Start()
        {
            customLevelSave.Load();
            savedGrid = customLevelSave.customGrids.Find(x => x.gridName == LevelEditorNewLevelSetup.levelName);
            if (LevelEditorNewLevelSetup.isSmallGrid)
            {
                smallGridGO.SetActive(true);
                spawner.PopulateGrid(smallGridComponent, savedGrid).Forget();
            }
            else
            {
                largeGridGO.SetActive(true);
                spawner.PopulateGrid(largeGridComponent, savedGrid).Forget();
            }
        }
    }
}
