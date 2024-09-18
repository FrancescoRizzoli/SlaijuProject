using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{
    public class LevelGallery : MonoBehaviour
    {
        public List<GameObject> gameObjects;
        private int currentIndex = 0;

        void Start()
        {
            foreach (GameObject obj in gameObjects)
            {
                obj.SetActive(false);
            }

            if (gameObjects.Count > 0)
            {
                gameObjects[currentIndex].SetActive(true);
            }
        }

        public void Next()
        {
            if (gameObjects.Count == 0) return;

            gameObjects[currentIndex].SetActive(false);

            currentIndex = (currentIndex + 1) % gameObjects.Count;

            gameObjects[currentIndex].SetActive(true);
        }

        public void Prev()
        {
            if (gameObjects.Count == 0) return;

            gameObjects[currentIndex].SetActive(false);

            currentIndex = (currentIndex - 1 + gameObjects.Count) % gameObjects.Count;

            gameObjects[currentIndex].SetActive(true);
        }

        public void Direct(int index)
        {
            if (gameObjects.Count == 0 || index < 0 || index >= gameObjects.Count) return;

            gameObjects[currentIndex].SetActive(false);

            currentIndex = index;

            gameObjects[currentIndex].SetActive(true);
        }
    }
}
