using UnityEngine;
using UnityEditor;

namespace Grid.Editor
{

    public class ChangeMaterialsEditor : UnityEditor.EditorWindow
    {
        private Material newMaterial;
        private GridComponent grid;
        private CellID cellID;


        [MenuItem("Window/Material Changer")]
        public static void ShowWindow()
        {
            // Crea la finestra
            GetWindow<ChangeMaterialsEditor>("Material Changer");
        }
        private void OnEnable()
        {
            if (grid == null)
            {
                grid = GameObject.FindAnyObjectByType<GridComponent>();
            }
        }

        private void OnGUI()
        {

            GUILayout.Label("Material Changer", EditorStyles.boldLabel);

            cellID = (CellID)EditorGUILayout.EnumPopup("Cell ID",cellID);

            // Crea un campo per inserire il materiale
            newMaterial = (Material)EditorGUILayout.ObjectField("New Material", newMaterial, typeof(Material), false);

            // Aggiungi un pulsante per cambiare i materiali
            if (GUILayout.Button("Change Materials"))
            {
                ChangeMaterial();
            }
        }


        public void ChangeMaterial()
        {
            foreach(BaseCell cell in grid.gridArray)
            {
                if (newMaterial == null)
                    return;
                if(cellID == CellID.None)
                {
                    if(cell.meshRenderer != null)
                    {
                        cell.meshRenderer.material = newMaterial;
                    }
                }
                else if(cellID == cell.ID)
                {
                    if (cell.meshRenderer != null)
                    {
                        cell.meshRenderer.material = newMaterial;
                    }
                }
            }
        }
    }
}
