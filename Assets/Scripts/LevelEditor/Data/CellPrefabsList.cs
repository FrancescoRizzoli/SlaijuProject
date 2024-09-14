using Grid;
using UnityEngine;

namespace LevelEditor
{
    [CreateAssetMenu(fileName = "CellPrefabsList", menuName = "ScriptableObject/LevelEditor/CellPrefabsList")]
    public class CellPrefabsList : ScriptableObject
    {
        public BaseCell[] cellPrefabs = new BaseCell[0];
    }
}
