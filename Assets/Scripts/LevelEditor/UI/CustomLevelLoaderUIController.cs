using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor
{
    public class CustomLevelLoaderUIController : MonoBehaviour
    {
        [Header("Level Color Filter")]
        [SerializeField] private TextMeshProUGUI colorFilterName = null;
        [SerializeField] private Button colorFilterNextButton = null;
        [SerializeField] private Button colorFilterPreviousButton = null;
        [SerializeField] private TextMeshProUGUI setName = null;
        [SerializeField] private string allColorsFilterName = "All colors";
        [Header("Scroll view")]
        [SerializeField] private Transform setGridGroup = null;
        [SerializeField] private CustomLevelLoaderButton customLevelLoaderButtonPrefab = null;
        [Header("Play Level Button")]
        [SerializeField] private Button playButton = null; 
        [Header("Random Levels")]
        [SerializeField] private CustomLevelUIRandomLevels randomLevels = null;
        [Header("Edit Button")]
        [SerializeField] private Button editButton= null;
        
        private CustomLevelLoaderController controller = null;
        private List<CustomLevelLoaderButton> levelButtons = new List<CustomLevelLoaderButton>();
        private int[] colorFilters;
        private int currentColorFilterIndex = -1;
        private string currentNameFilter = "";

        public void Init(CustomLevelLoaderController controller)
        {
            this.controller = controller;

            InitColorFilterData();
            randomLevels.SetRandomLevels(controller, this);
            PopulateViewport();
            SetFilter(true);

            colorFilterNextButton.onClick.AddListener(NextFilter);
            colorFilterPreviousButton.onClick.AddListener(PreviousFilter);
        }

        private void InitColorFilterData()
        {
            int colorsNumber = Enum.GetValues(typeof(LevelColor)).Cast<int>().Max() + 1;
            colorFilters = new int[colorsNumber + 1];
            colorFilters[0] = -1;
            for (int i = 1; i < colorFilters.Length; i++)
                colorFilters[i] = i - 1;
        }

        private void NextFilter() => SetFilter(true);
        private void PreviousFilter() => SetFilter(false);

        private void SetFilter(bool next)
        {
            if (next)
                currentColorFilterIndex = (currentColorFilterIndex + 1) % colorFilters.Length;
            else if (--currentColorFilterIndex < 0)
                currentColorFilterIndex += colorFilters.Length;

            SetCurrentFilter();
        }

        private void SetCurrentFilter()
        {
            if (colorFilters[currentColorFilterIndex] == -1)
            {
                setName.text = allColorsFilterName;
                colorFilterName.text = allColorsFilterName;
                foreach (CustomLevelLoaderButton button in levelButtons)
                {
                    button.gameObject.SetActive(true);
                    ApplyNameFilter(button);
                }
            }
            else
            {
                int colorValue = colorFilters[currentColorFilterIndex];
                colorFilterName.text = ((LevelColor)colorValue).ToString();
                setName.text = colorFilterName.text;
                foreach (CustomLevelLoaderButton button in levelButtons)
                {
                    if (button.customGrid.gridColor == (LevelColor)colorValue)
                    {
                        button.gameObject.SetActive(true);
                        ApplyNameFilter(button);
                    }
                    else
                        button.gameObject.SetActive(false);
                }
            }
        }

        private void ApplyNameFilter(CustomLevelLoaderButton button)
        {
            if (currentNameFilter == "" || button.customGrid.gridName.StartsWith(currentNameFilter))
                button.gameObject.SetActive(true);
            else
                button.gameObject.SetActive(false);
        }

        public void SearchLevel(string levelName)
        {
            currentNameFilter = levelName;
            SetCurrentFilter();
        }

        private void PopulateViewport()
        {
            for (int i = 0; i < controller.customLevelSave.customGrids.Count; i++)
            {
                CustomLevelLoaderButton button = Instantiate<CustomLevelLoaderButton>(customLevelLoaderButtonPrefab, setGridGroup);
                button.Init(controller.customLevelSave.customGrids[i], controller, this);
                levelButtons.Add(button);
            }
        }

        public void ToggleButtons(bool value)
        {
            editButton.interactable = true;
            playButton.interactable = value;
        }
    }
}
