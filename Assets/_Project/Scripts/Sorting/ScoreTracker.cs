using UnityEngine;
using AlgoDungeon.Core;
using AlgoDungeon.Data;

namespace AlgoDungeon.Sorting
{
    public class ScoreTracker : MonoBehaviour
    {
        public int Comparisons { get; private set; }
        public int Swaps { get; private set; }

        private void OnEnable()
        {
            GameEvents.OnTilesCompared += HandleCompared;
            GameEvents.OnTilesSwapped += HandleSwapped;
        }

        private void OnDisable()
        {
            GameEvents.OnTilesCompared -= HandleCompared;
            GameEvents.OnTilesSwapped -= HandleSwapped;
        }

        private void HandleCompared(int a, int b) => Comparisons++;
        private void HandleSwapped(int a, int b) => Swaps++;

        public int CalculateStars(LevelData data)
        {
            int total = Comparisons + Swaps;
            if (total <= data.threeStarsMaxMoves) return 3;
            if (total <= data.twoStarsMaxMoves) return 2;
            return 1;
        }

        public void Reset()
        {
            Comparisons = 0;
            Swaps = 0;
        }
    }
}

