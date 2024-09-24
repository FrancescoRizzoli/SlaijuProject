using Architecture;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using Utility;

namespace Grid.Cell
{
    public class BreakableRoadView : AView
    {
        [SerializeField] private SelectDeselectView selectDeselectView = null;
        [SerializeField] private GameObject whiteSelectionCubes = null;
        [SerializeField] private MeshRenderer roadGraphic = null;
        public Material[] cellPhaseMaterial = Array.Empty<Material>();
        public GameObject emptyGraphic = null;
        [SerializeField] private AudioClip damageAudioClip = null;
        [SerializeField] private ParticleSystem damageParticle = null;
        [SerializeField] private ParticleSystem roadToGrassParticle= null;


        public override async UniTask ChangeView()
        {
            if (roadToGrassParticle != null)
            {
                roadToGrassParticle.Play();
                await UniTask.WaitForSeconds(roadToGrassParticle.main.duration / 2);
            }
            emptyGraphic.SetActive(true);
            roadGraphic.gameObject.SetActive(false);
            if (selectDeselectView.SelectedGraphic != null)
                selectDeselectView.SelectedGraphic.SetActive(false);
            selectDeselectView.SelectedGraphic = whiteSelectionCubes;
        }

        public async UniTask DamageCell(int cellCurrentHealth)
        {
            if ( damageAudioClip != null)
                AudioManager.instance.PlayAudioClipPreDefinedSource(damageAudioClip, roadGraphic.GetComponent<AudioSource>());
            
            if (damageParticle != null)
            {
                damageParticle.Play();
                await UniTask.WaitForSeconds(damageParticle.main.duration / 2);
            }

            roadGraphic.material = cellPhaseMaterial[cellCurrentHealth-1];
            await UniTask.NextFrame();
        }
    }
}
