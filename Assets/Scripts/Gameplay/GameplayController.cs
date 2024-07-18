using Grid;
using UnityEngine;

namespace Gameplay
{
    public class GameplayController : MonoBehaviour
    {
        [SerializeField] private GridConstructor gridConstructor = null;
        [SerializeField] private GridSettings gridSettings;

        public void Start () => gridConstructor.CreateGrid(gridSettings.width, gridSettings.height);
    }
}
