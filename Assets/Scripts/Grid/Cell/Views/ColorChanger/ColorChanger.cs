using UnityEngine;

namespace Grid.Cell
{
    public class ColorChanger : MonoBehaviour
    {
        [SerializeField] private MeshRenderer[] targetMesh = new MeshRenderer[0];
        [SerializeField] private Material[] colorMaterial = new Material[0];

        public virtual void SetColor(int colorIndex)
        {
            foreach (MeshRenderer renderer in targetMesh)
                renderer.material = colorMaterial[colorIndex];
        }
    }
}
