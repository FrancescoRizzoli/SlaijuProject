using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using TMPro;

namespace Gameplay
{
    public class LevelUIInfo : MonoBehaviour
    {
        [SerializeField]
        LevelControllerScene levelControllerScene;
        [SerializeField]
        LevelInfoObject levelInfo;
        [SerializeField]
        TMP_Text levelName;
        [SerializeField]
        TMP_Text objectiveNumber;
        [SerializeField]
        TMP_Text timeDev;
        [SerializeField]
        TMP_Text moveDev;
        private void Awake()
        {
            LevelInfo info = levelInfo.GetLevelInfoBySceneName(levelControllerScene.SceneName);
            if (info == null)
                return;

            levelName.text = info.name;
            timeDev.text = info.time.ToString();
            moveDev.text = info.Moves.ToString();
            objectiveNumber.text = info.objetctiveNumber.ToString();
        }
    }
}
