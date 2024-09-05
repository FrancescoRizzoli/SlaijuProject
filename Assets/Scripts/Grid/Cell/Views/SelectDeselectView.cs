using Architecture;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Grid.Cell
{
    public class SelectDeselectView : AView
    {

        [SerializeField] private GameObject defaultGraphic = null;
        [SerializeField] private GameObject SelectedGraphic = null;
        [SerializeField] private ParticleSystem[] particleSystems = null;

        private void Start()
        {
            if (defaultGraphic != null)
                defaultGraphic.SetActive(true);

            if (SelectedGraphic != null)
                SelectedGraphic.SetActive(false);
        }

        public override async UniTask ChangeView()
        {
       
            if (defaultGraphic != null)
                defaultGraphic.SetActive(!defaultGraphic.activeSelf);
            if (SelectedGraphic != null)
                SelectedGraphic.SetActive(!SelectedGraphic.activeSelf);
            await UniTask.NextFrame();
        }
        public  async UniTask PlayParticleSystems()
        {
            if (particleSystems != null && particleSystems.Length > 0)
            {
                foreach (ParticleSystem ps in particleSystems)
                {
                    if (ps != null)
                    {
                        ps.Play();
                    }
                }
            }
            await UniTask.NextFrame();

        }
    }
}