using Core;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utility;

namespace LevelEditor
{
    [RequireComponent(typeof(Button))]
    public class RandomCustomLevelSelection : MonoBehaviour
    {
        private CustomLevelSave customLevelSave = new CustomLevelSave(LevelEditorController.SAVE_PATH);
        private Button button = null;

        private void Start()
        {
            customLevelSave.Load();
            button = GetComponent<Button>();
            button.onClick.AddListener(RandomLevel);
            button.interactable = customLevelSave.customGrids.Count > 0;
        }

        private void RandomLevel()
        {
            int levelIndex = Random.Range(0, customLevelSave.customGrids.Count);
            LevelEditorNewLevelSetup.levelName = customLevelSave.customGrids[levelIndex].gridName;
            LevelEditorNewLevelSetup.isSmallGrid = customLevelSave.customGrids[levelIndex].isSmallGrid;
            LevelEditorNewLevelSetup.levelColor = customLevelSave.customGrids[levelIndex].gridColor;
            SceneLoader.LoadScene((int)SceneName.CustomLevelPlay, LoadSceneMode.Single).Forget();
        }
    }
}
