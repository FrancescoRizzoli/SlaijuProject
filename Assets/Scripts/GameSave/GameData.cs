using System;
using System.IO;
using UnityEngine;

namespace GameSave
{
    [Serializable]
    public class GameData
    {
        private string SAVE_PATH = "/SlaijuReborn.save";

        public GameData(string save_path) 
        {
            SAVE_PATH = save_path;
        }

        public string ToJson() => JsonUtility.ToJson(this);

        public void Save()
        {
            string jsonString = ToJson();
            File.WriteAllText(Application.persistentDataPath + SAVE_PATH, jsonString);
        }

        public void Load()
        {
            if (!File.Exists(Application.persistentDataPath + SAVE_PATH))
                return;

            JsonUtility.FromJsonOverwrite(File.ReadAllText(Application.persistentDataPath + SAVE_PATH), this);
        }
    }
}
