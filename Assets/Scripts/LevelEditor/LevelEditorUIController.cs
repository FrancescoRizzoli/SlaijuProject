using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor
{
    public class LevelEditorUIController : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private LevelEditorCellData environmentCellData = null;
        [SerializeField] private LevelEditorCellData objectiveCellData = null;
        [SerializeField] private LevelEditorCellData roadCellData = null;
        [Header("Buttons")]
        [SerializeField] private Button environmentButton = null;
        [SerializeField] private Button objectiveButton = null;
        [SerializeField] private Button roadButton = null;
        [Header("Scroll view")]
        [SerializeField] private Transform scrollViewContentTransform = null;
        [SerializeField] private TextMeshProUGUI setNamePrefab = null;
        [SerializeField] private GameObject setGridGroupPrefab = null;
        [SerializeField] private Button cellEditorButtonPrefab = null;

        private string currentSetName = "";
        private Button previousCellButtonClicked = null;
        private Transform currentGridTransform = null;

        private void Start()
        {
            environmentButton.onClick.AddListener(ShowEnvironmentCells);
            objectiveButton.onClick.AddListener(ShowObjectiveCells);
            roadButton.onClick.AddListener(ShowRoadCells);
        }

        private void ShowEnvironmentCells()
        {
            SwitchNonInteractableButton(environmentButton);
            ClearViewport();
            PopulateViewport(environmentCellData);
        }

        private void ShowObjectiveCells()
        {
            SwitchNonInteractableButton(objectiveButton);
            ClearViewport();
            PopulateViewport(objectiveCellData);
        }

        private void ShowRoadCells()
        {
            SwitchNonInteractableButton(roadButton);
            ClearViewport();
            PopulateViewport(roadCellData);
        }

        private void ClearViewport()
        {
            foreach(Transform child in scrollViewContentTransform)
                Destroy(child.gameObject);
        }

        private void SwitchNonInteractableButton(Button targetButton)
        {
            if (previousCellButtonClicked != null)
                previousCellButtonClicked.interactable = true;

            targetButton.interactable = false;
            previousCellButtonClicked = targetButton;
        }

        private void PopulateViewport(LevelEditorCellData targetEditorCellData)
        {
            foreach (EditorCell ec in targetEditorCellData.editorCell)
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
