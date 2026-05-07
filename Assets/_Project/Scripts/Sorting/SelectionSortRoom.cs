using UnityEngine;
using AlgoDungeon.Core;

namespace AlgoDungeon.Sorting
{
    /// <summary>
    /// Δωμάτιο Selection Sort.
    /// Ο παίκτης βρίσκει το ελάχιστο στοιχείο και το swap-άρει
    /// με το πρώτο unsorted στοιχείο.
    /// </summary>
    public class SelectionSortRoom : SortingRoomBase
    {
        [SerializeField] private float swapAnimationDuration = 0.3f;

        private int sortedUpTo = 0;       // όλα τα tiles πριν από αυτή τη θέση είναι sorted
        private MonsterTile minCandidate; // το tile που θεωρείται τωρινό ελάχιστο
        private MonsterTile firstOfPass;  // η αρχή του pass (το "slot" που θα γεμίσει)

        public override void Initialize(ArrayManager manager)
        {
            base.Initialize(manager);
            sortedUpTo = 0;
            MarkSortedTiles();
            HighlightCurrentSlot();
        }

        public override void HandleTileInteraction(MonsterTile tile)
        {
            // Αγνοούμε ήδη-ταξινομημένα tiles
            if (tile.CurrentIndex < sortedUpTo)
            {
                Debug.Log("Selection Sort: αυτό το στοιχείο είναι ήδη ταξινομημένο!");
                return;
            }

            // Πρώτη επιλογή pass — ορίζουμε το "slot" (firstOfPass)
            if (firstOfPass == null)
            {
                firstOfPass = arrayManager.Tiles[sortedUpTo];
                minCandidate = tile;
                tile.SetState(TileState.Selected);
                return;
            }

            // Σύγκριση με τον τωρινό υποψήφιο ελαχίστου
            GameEvents.TilesCompared(minCandidate.CurrentIndex, tile.CurrentIndex);

            if (tile.Value < minCandidate.Value)
            {
                minCandidate.SetState(TileState.Idle);
                minCandidate = tile;
                tile.SetState(TileState.Selected);
                Debug.Log($"Selection Sort: νέο ελάχιστο = {tile.Value}");
            }
            else
            {
                tile.SetState(TileState.Comparing);
                Invoke(nameof(ResetComparing), 0.3f);
            }
        }

        /// <summary>
        /// Ο παίκτης επιβεβαιώνει το swap (π.χ. πατώντας Enter/Space όταν
        /// έχει ήδη επιλέξει το ελάχιστο). Συνδέστε αυτό σε ένα UI Button
        /// ή σε ένα KeyCode.Return handler στη scene.
        /// </summary>
        public void ConfirmSwap()
        {
            if (minCandidate == null || firstOfPass == null) return;

            int slotIdx = firstOfPass.CurrentIndex; // = sortedUpTo
            int minIdx  = minCandidate.CurrentIndex;

            if (slotIdx != minIdx)
            {
                arrayManager.SwapTiles(slotIdx, minIdx);
            }

            // Το tile στη θέση sortedUpTo είναι τώρα στη σωστή θέση
            arrayManager.Tiles[sortedUpTo].SetState(TileState.Sorted);
            GameEvents.TileMarkedSorted(sortedUpTo);
            sortedUpTo++;

            firstOfPass  = null;
            minCandidate = null;

            Invoke(nameof(AfterSwap), swapAnimationDuration + 0.05f);
        }

        private void AfterSwap()
        {
            HighlightCurrentSlot();
            CheckCompletion();
        }

        private void HighlightCurrentSlot()
        {
            if (sortedUpTo < arrayManager.Tiles.Count)
                arrayManager.Tiles[sortedUpTo].SetState(TileState.Selected);
        }

        private void MarkSortedTiles()
        {
            for (int i = 0; i < sortedUpTo; i++)
                arrayManager.Tiles[i].SetState(TileState.Sorted);
        }

        private void ResetComparing()
        {
            foreach (var t in arrayManager.Tiles)
                if (t.GetState() == TileState.Comparing)
                    t.SetState(TileState.Idle);
        }
    }
}
