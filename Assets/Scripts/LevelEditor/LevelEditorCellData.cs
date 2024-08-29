using Grid;
using System;
using UnityEngine;

namespace LevelEditor
{
    [Serializable]
    public struct EditorCell
    {
        public BaseCell cellPrefab;
        public Sprite cellSprite;
    }

    [CreateAssetMenu(fileName = "LevelEditorCellData", menuName = "ScriptableObject/LevelEditor/LevelEditorCellData")]
    public class LevelEditorCellData : ScriptableObject
    {
        public EditorCell[] editorCell = Array.Empty<EditorCell>();
    }
}
