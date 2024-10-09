using TMPro;
using UnityEngine;

namespace LevelEditor
{
    public class LevelNameUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI[] nameTextArea = null;

        private void Start()
        {
            foreach (TextMeshProUGUI t in nameTextArea)
                t.text = $"LEVEL\n{LevelEditorNewLevelSetup.levelName}";
        }
    }
}
