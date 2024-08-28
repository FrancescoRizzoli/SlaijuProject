using Grid;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor
{
    [Serializable]
    public struct EditorCell
    {
        public BaseCell cellPrefab;
        public Image cellSprite;
    }

    [CreateAssetMenu(fileName = "LevelEditorCellData", menuName = "ScriptableObject/LevelEditor/LevelEditorCellData")]
    public class LevelEditorCellData : ScriptableObject
    {
        public EditorCell[] editorCell = Array.Empty<EditorCell>();
    }
}
