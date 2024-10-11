using UnityEngine;

namespace LevelEditor
{
    public class LevelEditorVisualCell : MonoBehaviour
    {
        [SerializeField] private GameObject grayBox = null;
        [SerializeField] private GameObject selectCubes = null;
        [SerializeField] private GameObject notEmptyCellSelectCubes = null;
        [SerializeField] private GameObject deleteCubes = null;
        [SerializeField] private GameObject rotateView = null;

        public void ToggleGrayBox(bool value) => grayBox.SetActive(value);
        public void ToggleSelectCubes(bool value, bool notEmptyCell)
        {
            if (!value)
            {
                selectCubes.SetActive(false);
                notEmptyCellSelectCubes.SetActive(false);
                return;
            }

            selectCubes.SetActive(!notEmptyCell);
            notEmptyCellSelectCubes.SetActive(notEmptyCell);
        }
        public void ToggleDeleteCubes(bool value) => deleteCubes.SetActive(value);
        public void ToggleRotateView(bool value) => rotateView.SetActive(value);
    }
}
