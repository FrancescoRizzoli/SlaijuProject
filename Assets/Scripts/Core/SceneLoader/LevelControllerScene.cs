using Cysharp.Threading.Tasks;
using UnityEngine;
using Utility;

namespace Core
{
    public class LevelControllerScene : MonoBehaviour
    {
        public SceneName SceneName;
        
        public void LoadNextLevel()
        {
            SceneLoader.LoadScene(((int)SceneName) + 1).Forget();
        }
        public void Reload()
        {
            SceneLoader.ReloadCurrentLevel((int)SceneName).Forget();
        }


    }
}
