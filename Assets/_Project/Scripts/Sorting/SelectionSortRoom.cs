using UnityEngine;
using AlgoDungeon.Core;

namespace AlgoDungeon.Sorting
{
    public class SelectionSortRoom : SortingRoomBase
    {
        [SerializeField] private float swapAnimationDuration = 0.3f;
        [SerializeField] private int maxWrongMoves = 3;

        private MonsterTile firstSelected;
        private int currentPosition = 0;
        private int wrongMoves = 0;
        private bool roomEnded = false;

        public override void HandleTileInteraction(MonsterTile tile)
        {
            if (roomEnded)
                return;

            if (arrayManager == null)
                return;

            if (arrayManager.IsSorted())
                return;

            if (firstSelected == null)
            {
                if (tile.CurrentIndex != currentPosition)
                {
                    GameEvents.TilesCompared(currentPosition, tile.CurrentIndex);

                    tile.SetState(TileState.Comparing);

                    RegisterWrongMove(
                        $"Λάθος επιλογή! Πρέπει πρώτα να επιλέξεις τη θέση {currentPosition}."
                    );

                    Invoke(nameof(ResetStates), 0.4f);
                    return;
                }

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

            int selectedIndex = firstSelected.CurrentIndex;
            int chosenIndex = tile.CurrentIndex;

            GameEvents.TilesCompared(selectedIndex, chosenIndex);

            firstSelected.SetState(TileState.Comparing);
            tile.SetState(TileState.Comparing);

            int minIndex = FindMinIndexFrom(currentPosition);

            if (chosenIndex == minIndex)
            {
                Debug.Log("Σωστή επιλογή minimum!");

                if (currentPosition != minIndex)
                {
                    arrayManager.SwapTiles(currentPosition, minIndex);
                }

                currentPosition++;
            }
            else
            {
                RegisterWrongMove(
                    $"Λάθος επιλογή! Το μικρότερο στοιχείο είναι στη θέση {minIndex}, όχι στη θέση {chosenIndex}."
                );
            }

            firstSelected = null;

            Invoke(nameof(DelayedCheck), swapAnimationDuration + 0.05f);
        }

        private int FindMinIndexFrom(int startIndex)
        {
            int minIndex = startIndex;

            for (int i = startIndex + 1; i < arrayManager.Tiles.Count; i++)
            {
                if (arrayManager.Tiles[i].Value < arrayManager.Tiles[minIndex].Value)
                {
                    minIndex = i;
                }
            }

            return minIndex;
        }

        private void RegisterWrongMove(string message)
        {
            wrongMoves++;

            Debug.Log($"{message} Λάθη: {wrongMoves}/{maxWrongMoves}");

            if (wrongMoves >= maxWrongMoves)
            {
                roomEnded = true;
                firstSelected = null;

                Debug.Log("Game Over! Έκανες 3 λάθη.");

                GameEvents.RoomCompleted(0);
            }
        }

        private void DelayedCheck()
        {
            if (roomEnded)
                return;

            ResetStates();
            CheckCompletion();
        }

        private void ResetStates()
        {
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