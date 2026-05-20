using UnityEngine;
using AlgoDungeon.Core;

namespace AlgoDungeon.Sorting
{
    public class BubbleSortRoom : SortingRoomBase
    {
        [SerializeField] private float swapAnimationDuration = 0.3f;
        [SerializeField] private int maxWrongMoves = 3;

        private MonsterTile firstSelected;
        private int wrongMoves = 0;
        private bool roomEnded = false;

        public override void Initialize(ArrayManager manager)
        {
            base.Initialize(manager);

            firstSelected = null;
            wrongMoves = 0;
            roomEnded = false;

            if (arrayManager != null && arrayManager.IsSorted())
            {
                CompleteRoom();
            }
        }

        public override void HandleTileInteraction(MonsterTile tile)
        {
            if (roomEnded)
                return;

            if (arrayManager == null)
                return;

            if (arrayManager.IsSorted())
            {
                CompleteRoom();
                return;
            }

            if (tile == null)
                return;

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
                GameEvents.TilesCompared(idxA, idxB);

                firstSelected.SetState(TileState.Comparing);
                tile.SetState(TileState.Comparing);

                RegisterWrongMove("Λάθος επιλογή! Στο Bubble Sort συγκρίνουμε μόνο γειτονικά στοιχεία.");

                firstSelected = null;
                Invoke(nameof(ResetComparingStates), 0.4f);
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
                RegisterWrongMove("Λάθος/περιττή σύγκριση! Αυτά τα στοιχεία είναι ήδη στη σωστή σειρά.");
            }

            firstSelected = null;

            float delay = didSwap ? swapAnimationDuration + 0.05f : 0.4f;
            Invoke(nameof(DelayedCheck), delay);
        }

        private void RegisterWrongMove(string message)
        {
            wrongMoves++;

            Debug.Log($"{message} Λάθη: {wrongMoves}/{maxWrongMoves}");

            if (wrongMoves >= maxWrongMoves)
            {
                roomEnded = true;
                firstSelected = null;

                ResetComparingStates();

                Debug.Log("Game Over! Έκανες 3 λάθη.");

                GameEvents.RoomCompleted(0);
            }
        }

        private void DelayedCheck()
        {
            if (roomEnded)
                return;

            ResetComparingStates();

            if (arrayManager != null && arrayManager.IsSorted())
            {
                CompleteRoom();
                return;
            }

            CheckCompletion();
        }

        private void CompleteRoom()
        {
            if (roomEnded)
                return;

            roomEnded = true;
            firstSelected = null;

            if (arrayManager != null && arrayManager.Tiles != null)
            {
                foreach (var tile in arrayManager.Tiles)
                {
                    tile.SetState(TileState.Sorted);
                }
            }

            CheckCompletion();
        }

        private void ResetComparingStates()
        {
            if (arrayManager == null || arrayManager.Tiles == null)
                return;

            foreach (var t in arrayManager.Tiles)
            {
                if (t.GetState() != TileState.Sorted)
                    t.SetState(TileState.Idle);
            }
        }

        protected override int CalculateStars()
        {
            if (wrongMoves == 0)
                return 3;

            if (wrongMoves == 1)
                return 2;

            return 1;
        }
    }
}