using UnityEngine;
using UnityEditor;

namespace Grid.Editor
{
    [CustomEditor(typeof(GridComponent))]
    public class GridComponentEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GridComponent gridComponent = (GridComponent)target;

            if (GUILayout.Button("Initialize Grid"))
            {
                gridComponent.InitializeGrid();
            }

            if (gridComponent.gridArray != null &&
                gridComponent.gridArray.GetLength(0) == gridComponent.width &&
                gridComponent.gridArray.GetLength(1) == gridComponent.height)
            {
                EditorGUILayout.LabelField("Grid Array", EditorStyles.boldLabel);

                for (int y = gridComponent.height - 1; y >= 0; y--)
                {
                    EditorGUILayout.BeginHorizontal();
                    for (int x = 0; x < gridComponent.width; x++)
                    {
                        BaseCell cell = gridComponent.gridArray[x, y];
                        string cellInfo = cell != null ? $"({x},{y}) {cell.name}" : $"({x},{y}) null";
                        EditorGUILayout.LabelField(cellInfo, GUILayout.Width(100));
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
            else
            {
                EditorGUILayout.LabelField("Grid dimensions do not match array size. Please re-initialize the grid.");
            }
        }
    }
}
