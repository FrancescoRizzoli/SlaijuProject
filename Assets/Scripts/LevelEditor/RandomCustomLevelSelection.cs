using Core;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
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
        private List<CustomGrid> playableGrids = new List<CustomGrid>();
        

        private void Start()
        {
            customLevelSave.Load();
            button = GetComponent<Button>();
            button.onClick.AddListener(RandomLevel);
            button.interactable = customLevelSave.customGrids.Count > 0;

            foreach (CustomGrid cg in customLevelSave.customGrids)
                if (cg.gridComplete)
                    playableGrids.Add(cg);
        }

        private void RandomLevel()
        {
            int levelIndex = Random.Range(0, playableGrids.Count);
            LevelEditorNewLevelSetup.levelName = playableGrids[levelIndex].gridName;
            LevelEditorNewLevelSetup.isSmallGrid = playableGrids[levelIndex].isSmallGrid;
            LevelEditorNewLevelSetup.levelColor = playableGrids[levelIndex].gridColor;
            SceneLoader.LoadScene((int)SceneName.CustomLevelPlay, LoadSceneMode.Single).Forget();
        }
    }
}
