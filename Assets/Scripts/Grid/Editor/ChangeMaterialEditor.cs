using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Grid.Cell;

namespace Grid.Editor
{

    public class ChangeMaterialsEditor : UnityEditor.EditorWindow
    {

        [SerializeField]
        private List<Material> newMaterials = new List<Material>(); // Changed to List<Material>

        [SerializeField]
        private bool brokenRoad;

        private SerializedObject serializedObject; // Serialized object for drawing the list
        private SerializedProperty serializedMaterials; // Serialized property for the list
        private SerializedProperty serializedBrokenRoad;


        private GridComponent grid;
        private CellID cellID;

        [MenuItem("Window/Material Changer")]
        public static void ShowWindow()
        {
            // Create the window
            GetWindow<ChangeMaterialsEditor>("Material Changer");
        }

        private void OnEnable()
        {
            // Initialize the serialized object and property
            serializedObject = new SerializedObject(this);
            serializedMaterials = serializedObject.FindProperty("newMaterials");
            serializedBrokenRoad = serializedObject.FindProperty("brokenRoad");

            if (grid == null)
            {
                grid = GameObject.FindAnyObjectByType<GridComponent>();
            }
        }

        private void OnGUI()
        {
            GUILayout.Label("Material Changer", EditorStyles.boldLabel);

            cellID = (CellID)EditorGUILayout.EnumPopup("Cell ID", cellID);

            // Update and draw the serialized list
            serializedObject.Update(); // Update serialized object
            EditorGUILayout.PropertyField(serializedMaterials, new GUIContent("Materials"), true); // Draw the list
            EditorGUILayout.PropertyField(serializedBrokenRoad, new GUIContent("brokenRoad"));
            serializedObject.ApplyModifiedProperties(); // Apply changes

            // Add a button to change the materials
            if (GUILayout.Button("Change Materials"))
            {
                ChangeMaterial();
            }
        }


        public void ChangeMaterial()
        {
            if (newMaterials.Count == 0)
                return;

            int i = 0; // Initialize the index for newMaterials
            grid.InitializeGrid();

            foreach (BaseCell cell in grid.gridArray)
            {
                // If CellID is None, replace all materials
                if (cellID == CellID.None)
                {
                    i = ReplaceMaterials(cell, i);
                }
                // If the cell's ID matches the selected CellID
                else if (cellID == cell.ID)
                {
                    
                    if (brokenRoad && cell.GetType() == typeof(BreakableRoad))
                    {
                        i = ReplaceMaterials(cell, i);
                    }
                    
                    else if (!brokenRoad && cell.GetType() != typeof(BreakableRoad))
                    {
                        i = ReplaceMaterials(cell, i);
                    }
                }
            }
        }

        private int ReplaceMaterials(BaseCell cell, int materialIndex)
        {
            if (cell.meshRenderer.Count == 0)
                return materialIndex;

            foreach (MeshRenderer mr in cell.meshRenderer)
            {
                
                if (materialIndex >= newMaterials.Count)
                    return materialIndex;

                mr.material = newMaterials[materialIndex];
                materialIndex++;
            }

            return materialIndex;
        }
    }
}
