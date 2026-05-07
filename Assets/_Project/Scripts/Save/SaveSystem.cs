using System.IO;
using UnityEngine;

namespace AlgoDungeon.Save
{
    [System.Serializable]
    public class SaveData
    {
        public int[] starsPerLevel = new int[20];
        public int highestUnlockedLevel = 1;
    }

    public static class SaveSystem
    {
        private static string Path => Application.persistentDataPath + "/save.json";

        public static void Save(SaveData data)
        {
            File.WriteAllText(Path, JsonUtility.ToJson(data, true));
        }

        public static SaveData Load()
        {
            if (!File.Exists(Path)) return new SaveData();
            return JsonUtility.FromJson<SaveData>(File.ReadAllText(Path));
        }
    }
}

