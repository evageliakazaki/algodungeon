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
            scoreTracker = FindAnyObjectByType<ScoreTracker>();
        }

        public abstract void HandleTileInteraction(MonsterTile tile);

        protected virtual int CalculateStars()
        {
            return 3;
        }

        protected void CheckCompletion()
        {
            if (!arrayManager.IsSorted())
                return;

            foreach (var tile in arrayManager.Tiles)
            {
                tile.SetState(TileState.Sorted);
            }

            int stars = CalculateStars();

            GameEvents.RoomCompleted(stars);
        }
    }
}