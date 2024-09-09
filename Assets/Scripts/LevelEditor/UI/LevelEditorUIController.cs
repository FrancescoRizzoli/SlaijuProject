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
        [SerializeField] private LevelEditorCellTypeFilterData[] cellTypeFilterDataArray = null;
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
        [Header("Trash Button")]
        [SerializeField] private LevelEditorTrashButton trashButton = null;

        private LevelEditorController editorController = null;
        private LevelEditorSpawner cellSpawner = null;
        private int currentCellTypeFilterIndex = -1;
        private string currentSetName = "";
        private Transform currentGridTransform = null;
        private GameObject[] filterGOArray;
        private List<LevelEditorCellButton> editorCellButtonList = new List<LevelEditorCellButton>();
        private List<LevelEditorCellButton> limitedCellButton = new List<LevelEditorCellButton>();

        public void Init(LevelEditorController controller, LevelEditorSpawner spawner)
        {
            editorController = controller;
            cellSpawner = spawner;

            cellTypeFilterNextButton.onClick.AddListener(NextCellTypeFilter);
            cellTypeFilterPreviousButton.onClick.AddListener(PreviousCellTypeFilter);

            trashButton.Init(editorController);
            editorController.deleteCellAction.OnCellDeleted += HandleCellDeleted;

            recentlyUsedCells.Init(editorController);

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

                    if(ec.limited)
                        limitedCellButton.Add(button);

                    editorCellButtonList.Add(button);

                    button.Init(cellTypeFilterDataArray[i].filterType, ec, editorController, recentlyUsedCells);
                }

                currentSetName = "";

            }
        }

        private void HandleCellDeleted(Type cellType)
        {
            foreach (LevelEditorCellButton button in limitedCellButton)
                if (cellType == button.cellPrefab.GetType())
                    button.IncrementQuantityAvailable();
        }

        public void ResetUI()
        {
            foreach (LevelEditorCellButton editorButton in editorCellButtonList)
                editorButton.SetOnClickEvents();

            foreach (LevelEditorCellButton limitedButton in limitedCellButton)
                limitedButton.ResetQuantity();

            foreach(LevelEditorCellButton recentCellButton in recentlyUsedCells.cellButton)
                recentCellButton.SetOnClickEvents();
        }
    }
}
