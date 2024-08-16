using Architecture;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Grid.Cell
{
    public class BreakableRoadView : AView
    {
        [SerializeField] private GameObject roadGraphic = null;
        [SerializeField] private GameObject emptyGraphic = null;

        public override async UniTask ChangeView()
        {
            emptyGraphic.SetActive(true);
            roadGraphic.SetActive(false);
            await UniTask.NextFrame();
        }
    }
}
