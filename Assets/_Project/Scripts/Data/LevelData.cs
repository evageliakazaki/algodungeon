using UnityEngine;

namespace AlgoDungeon.Data
{
    public enum SortAlgorithm { BubbleSort, SelectionSort, InsertionSort }

    [CreateAssetMenu(fileName = "NewLevel", menuName = "AlgoDungeon/Level Data")]
    public class LevelData : ScriptableObject
    {
        [Header("Identity")]
        public string levelName;
        public int levelNumber;
        public SortAlgorithm algorithm;

        [Header("Setup")]
        public int[] inputArray;       // π.χ. [5, 2, 8, 1, 9]
        public Vector2 startPosition;  // θέση παίκτη
        public Vector2 tileStartPosition; // πρώτο τέρας
        public float tileSpacing = 1.5f;

        [Header("Pedagogy")]
        [TextArea(3, 10)] public string tutorialText;
        [TextArea(3, 10)] public string pseudocodeText;

        [Header("Star Thresholds (μέγιστες κινήσεις)")]
        public int threeStarsMaxMoves;
        public int twoStarsMaxMoves;
        // αν ξεπεράσει το two stars → 1 αστέρι
    }
}

