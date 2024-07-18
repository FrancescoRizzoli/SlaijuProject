using UnityEngine;

namespace Grid
{
    public class GridConstructor : MonoBehaviour
    {
        [SerializeField] private GameObject gridCellPrefab = null;

        public void CreateGrid(int width, int height)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    GameObject go = Instantiate<GameObject>(gridCellPrefab);
                    go.name = $"GridCell[{i},{j}]";
                    go.transform.parent = transform;
                    go.transform.localPosition = Vector3.right * i + Vector3.forward * j;
                }
            }
        }
    }
}
