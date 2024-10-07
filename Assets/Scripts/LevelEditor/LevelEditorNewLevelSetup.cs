using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor
{
    public class LevelEditorNewLevelSetup : MonoBehaviour
    {
        [SerializeField] private Button createButton = null;
        [SerializeField] private TextMeshProUGUI levelNameWarningTextArea = null;
        [SerializeField] private BanWordsList banWordsList = null;

        private CustomLevelSave customLevelSave = new CustomLevelSave(LevelEditorController.SAVE_PATH);

        public static string levelName = "";
        public static LevelColor levelColor = LevelColor.Green;
        public static bool isSmallGrid = true;

        private const string LEVEL_NAME_NOT_AVAILABLE = "Name already used! Choose another name for your level...";
        private const string LEVEL_NAME_EMPTY = "Name cannot be empty";
        private const string LEVEL_NAME_BANNED = "The entered level name contains an offensive word that is not allowed in our game";

        void Start()
        {
            levelName = "";
            levelColor = LevelColor.Green;
            isSmallGrid = true;
            levelNameWarningTextArea.gameObject.SetActive(false);
            createButton.interactable = false;
            customLevelSave.Load();
        }

        public void HandleInputFieldChange(string value)
        {
            string trimmedString = value.Trim();

            if (trimmedString != "")
            {
                foreach(CustomGrid savedGrid in customLevelSave.customGrids)
                    if (savedGrid.gridName == trimmedString)
                    {
                        createButton.interactable = false;
                        levelNameWarningTextArea.gameObject.SetActive(true);
                        levelNameWarningTextArea.text = LEVEL_NAME_NOT_AVAILABLE;
                        return;
                    }

                if (ContainsBannedWord(value))
                    return;

                createButton.interactable = true;
                levelNameWarningTextArea.gameObject.SetActive(false);
                levelName = trimmedString;
            }
            else
            {
                createButton.interactable = false;
                levelNameWarningTextArea.gameObject.SetActive(true);
                levelNameWarningTextArea.text = LEVEL_NAME_EMPTY;
            }
        }

        private bool ContainsBannedWord(string value)
        {
            string reformedString = value.ToLower();
            reformedString = Regex.Replace(reformedString, "[^a-zA-Z]", "");

            foreach (string s in banWordsList.banList)
            {
                if (reformedString.Contains(s.ToLower()))
                {
                    createButton.interactable = false;
                    levelNameWarningTextArea.gameObject.SetActive(true);
                    levelNameWarningTextArea.text = LEVEL_NAME_BANNED;
                    return true;
                }
            }

            return false;
        }

        public void HandleLevelColorSelection(int value)
        {
            levelColor = (LevelColor) value;
        }

        public void HandleSmallGridSelected(bool value)
        {
            isSmallGrid = value;
        }
    }
}
