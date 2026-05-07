using UnityEngine;
using AlgoDungeon.Core;

namespace AlgoDungeon.Sorting
{
    public abstract class SortingRoomBase : MonoBehaviour
    {
        protected ArrayManager arrayManager;
        protected ScoreTracker scoreTracker;

        public virtual void Initialize(ArrayManager manager)
        {
            arrayManager = manager;
            scoreTracker = FindFirstObjectByType<ScoreTracker>();
        }

        // Κάθε αλγόριθμος ορίζει δικούς του κανόνες
        public abstract void HandleTileInteraction(MonsterTile tile);

        protected void CheckCompletion()
        {
            if (arrayManager.IsSorted())
            {
                int stars = scoreTracker.CalculateStars(arrayManager.Data);
                GameEvents.RoomCompleted(stars);
            }
        }
    }
}
