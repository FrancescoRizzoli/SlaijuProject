using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor
{
    public class LevelEditorUIController : MonoBehaviour
    {
        [Header("Cell Type Filter")]
        [SerializeField] private LevelEditorCellTypeFilterData[] environmentCellFilterDataArray = null;
        [SerializeField] private LevelEditorCellTypeFilterData[] frameCellFilterData = null;
        [SerializeField] private LevelEditorCellTypeFilterData objectiveCellFilterData = null;
        [SerializeField] private LevelEditorCellTypeFilterData roadCellFilterData = null;
        [SerializeField] private TextMeshProUGUI cellTypeFilterName = null;
        [SerializeField] private Button cellTypeFilterNextButton = null;
        [SerializeField] private Button cellTypeFilterPreviousButton = null;
        [SerializeField] private GameObject filterParentGOPrefab = null;
        [Header("Scroll view")]
        [SerializeField] private Transform scrollViewContentTransform = null;
        [SerializeField] private TextMeshProUGUI setNamePrefab = null;
        [SerializeField] private GameObject setGridGroupPrefab = null;
        [SerializeField] private LevelEditorCellButton cellEditorButtonPrefab = null;
        [Header("Recently used cells")]
        [SerializeField] private LevelEditorRecentlyUsedCells recentlyUsedCells = null;
        [Header("Action Buttons")]
        public LevelEditorTrashButton trashButton = null;
        public LevelEditorRotateButton rotateButton = null;
        [Header("Simulation")]
        public Button simulateButton = null;
        [SerializeField] private Canvas simulationCanvas = null;

        private LevelEditorController editorController = null;
        private int currentCellTypeFilterIndex = -1;
        private string currentSetName = "";
        private Transform currentGridTransform = null;
        private GameObject[] filterGOArray;
        private List<LevelEditorCellButton> editorCellButtonList = new List<LevelEditorCellButton>();
        private Dictionary<Type, LevelEditorCellButton> limitedCellButton = new Dictionary<Type, LevelEditorCellButton>();
        LevelEditorCellTypeFilterData[] cellTypeFilterDataArray = new LevelEditorCellTypeFilterData[4];

        public LevelEditorCellButton lastSelectedButton { get; private set; } = null;

        public void Init(LevelEditorController controller)
        {
            editorController = controller;

            cellTypeFilterNextButton.onClick.AddListener(NextCellTypeFilter);
            cellTypeFilterPreviousButton.onClick.AddListener(PreviousCellTypeFilter);

            trashButton.Init(editorController);
            rotateButton.Init(editorController);
            editorController.deleteCellAction.OnCellDeleted += HandleCellDeleted;
            editorController.positionCellAction.OnCellReplaced += HandleCellDeleted;
            editorController.currentGrid.OnSimulationCondition += HandleGridFullEvent;

            recentlyUsedCells.Init(editorController);

            cellTypeFilterDataArray[0] = environmentCellFilterDataArray[(int)LevelEditorNewLevelSetup.levelColor];
            cellTypeFilterDataArray[1] = objectiveCellFilterData;
            cellTypeFilterDataArray[2] = roadCellFilterData;
            cellTypeFilterDataArray[3] = frameCellFilterData[(int)LevelEditorNewLevelSetup.levelColor];

            PopulateViewport();
            SelectFilter(true);
        }

        private void NextCellTypeFilter() => SelectFilter(true);

        private void PreviousCellTypeFilter() => SelectFilter(false);

        private void SelectFilter(bool next)
        {
            if (next)
                currentCellTypeFilterIndex = (currentCellTypeFilterIndex + 1) % cellTypeFilterDataArray.Length;
            else if (--currentCellTypeFilterIndex < 0)
                    currentCellTypeFilterIndex += cellTypeFilterDataArray.Length;
            

            cellTypeFilterName.text = cellTypeFilterDataArray[currentCellTypeFilterIndex].filterType.ToString();
            SetCellTypeFilter(currentCellTypeFilterIndex);
        }

        private void SetCellTypeFilter(int index)
        {
            for(int i = 0; i < filterGOArray.Length; i++)
                if (i == index)
                    filterGOArray[i].SetActive(true);
                else
                    filterGOArray[i].SetActive(false);
        }

        private void PopulateViewport()
        {
            filterGOArray = new GameObject[cellTypeFilterDataArray.Length];

            for (int i = 0; i < cellTypeFilterDataArray.Length; i++)
            {
                GameObject filterGO = Instantiate(filterParentGOPrefab, scrollViewContentTransform);
                filterGO.name = $"Filter [{i}]";
                filterGOArray[i] = filterGO;

                foreach (EditorCell ec in cellTypeFilterDataArray[i].editorCell)
                {
                    if (currentSetName != ec.setHeaderName)
                    {
                        Instantiate(setNamePrefab, filterGO.transform).text = ec.setHeaderName;
                        currentSetName = ec.setHeaderName;
                        currentGridTransform = Instantiate(setGridGroupPrefab, filterGO.transform).transform;
                    }
                    LevelEditorCellButton button = Instantiate<LevelEditorCellButton>(cellEditorButtonPrefab, currentGridTransform);

                    editorCellButtonList.Add(button);

                    button.Init(cellTypeFilterDataArray[i].filterType, ec, editorController, recentlyUsedCells);

                    if(ec.limited)
                        limitedCellButton[button.cellPrefab.GetType()] = button;
                }

                currentSetName = "";
            }
        }

        public void SetLastSelectedButton(LevelEditorCellButton button)
        {
            if (lastSelectedButton != null && lastSelectedButton.limitQuantity)
                lastSelectedButton.UnsubscribeCellPositioning();

            lastSelectedButton = button;
        }

        private void HandleCellDeleted(Type cellType)
        {
            if(limitedCellButton.ContainsKey(cellType))
                limitedCellButton[cellType].IncrementQuantityAvailable();
        }

        public void HandleCellInserted(Type cellType)
        {
            if (limitedCellButton.ContainsKey(cellType))
                limitedCellButton[cellType].HandleCellPositioned();
        }

        private void HandleGridFullEvent(bool conditionValue) => simulateButton.interactable = conditionValue;

        public void ResetUI()
        {
            foreach (LevelEditorCellButton editorButton in editorCellButtonList)
                editorButton.SetOnClickEvents();

            foreach (KeyValuePair<Type, LevelEditorCellButton> kvp in limitedCellButton)
                kvp.Value.ResetQuantity();

            foreach(LevelEditorCellButton recentCellButton in recentlyUsedCells.cellButton)
                recentCellButton.SetOnClickEvents();

            editorController.currentGrid.OnSimulationCondition += HandleGridFullEvent;
            simulateButton.interactable = false;
            rotateButton.ToggleButton();
            rotateButton.SubscribeToCurrentGrid();
            trashButton.ToggleButton();
            trashButton.SubscribeToCurrentGrid();
        }

        public void ToggleSimulation(bool value)
        {
            GetComponent<Canvas>().enabled = !value;
            simulationCanvas.enabled = value;
        }
    }
}
