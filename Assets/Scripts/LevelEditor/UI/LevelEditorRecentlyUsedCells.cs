using UnityEngine;

namespace LevelEditor
{
    public class LevelEditorRecentlyUsedCells : MonoBehaviour
    {
        [SerializeField] private LevelEditorCellButton[] cellButton = new LevelEditorCellButton[4];

        public void Init(LevelEditorController editorController)
        {
            foreach (LevelEditorCellButton button in cellButton)
                button.Init(editorController);
        }

        public void AddCell(LevelEditorCellButton target)
        {
            Debug.Log("Aggiungo?");

            // return if it's already in the recent list
            foreach (LevelEditorCellButton cell in cellButton)
                if (cell.cellPrefab != null && cell.cellPrefab == target.cellPrefab)
                    return;

            // shift down the recent cells
            for (int i = cellButton.Length - 1; i > 0; i--)
            {
                if (cellButton[i - 1].cellPrefab == null)
                    continue;
                else
                    UpdateRecentCell(i, cellButton[i - 1]);
            }

            UpdateRecentCell(0, target);
        }

        private void UpdateRecentCell(int index, LevelEditorCellButton target)
        {
            cellButton[index].cellPrefab = target.cellPrefab;
            cellButton[index].button.image.sprite = target.button.image.sprite;
            cellButton[index].cellType = target.cellType;
            cellButton[index].button.onClick.RemoveAllListeners();
            cellButton[index].SetOnClickEvents();
            cellButton[index].gameObject.SetActive(true);
        }
    }
}
