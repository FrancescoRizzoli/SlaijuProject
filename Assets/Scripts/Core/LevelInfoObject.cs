using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Core
{

    
    [CreateAssetMenu(fileName = "LevelInfo", menuName = "ScriptableObject/LevelInfo", order = 1)]
    public class LevelInfoObject : ScriptableObject
    {
        public List<LevelInfo> levels;
        public LevelInfo GetLevelInfoBySceneName(SceneName sceneName)
        {
            foreach (LevelInfo info in levels)
            {
                if(info.sceneName == sceneName)
                {
                    return info;
                }
                
            }
            return null; // Restituisce null se nessun livello corrisponde al nome della scena
        }

    }
}
