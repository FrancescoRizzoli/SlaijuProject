using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Grid.Cell
{
    public class GeneratorCell : DestructibleCell
    {
        [SerializeField] private ShieldedCityCell targetCity = null;

        private bool damageInProgress = false;

#if UNITY_EDITOR
        private void OnValidate() => ID = CellID.Generator;
#endif

        public override async UniTask GetDamage()
        {
            if (damageInProgress)
                return;

            damageInProgress = true;
            await base.GetDamage();

            if (currentHealth == 0)
                targetCity.RemoveShields();

            damageInProgress = false;
        }
    }
}
