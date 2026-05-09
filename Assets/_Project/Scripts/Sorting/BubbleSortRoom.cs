using UnityEngine;
using AlgoDungeon.Core;

namespace AlgoDungeon.Sorting
{
    public class BubbleSortRoom : SortingRoomBase
    {
        [SerializeField] private float swapAnimationDuration = 0.3f;

        private MonsterTile firstSelected;
        private int wrongComparisons = 0;

        public override void HandleTileInteraction(MonsterTile tile)
        {
            if (firstSelected == null)
            {
                firstSelected = tile;
                tile.SetState(TileState.Selected);
                return;
            }

            if (firstSelected == tile)
            {
                firstSelected.SetState(TileState.Idle);
                firstSelected = null;
                return;
            }

            int idxA = firstSelected.CurrentIndex;
            int idxB = tile.CurrentIndex;

            if (Mathf.Abs(idxA - idxB) != 1)
            {
                wrongComparisons++;
                Debug.Log("Λάθος σύγκριση: μπορείς να συγκρίνεις μόνο γειτονικά στοιχεία.");

                firstSelected.SetState(TileState.Idle);
                firstSelected = null;
                return;
            }

            GameEvents.TilesCompared(idxA, idxB);

            firstSelected.SetState(TileState.Comparing);
            tile.SetState(TileState.Comparing);

            int leftIdx = Mathf.Min(idxA, idxB);
            int rightIdx = Mathf.Max(idxA, idxB);

            bool didSwap = false;

            if (arrayManager.Tiles[leftIdx].Value > arrayManager.Tiles[rightIdx].Value)
            {
                arrayManager.SwapTiles(leftIdx, rightIdx);
                didSwap = true;
            }
            else
            {
                wrongComparisons++;
                Debug.Log("Λάθος/περιττή σύγκριση: τα στοιχεία ήταν ήδη στη σωστή σειρά.");
            }

            firstSelected = null;

            float delay = didSwap ? swapAnimationDuration + 0.05f : 0.05f;
            Invoke(nameof(DelayedCheck), delay);
        }

        private void DelayedCheck()
        {
            ResetComparingStates();
            CheckCompletion();
        }

        private void ResetComparingStates()
        {
            foreach (var t in arrayManager.Tiles)
            {
                if (t.GetState() == TileState.Comparing)
                    t.SetState(TileState.Idle);
            }
        }

        protected override int CalculateStars()
        {
            if (wrongComparisons == 0)
                return 3;

            if (wrongComparisons == 1)
                return 2;

            return 1;
        }
    }
}