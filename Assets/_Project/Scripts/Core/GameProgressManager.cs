using UnityEngine;
using UnityEngine.SceneManagement;

namespace AlgoDungeon.Core
{
    public static class GameProgressManager
    {
        private const string StarsKeyPrefix = "LevelStars_";
        private const string CompletedKeyPrefix = "LevelCompleted_";
        private const string UnlockedKeyPrefix = "LevelUnlocked_";

        public static void CompleteLevel(string levelName, int stars)
        {
            if (stars <= 0)
                return;

            int previousStars = GetStars(levelName);

            if (stars > previousStars)
            {
                PlayerPrefs.SetInt(StarsKeyPrefix + levelName, stars);
            }

            PlayerPrefs.SetInt(CompletedKeyPrefix + levelName, 1);

            UnlockNextLevel();

            PlayerPrefs.Save();

            Debug.Log($"Progress Saved: {levelName} completed with {stars} stars.");
        }

        public static int GetStars(string levelName)
        {
            return PlayerPrefs.GetInt(StarsKeyPrefix + levelName, 0);
        }

        public static bool IsCompleted(string levelName)
        {
            return PlayerPrefs.GetInt(CompletedKeyPrefix + levelName, 0) == 1;
        }

        public static bool IsUnlocked(string levelName)
        {
            // Η πρώτη Selection πίστα πρέπει να είναι πάντα unlocked.
            if (levelName == "Selection_Level_01")
                return true;

            return PlayerPrefs.GetInt(UnlockedKeyPrefix + levelName, 0) == 1;
        }

        public static void UnlockLevel(string levelName)
        {
            PlayerPrefs.SetInt(UnlockedKeyPrefix + levelName, 1);
            PlayerPrefs.Save();
        }

        private static void UnlockNextLevel()
        {
            int currentIndex = SceneManager.GetActiveScene().buildIndex;
            int nextIndex = currentIndex + 1;

            if (nextIndex >= SceneManager.sceneCountInBuildSettings)
                return;

            string nextScenePath = SceneUtility.GetScenePathByBuildIndex(nextIndex);
            string nextSceneName = System.IO.Path.GetFileNameWithoutExtension(nextScenePath);

            // Ξεκλειδώνουμε μόνο Selection levels, όχι MainMenu, AlgorithmSelect κλπ.
            if (nextSceneName.StartsWith("Selection_Level_"))
            {
                UnlockLevel(nextSceneName);
                Debug.Log($"Unlocked next level: {nextSceneName}");
            }
        }

        public static void ResetProgress()
        {
            PlayerPrefs.DeleteAll();

            // Ξεκλειδώνουμε ξανά την πρώτη πίστα.
            UnlockLevel("Selection_Level_01");

            PlayerPrefs.Save();

            Debug.Log("Progress reset.");
        }
    }
}
