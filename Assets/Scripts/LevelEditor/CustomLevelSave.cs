using GameSave;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LevelEditor
{
    [Serializable]
    public struct CustomGrid
    {
        public string gridName;
        public bool gridComplete;
        public bool isSmallGrid;
        public LevelColor gridColor;
        public List<CustomGridCell> gridCells;
    }

    [Serializable]
    public class CustomGridCell
    {
        public string cellName;
        public Vector2Int positionInGrid;
        public Vector3 forwardDirection;
    }

    [Serializable]
    public class CustomLevelSave : GameData
    {
        public List<CustomGrid> customGrids = new List<CustomGrid>();

        public CustomLevelSave(string save_path) : base(save_path) { }
    }
}
