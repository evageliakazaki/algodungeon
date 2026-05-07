using UnityEngine;
using AlgoDungeon.Core;

namespace AlgoDungeon.Sorting
{
    /// <summary>
    /// Δωμάτιο Insertion Sort.
    /// Ο παίκτης επιλέγει ένα unsorted tile και το "σύρει" αριστερά
    /// μέχρι να βρει τη σωστή θέση του, κάνοντας swaps.
    ///
    /// ΣΗΜΕΙΩΣΗ: Αυτό το αρχείο πρέπει να μετονομαστεί από
    /// "InsertationSortRoom.cs" σε "InsertionSortRoom.cs"
    /// (τυπογραφικό λάθος στο αρχικό αρχείο).
    /// </summary>
    public class InsertionSortRoom : SortingRoomBase
    {
        [SerializeField] private float swapAnimationDuration = 0.3f;

        private int sortedBoundary = 1;    // ο πρώτος unsorted index
        private MonsterTile activeKey;     // το tile που «εισάγουμε»
        private int keyCurrentIndex;       // τρέχων index του key μέσα στο pass

        public override void Initialize(ArrayManager manager)
        {
            base.Initialize(manager);
            sortedBoundary = 1;
            // Το πρώτο στοιχείο θεωρείται ήδη sorted
            if (arrayManager.Tiles.Count > 0)
                arrayManager.Tiles[0].SetState(TileState.Sorted);
        }

        public override void HandleTileInteraction(MonsterTile tile)
        {
            // Αν δεν έχουμε επιλεγμένο key ακόμα
            if (activeKey == null)
            {
                // Μόνο το tile ακριβώς στο sortedBoundary μπορεί να είναι το key
                if (tile.CurrentIndex != sortedBoundary)
                {
                    Debug.Log($"Insertion Sort: πρέπει να επιλέξεις το στοιχείο στη θέση {sortedBoundary}.");
                    return;
                }
                activeKey = tile;
                keyCurrentIndex = sortedBoundary;
                tile.SetState(TileState.Selected);
                return;
            }

            // Έχουμε key — ο παίκτης πατάει το αριστερό γείτονα για σύγκριση
            int leftIdx = keyCurrentIndex - 1;
            if (leftIdx < 0)
            {
                // Φτάσαμε αρχή — το key μένει εδώ
                FinishPass();
                return;
            }

            MonsterTile leftTile = arrayManager.Tiles[leftIdx];

            if (tile != leftTile)
            {
                Debug.Log("Insertion Sort: σύγκρινε με τον αριστερό γείτονα!");
                return;
            }

            GameEvents.TilesCompared(keyCurrentIndex, leftIdx);

            if (arrayManager.Tiles[keyCurrentIndex].Value < leftTile.Value)
            {
                // Swap και προχωράμε αριστερά
                arrayManager.SwapTiles(leftIdx, keyCurrentIndex);
                keyCurrentIndex = leftIdx;
                Invoke(nameof(RefreshAfterSwap), swapAnimationDuration + 0.05f);
            }
            else
            {
                // Το key βρήκε τη θέση του
                FinishPass();
            }
        }

        private void RefreshAfterSwap()
        {
            if (keyCurrentIndex > 0)
                activeKey?.SetState(TileState.Selected);
            else
                FinishPass();
        }

        private void FinishPass()
        {
            // Σήμαινε όλα τα sorted tiles
            for (int i = 0; i <= sortedBoundary; i++)
            {
                arrayManager.Tiles[i].SetState(TileState.Sorted);
                GameEvents.TileMarkedSorted(i);
            }

            activeKey = null;
            sortedBoundary++;
            CheckCompletion();
        }
    }
}
