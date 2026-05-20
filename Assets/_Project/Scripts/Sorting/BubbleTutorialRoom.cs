using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using AlgoDungeon.Core;

namespace AlgoDungeon.Sorting
{
    public class BubbleTutorialRoom : SortingRoomBase
    {
        [Header("Tutorial UI")]
        [SerializeField] private TextMeshProUGUI instructionText;
        [SerializeField] private TextMeshProUGUI feedbackText;
        [SerializeField] private Button startButton;

        [Header("Scene Names")]
        [SerializeField] private string firstBubbleLevel = "Bubble_Level_01";

        [Header("Timing")]
        [SerializeField] private float swapAnimationDuration = 0.3f;

        private MonsterTile firstSelected;
        private int tutorialStep = 0;
        private bool waiting = false;

        public override void Initialize(ArrayManager manager)
        {
            base.Initialize(manager);

            firstSelected = null;
            tutorialStep = 0;
            waiting = false;

            if (startButton != null)
                startButton.gameObject.SetActive(false);

            SetStep(0);
        }

        public override void HandleTileInteraction(MonsterTile tile)
        {
            if (waiting || tile == null || arrayManager == null)
                return;

            switch (tutorialStep)
            {
                case 0:
                    SelectFirstBubblePairLeft(tile);
                    break;

                case 1:
                    SelectFirstBubblePairRight(tile);
                    break;

                case 2:
                    SelectSecondBubblePairLeft(tile);
                    break;

                case 3:
                    SelectSecondBubblePairRight(tile);
                    break;

                case 4:
                    SelectThirdBubblePairLeft(tile);
                    break;

                case 5:
                    SelectThirdBubblePairRight(tile);
                    break;
            }
        }

        private void SelectFirstBubblePairLeft(MonsterTile tile)
        {
            // Initial array: 3, 1, 4, 2, 5
            // First Bubble comparison: 3 and 1
            if (tile.Value == 3)
            {
                firstSelected = tile;
                tile.SetState(TileState.Selected);

                SetStep(1);
                SetFeedback("Correct! 3 is the left monster of the first pair.");
            }
            else
            {
                SetFeedback("Not yet. First select 3, the left monster of the first pair.");
            }
        }

        private void SelectFirstBubblePairRight(MonsterTile tile)
        {
            if (firstSelected == null)
            {
                SetStep(0);
                return;
            }

            if (tile.Value == 1 && AreAdjacent(firstSelected, tile))
            {
                GameEvents.TilesCompared(firstSelected.CurrentIndex, tile.CurrentIndex);

                firstSelected.SetState(TileState.Comparing);
                tile.SetState(TileState.Comparing);

                SetFeedback("Correct! 3 is bigger than 1, so they swap.");

                SwapSelectedPair(tile, nameof(AfterFirstSwap));
            }
            else
            {
                SetFeedback("Select the adjacent monster 1. In Bubble Sort we compare neighbors.");
            }
        }

        private void AfterFirstSwap()
        {
            // Array becomes: 1, 3, 4, 2, 5
            waiting = false;
            firstSelected = null;
            ResetStates();

            SetStep(2);
        }

        private void SelectSecondBubblePairLeft(MonsterTile tile)
        {
            // Next useful Bubble comparison: 4 and 2
            if (tile.Value == 4)
            {
                firstSelected = tile;
                tile.SetState(TileState.Selected);

                SetStep(3);
                SetFeedback("Correct! Now compare 4 with its right neighbor.");
            }
            else
            {
                SetFeedback("Now select 4. It must be compared with the monster on its right.");
            }
        }

        private void SelectSecondBubblePairRight(MonsterTile tile)
        {
            if (firstSelected == null)
            {
                SetStep(2);
                return;
            }

            if (tile.Value == 2 && AreAdjacent(firstSelected, tile))
            {
                GameEvents.TilesCompared(firstSelected.CurrentIndex, tile.CurrentIndex);

                firstSelected.SetState(TileState.Comparing);
                tile.SetState(TileState.Comparing);

                SetFeedback("Correct! 4 is bigger than 2, so they swap.");

                SwapSelectedPair(tile, nameof(AfterSecondSwap));
            }
            else
            {
                SetFeedback("Select the adjacent monster 2. Bubble Sort compares neighboring monsters.");
            }
        }

        private void AfterSecondSwap()
        {
            // Array becomes: 1, 3, 2, 4, 5
            waiting = false;
            firstSelected = null;
            ResetStates();

            SetStep(4);
        }

        private void SelectThirdBubblePairLeft(MonsterTile tile)
        {
            // Final useful comparison: 3 and 2
            if (tile.Value == 3)
            {
                firstSelected = tile;
                tile.SetState(TileState.Selected);

                SetStep(5);
                SetFeedback("Correct! Now compare 3 with its right neighbor.");
            }
            else
            {
                SetFeedback("Now select 3. It must be compared with the monster on its right.");
            }
        }

        private void SelectThirdBubblePairRight(MonsterTile tile)
        {
            if (firstSelected == null)
            {
                SetStep(4);
                return;
            }

            if (tile.Value == 2 && AreAdjacent(firstSelected, tile))
            {
                GameEvents.TilesCompared(firstSelected.CurrentIndex, tile.CurrentIndex);

                firstSelected.SetState(TileState.Comparing);
                tile.SetState(TileState.Comparing);

                SetFeedback("Correct! 3 is bigger than 2, so they swap.");

                SwapSelectedPair(tile, nameof(AfterThirdSwap));
            }
            else
            {
                SetFeedback("Select the adjacent monster 2.");
            }
        }

        private void AfterThirdSwap()
        {
            // Array becomes: 1, 2, 3, 4, 5
            waiting = false;
            firstSelected = null;
            ResetStates();

            foreach (var tile in arrayManager.Tiles)
                tile.SetState(TileState.Sorted);

            SetStep(6);
        }

        private bool AreAdjacent(MonsterTile a, MonsterTile b)
        {
            return Mathf.Abs(a.CurrentIndex - b.CurrentIndex) == 1;
        }

        private void SwapSelectedPair(MonsterTile secondTile, string callbackName)
        {
            int idxA = firstSelected.CurrentIndex;
            int idxB = secondTile.CurrentIndex;

            int leftIdx = Mathf.Min(idxA, idxB);
            int rightIdx = Mathf.Max(idxA, idxB);

            arrayManager.SwapTiles(leftIdx, rightIdx);

            waiting = true;

            Invoke(callbackName, swapAnimationDuration + 0.35f);
        }

        private void SetStep(int step)
        {
            tutorialStep = step;

            switch (tutorialStep)
            {
                case 0:
                    SetInstruction(
                        "Controls:\n" +
                        "Move with W A S D or the Arrow Keys.\n" +
                        "Press E near a monster to select it.\n\n" +
                        "Bubble Sort rule:\n" +
                        "Compare two adjacent monsters.\n" +
                        "If the left monster is bigger than the right one,\n" +
                        "they swap positions.\n\n" +
                        "Move to 3 and press E."
                    );
                    SetFeedback("");
                    break;

                case 1:
                    SetInstruction(
                        "Good!\n\n" +
                        "In Bubble Sort, we compare neighbors.\n" +
                        "Now select the monster directly next to 3.\n\n" +
                        "Move to 1 and press E."
                    );
                    break;

                case 2:
                    SetInstruction(
                        "Great! 3 and 1 swapped.\n\n" +
                        "Now continue scanning the row.\n" +
                        "The next useful comparison is 4 and 2.\n\n" +
                        "Move to 4 and press E."
                    );
                    break;

                case 3:
                    SetInstruction(
                        "Good!\n\n" +
                        "Now select the adjacent monster on the right.\n\n" +
                        "Move to 2 and press E."
                    );
                    break;

                case 4:
                    SetInstruction(
                        "Nice! 4 moved to the right.\n\n" +
                        "Now compare 3 with 2.\n" +
                        "This will finish the sorted row.\n\n" +
                        "Move to 3 and press E."
                    );
                    break;

                case 5:
                    SetInstruction(
                        "Good!\n\n" +
                        "Now select the adjacent monster 2.\n\n" +
                        "Move to 2 and press E."
                    );
                    break;

                case 6:
                    SetInstruction(
                        "Excellent!\n\n" +
                        "You sorted the monsters using Bubble Sort:\n" +
                        "1. Compare adjacent monsters.\n" +
                        "2. If the left one is bigger, swap them.\n" +
                        "3. Repeat until the row is sorted.\n\n" +
                        "3 mistakes in a dungeon means Game Over."
                    );

                    SetFeedback("You are ready!");

                    if (startButton != null)
                        startButton.gameObject.SetActive(true);
                    else
                        Debug.LogError("Start Button is not assigned in BubbleTutorialRoom.");
                    break;
            }
        }

        private void SetInstruction(string message)
        {
            if (instructionText != null)
                instructionText.text = message;
        }

        private void SetFeedback(string message)
        {
            if (feedbackText != null)
                feedbackText.text = message;
        }

        private void ResetStates()
        {
            foreach (var tile in arrayManager.Tiles)
            {
                if (tile.GetState() != TileState.Sorted)
                    tile.SetState(TileState.Idle);
            }
        }

        public void StartDungeon()
        {
            SceneManager.LoadScene(firstBubbleLevel);
        }
    }
}