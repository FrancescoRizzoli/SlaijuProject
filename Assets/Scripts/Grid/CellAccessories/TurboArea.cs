using Character;
using UnityEngine;

namespace CellAccessoryLogic
{
    [RequireComponent(typeof(Collider))]
    public class TurboArea : MonoBehaviour
    {
        [SerializeField] private float turboSpeed = 2.0f;

        private void OnTriggerEnter(Collider other)
        {
            CharacterStateController character = other.GetComponent<CharacterStateController>();

            if (character != null)
                character.SetCharacterSpeed(turboSpeed);
        }
    }
}
