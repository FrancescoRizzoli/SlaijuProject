using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{
    public class UITutorial : ScreenBase
    {
        [SerializeField]
        GameObject[] images;
        [SerializeField]
        Button nextButton;
        [SerializeField]
        Button prevButton;

        private int currentIndex = 0;

        void Start()
        {
            UpdateGallery();

            nextButton.onClick.AddListener(NextImage);
            prevButton.onClick.AddListener(PrevImage);
        }

        void UpdateGallery()
        {
            for (int i = 0; i < images.Length; i++)
            {
                images[i].SetActive(i == currentIndex);
            }
        }

        void NextImage()
        {
            currentIndex = (currentIndex + 1) % images.Length;
            UpdateGallery();
        }

        void PrevImage()
        {
            currentIndex = (currentIndex - 1 + images.Length) % images.Length;
            UpdateGallery();
        }
    }
}
