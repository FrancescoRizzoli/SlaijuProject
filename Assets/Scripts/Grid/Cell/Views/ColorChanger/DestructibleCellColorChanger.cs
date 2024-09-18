using UnityEngine;

namespace Grid.Cell
{
    public class DestructibleCellColorChanger : ColorChanger
    {
        [SerializeField] private CellDestructionView destructionView = null;
        [SerializeField] private Material[] emptyGrassColors = new Material[0];

        public override void SetColor(int colorIndex)
        {
            base.SetColor(colorIndex);
            destructionView.frontGrassGameObject.GetComponent<MeshRenderer>().material = emptyGrassColors[colorIndex];
        }
    }
}
