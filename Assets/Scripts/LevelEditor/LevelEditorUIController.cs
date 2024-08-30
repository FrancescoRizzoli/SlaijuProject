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
        [Header("Scroll view")]
        [SerializeField] private Transform scrollViewContentTransform = null;
        [SerializeField] private TextMeshProUGUI setNamePrefab = null;
        [SerializeField] private GameObject setGridGroupPrefab = null;
        [SerializeField] private Button cellEditorButtonPrefab = null;

        private int currentCellTypeFilterIndex = -1;
        private string currentSetName = "";
        private Transform currentGridTransform = null;

        private void Start()
        {
            cellTypeFilterNextButton.onClick.AddListener(NextCellTypeFilter);
            cellTypeFilterPreviousButton.onClick.AddListener(PreviousCellTypeFilter);

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
            

            cellTypeFilterName.text = cellTypeFilterDataArray[currentCellTypeFilterIndex].filterName;
            SetCellTypeFilter(currentCellTypeFilterIndex);
        }

        private void SetCellTypeFilter(int index)
        {
            ClearViewport();
            PopulateViewport(index);
        }

        private void ClearViewport()
        {
            foreach(Transform child in scrollViewContentTransform)
                Destroy(child.gameObject);
        }

        private void PopulateViewport(int index)
        {
            foreach (EditorCell ec in cellTypeFilterDataArray[currentCellTypeFilterIndex].editorCell)
            {
                if (currentSetName != ec.setHeaderName)
                {
                    Instantiate(setNamePrefab, scrollViewContentTransform).text = ec.setHeaderName;
                    currentSetName = ec.setHeaderName;
                    currentGridTransform = Instantiate(setGridGroupPrefab, scrollViewContentTransform).transform;
                }
                Instantiate(cellEditorButtonPrefab, currentGridTransform).image.sprite = ec.cellSprite;
            }

            currentSetName = "";
        }
    }
}
