using Grid;
using UnityEngine;

namespace LevelEditor
{
    public class LevelEditorController : MonoBehaviour
    {
        [SerializeField] private LevelEditorUIController uiController = null;
        [SerializeField] private LevelEditorSpawner cellSpawner = null;
        [SerializeField] private LevelEditorGridPlayerInput playerInput = null;
        [Header("Grid Prefabs")]
        [SerializeField] private LevelEditorGridComponent largeGridPrefab = null;
        [SerializeField] private LevelEditorGridComponent smallGridPrefab = null;

        public LevelEditorGridComponent currentGrid { get; private set; } = null;

        public BaseCell newSelectedBaseCell { get; set; } = null;

        private void Start()
        {
            currentGrid = largeGridPrefab;      // TODO: logic for the correct grid

            uiController.Init(this, cellSpawner);
            cellSpawner.Init(this, currentGrid);
            playerInput.Init(this);
        }


        private Ray ray;
        private RaycastHit hit;

        private void Update()
        {
            if (newSelectedBaseCell == null)
                return;

            playerInput.ProcessInput();
        }
    }
}
