using Grid;
using System;
using UnityEngine;

namespace LevelEditor
{
    [Serializable]
    public struct EditorCell
    {
        public string setHeaderName;
        public BaseCell cellPrefab;
        public Sprite[] cellSprite;
        public bool limited;
        [Min(0)] public int maxQuantity;
    }

    public enum EditorCellType
    {
        Environment,
        Objective,
        Road,
        Frame
    }

    [CreateAssetMenu(fileName = "LevelEditorCellTypeFilterData", menuName = "ScriptableObject/LevelEditor/LevelEditorCellTypeFilterData")]
    public class LevelEditorCellTypeFilterData : ScriptableObject
    {
        public EditorCellType filterType;
        public EditorCell[] editorCell = Array.Empty<EditorCell>();
    }
}
