using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utility;

namespace Core
{
    public class SceneButtonManager : MonoBehaviour
    {
        [SerializeField]
        SceneName sceneToLoad;
        //[SerializeField]
        //SceneName currentLevel;
        // [SerializeField]
        //bool isAdditive = false; 

        private void Awake()
        {
            Button button = GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(OnButtonClicked);
            }
        }

        public void OnButtonClicked()
        {
            Debug.Log("load");
            LoadLevel().Forget();
        }

        private async UniTask LoadSelectedScene()
        {
            switch (sceneToLoad)
            {
                case SceneName.MainMenu:
                    await SceneLoader.LoadScene((int)SceneName.MainMenu, LoadSceneMode.Single);
                    break;
                case SceneName.Level1:
                case SceneName.Level2:
                case SceneName.Level3:
                case SceneName.Level4:
                    await SceneLoader.LoadScenes((int)SceneName.Play, (int)sceneToLoad);
                    break;
                default:
                    Debug.LogWarning("Scene type not handled");
                    break;
            }
        }
        public async UniTask LoadLevel()
        {
            await SceneLoader.LoadScene((int)sceneToLoad, LoadSceneMode.Single);
        }
    }
}
