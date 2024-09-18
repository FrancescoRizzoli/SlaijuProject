using System;
using UnityEngine;

namespace Grid.Cell
{
    public class RoadCrackColorChanger : ColorChanger
    {
        [Serializable]
        public struct CellPhaseColor
        {
            public Material[] cellPhaseMaterials;
        }

        [SerializeField] private BreakableRoadView breakableRoadView = null;
        [SerializeField] private CellPhaseColor[] cellPhaseMaterialColors = new CellPhaseColor[0];
        [SerializeField] private Material[] emptyGrassColors = new Material[0];

        public override void SetColor(int colorIndex)
        {
            base.SetColor(colorIndex);
            breakableRoadView.cellPhaseMaterial = cellPhaseMaterialColors[colorIndex].cellPhaseMaterials;
            breakableRoadView.emptyGraphic.GetComponent<MeshRenderer>().material = emptyGrassColors[colorIndex];
        }
    }
}
