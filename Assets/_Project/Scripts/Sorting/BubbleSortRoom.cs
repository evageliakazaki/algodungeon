using UnityEngine;
using AlgoDungeon.Core;

namespace AlgoDungeon.Sorting
{
    /// <summary>
    /// Δωμάτιο Bubble Sort — ο παίκτης επιλέγει δύο ΓΕΙΤΟΝΙΚΑ tiles.
    /// Αν το αριστερό > δεξί, γίνεται swap.
    /// </summary>
    public class BubbleSortRoom : SortingRoomBase
    {
        [SerializeField] private float swapAnimationDuration = 0.3f;

        private MonsterTile firstSelected;

        public override void HandleTileInteraction(MonsterTile tile)
        {
            // Πρώτη επιλογή
            if (firstSelected == null)
            {
                firstSelected = tile;
                tile.SetState(TileState.Selected);
                return;
            }

            // Επιλέχθηκε το ίδιο tile — deselect
            if (firstSelected == tile)
            {
                firstSelected.SetState(TileState.Idle);
                firstSelected = null;
                return;
            }

            int idxA = firstSelected.CurrentIndex;
            int idxB = tile.CurrentIndex;

            // Μόνο γειτονικά επιτρέπονται στο Bubble Sort
            if (Mathf.Abs(idxA - idxB) != 1)
            {
                Debug.Log("Bubble Sort: μπορείς να συγκρίνεις μόνο γειτονικά στοιχεία!");
                firstSelected.SetState(TileState.Idle);
                firstSelected = null;
                return;
            }

            // Σύγκριση
            GameEvents.TilesCompared(idxA, idxB);
            firstSelected.SetState(TileState.Comparing);
            tile.SetState(TileState.Comparing);

            int leftIdx  = Mathf.Min(idxA, idxB);
            int rightIdx = Mathf.Max(idxA, idxB);

            bool didSwap = false;
            if (arrayManager.Tiles[leftIdx].Value > arrayManager.Tiles[rightIdx].Value)
            {
                arrayManager.SwapTiles(leftIdx, rightIdx);
                didSwap = true;
            }

            firstSelected = null;

            // BUG FIX: Περιμένουμε το animation (0.3s) πριν ελέγξουμε αν τελείωσε.
            // Το CheckCompletion() ΔΕΝ καλείται αμέσως γιατί τα Tiles
            // ακόμα κινούνται και οι θέσεις δεν έχουν ενημερωθεί οπτικά.
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
                if (t.GetState() == TileState.Comparing)
                    t.SetState(TileState.Idle);
        }
    }
}
