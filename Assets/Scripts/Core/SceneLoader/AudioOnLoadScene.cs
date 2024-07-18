using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility;

namespace Core
{
    public class AudioOnLoadScene : MonoBehaviour
    {
        [SerializeField]
        AudioComponent audioComponent;
        private void OnEnable()
        {
            SceneLoader.OnLoadingCompleted += PlayAudio;
        }
        private void OnDisable()
        {
            SceneLoader.OnLoadingCompleted -= PlayAudio;
        }
        private void PlayAudio()
        {

        }
    }
}
