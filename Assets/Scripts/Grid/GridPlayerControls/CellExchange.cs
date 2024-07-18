using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Grid.Cell;
using DG.Tweening; 

namespace Grid
{
    public class CellExchange : MonoBehaviour
    {
        [SerializeField]
        float firstLiftSpeed = 0.25f;
        [SerializeField]
        float exchangeSpeed = 0.5f;
        [SerializeField]
        float endLiftSpeed = 0.25f;
        [SerializeField]
        float liftHeight = 0.5f;
        public async UniTask Exchange(BaseCell cell1, BaseCell cell2)
        {
            Vector3 cell1StartPos = cell1.transform.position;
            Vector3 cell2StartPos = cell2.transform.position;
            cell1.isSwitching = true;
            cell2.isSwitching = true;

            cell1.PlayParticleExchange();
            cell2.PlayParticleExchange();
            // animation sequence
            Sequence cell1Sequence = DOTween.Sequence();
            UniTask task1 = cell1Sequence.Append(cell1.transform.DOMoveY(cell1StartPos.y + liftHeight, firstLiftSpeed))
                          .Append(cell1.transform.DOMove(cell2StartPos + new Vector3(0, liftHeight, 0), exchangeSpeed).SetEase(Ease.InOutQuad))
                          .Append(cell1.transform.DOMoveY(cell2StartPos.y, endLiftSpeed)).ToUniTask();

            Sequence cell2Sequence = DOTween.Sequence();
            UniTask task2 = cell2Sequence.Append(cell2.transform.DOMoveY(cell2StartPos.y + liftHeight * 1.2f, firstLiftSpeed)) // Cell 2 rises slightly higher
                          .Append(cell2.transform.DOMove(cell1StartPos + new Vector3(0, liftHeight * 1.2f, 0), exchangeSpeed).SetEase(Ease.InOutQuad))
                          .Append(cell2.transform.DOMoveY(cell1StartPos.y, endLiftSpeed)).ToUniTask();

            // wait completition
            await UniTask.WhenAll(task1, task2);
            cell1.PlayParticleExchange();
            cell2.PlayParticleExchange();
            cell1.isSwitching = false;
            cell2.isSwitching = false;

        }
    }
}
