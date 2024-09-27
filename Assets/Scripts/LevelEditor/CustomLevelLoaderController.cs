using Cinemachine;
using Cysharp.Threading.Tasks;
using Grid;
using UnityEngine;

namespace LevelEditor
{
    public class CustomLevelLoaderController : MonoBehaviour
    {
        [SerializeField] private CustomLevelLoaderUIController uiController = null;
        [SerializeField] private CustomGridSpawner spawner = null;
        [SerializeField] private GridComponent smallGridPrefab = null;
        [SerializeField] private GridComponent largeGridPrefab = null;
        [Header("Cameras")]
        [SerializeField] private CinemachineVirtualCamera smallGridCamera = null;
        [SerializeField] private CinemachineVirtualCamera largeGridCamera = null;
        [Header("Grid External Frames")]
        [SerializeField] private GameObject smallFrame = null;
        [SerializeField] private GameObject largeFrame = null;

        public CustomLevelSave customLevelSave { get; private set; } = new CustomLevelSave(LevelEditorController.SAVE_PATH);

        private GridComponent currentGrid = null;

        private void Start()
        {
            customLevelSave.Load();
            uiController.Init(this);
        }

        public void ShowCustomGrid(CustomGrid customGrid)
        {
            if (currentGrid != null)
                Destroy(currentGrid.gameObject);

            currentGrid = Instantiate<GridComponent>(customGrid.isSmallGrid ? smallGridPrefab : largeGridPrefab);

            smallGridCamera.Priority = customGrid.isSmallGrid ? 10 : 9;
            largeGridCamera.Priority = customGrid.isSmallGrid ? 9 : 10;

            smallFrame.SetActive(customGrid.isSmallGrid);
            largeFrame.SetActive(!customGrid.isSmallGrid);

            LevelEditorNewLevelSetup.levelName = customGrid.gridName;
            LevelEditorNewLevelSetup.isSmallGrid = customGrid.isSmallGrid;
            LevelEditorNewLevelSetup.levelColor = customGrid.gridColor;

            spawner.PopulateGrid(currentGrid, customGrid).Forget();
        }
    }
}
