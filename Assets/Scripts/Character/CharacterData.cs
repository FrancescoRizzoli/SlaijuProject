using UnityEngine;

namespace Character
{
    [CreateAssetMenu(fileName = "CharacterData", menuName = "ScriptableObject/Character/CharacterData")]
    public class CharacterData : ScriptableObject
    {
        [Header("Movement")]
        [Min(0.01f)] public float movementSpeed = 1.0f;

        [Header("Attack")]
        [Min(0.0f)] public float idleTimeAfterCellDestruction = 5.0f;

        [Header("Level start")]
        [Min(0.0f)] public float waitTimeAtStart = 2.0f;

        [Header("Death - camera shake")]
        [Min(0.01f)] public float cameraShakeDuration = 1.0f;
        [Min(0.01f)] public float cameraShakeMagnitude = 0.25f;
    }
}
