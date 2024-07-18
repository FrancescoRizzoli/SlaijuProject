using UnityEngine;

namespace Grid
{
    [CreateAssetMenu(fileName = "GridSettings", menuName = "ScriptableObject/Grid/GridSettings")]
    public class GridSettings : ScriptableObject
    {
        public int width = 1;
        public int height = 1;
    }
}
