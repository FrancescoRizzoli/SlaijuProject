using System;
using UnityEngine;

namespace LevelEditor
{
    [CreateAssetMenu(fileName = "BanWordsList", menuName = "ScriptableObject/LevelEditor/BanWordsList")]
    public class BanWordsList : ScriptableObject
    {
        public string[] banList = Array.Empty<string>();
    }
}
