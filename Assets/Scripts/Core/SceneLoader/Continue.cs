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
        [SerializeField]
        GameObject playText;
        [SerializeField]
        GameObject continueText;
        private void Start()
        {
            int levelReached = Settings.GetLevelReached();
            if(levelReached > (int)SceneName.Level1)
            {
                playText.SetActive(false);
                continueText.SetActive(true);
            }
        }
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
