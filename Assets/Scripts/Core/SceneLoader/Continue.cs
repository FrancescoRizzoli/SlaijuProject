using Cysharp.Threading.Tasks;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility;
namespace Core
{
    public class Continue : MonoBehaviour
    {
        public void OnButtonClick()
        {
            LoadCurrentLevel().Forget();
        }
        public async UniTask LoadCurrentLevel()
        {
            await SceneLoader.LoadScene(Settings.GetLevelReached());
        }

    }
}
