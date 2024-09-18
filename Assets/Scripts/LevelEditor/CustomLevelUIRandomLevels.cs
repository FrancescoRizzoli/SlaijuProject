using System.Collections.Generic;
using UnityEngine;

namespace LevelEditor
{
    public class CustomLevelUIRandomLevels : MonoBehaviour
    {
        [SerializeField] private CustomLevelLoaderButton[] buttons = new CustomLevelLoaderButton[4];

        private List<CustomGrid> gridsBuffer = new List<CustomGrid>();

        public void SetRandomLevels(CustomLevelLoaderController controller, CustomLevelLoaderUIController uiController)
        {
            gridsBuffer = new List<CustomGrid> (controller.customLevelSave.customGrids);

            for (int i = 0; i < buttons.Length; i++)
            {
                if (gridsBuffer.Count == 0)
                    return;

                int index = Random.Range(0, gridsBuffer.Count);
                buttons[i].gameObject.SetActive(true);
                buttons[i].Init(gridsBuffer[index], controller, uiController);
                gridsBuffer.RemoveAt(index);
            }
        }
    }
}
