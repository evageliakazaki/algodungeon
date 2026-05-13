using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using AlgoDungeon.Core;

namespace AlgoDungeon.Sorting
{
    public class SelectionTutorialRoom : SortingRoomBase
    {
        [Header("Tutorial UI")]
        [SerializeField] private TextMeshProUGUI instructionText;
        [SerializeField] private TextMeshProUGUI feedbackText;
        [SerializeField] private Button startButton;

        [Header("Scene Names")]
        [SerializeField] private string firstSelectionLevel = "Selection_Level_01";

        [Header("Timing")]
        [SerializeField] private float swapAnimationDuration = 0.3f;

        private MonsterTile firstSelected;
        private int tutorialStep = 0;
        private bool waiting = false;

        public override void Initialize(ArrayManager manager)
        {
            base.Initialize(manager);

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
                    SelectFirstCurrent(tile);
                    break;

                case 1:
                    SelectFirstMinimum(tile);
                    break;

                case 2:
                    SelectSecondCurrent(tile);
                    break;

                case 3:
                    SelectSecondMinimum(tile);
                    break;

                case 4:
                    SelectThirdCurrent(tile);
                    break;

                case 5:
                    SelectThirdMinimum(tile);
                    break;
            }
        }

        private void SelectFirstCurrent(MonsterTile tile)
        {
            // Initial array: 4, 3, 5, 1, 2
            if (tile.CurrentIndex == 0 && tile.Value == 4)
            {
                firstSelected = tile;
                tile.SetState(TileState.Selected);

                SetStep(1);
                SetFeedback("Correct! 4 is the first unsorted monster.");
            }
            else
            {
                SetFeedback("Not yet. First select the first unsorted monster: 4.");
            }
        }

        private void SelectFirstMinimum(MonsterTile tile)
        {
            // Smallest in 4,3,5,1,2 is 1
            if (tile.Value == 1)
            {
                GameEvents.TilesCompared(firstSelected.CurrentIndex, tile.CurrentIndex);

                firstSelected.SetState(TileState.Comparing);
                tile.SetState(TileState.Comparing);

                SetFeedback("Correct! 1 is the smallest monster. Swapping 4 and 1...");

                arrayManager.SwapTiles(firstSelected.CurrentIndex, tile.CurrentIndex);

                firstSelected = null;
                waiting = true;

                Invoke(nameof(AfterFirstSwap), swapAnimationDuration + 0.35f);
            }
            else
            {
                SetFeedback("Look for the smallest monster in the unsorted row. It is 1.");
            }
        }

        private void AfterFirstSwap()
        {
            waiting = false;
            ResetStates();

            // Array is now: 1, 3, 5, 4, 2
            if (arrayManager.Tiles.Count > 0)
                arrayManager.Tiles[0].SetState(TileState.Sorted);

            SetStep(2);
        }

        private void SelectSecondCurrent(MonsterTile tile)
        {
            // First sorted: 1
            // Next first unsorted monster is 3
            if (tile.CurrentIndex == 1 && tile.Value == 3)
            {
                firstSelected = tile;
                tile.SetState(TileState.Selected);

                SetStep(3);
                SetFeedback("Correct! 3 is now the first unsorted monster.");
            }
            else
            {
                SetFeedback("The first position is sorted. Now select the next first unsorted monster: 3.");
            }
        }

        private void SelectSecondMinimum(MonsterTile tile)
        {
            // Remaining unsorted part: 3,5,4,2
            // Smallest is 2
            if (tile.Value == 2)
            {
                GameEvents.TilesCompared(firstSelected.CurrentIndex, tile.CurrentIndex);

                firstSelected.SetState(TileState.Comparing);
                tile.SetState(TileState.Comparing);

                SetFeedback("Correct! 2 is the smallest remaining monster. Swapping 3 and 2...");

                arrayManager.SwapTiles(firstSelected.CurrentIndex, tile.CurrentIndex);

                firstSelected = null;
                waiting = true;

                Invoke(nameof(AfterSecondSwap), swapAnimationDuration + 0.35f);
            }
            else
            {
                SetFeedback("From the remaining unsorted part, the smallest monster is 2.");
            }
        }

        private void AfterSecondSwap()
        {
            waiting = false;
            ResetStates();

            // Array is now: 1, 2, 5, 4, 3
            if (arrayManager.Tiles.Count > 0)
                arrayManager.Tiles[0].SetState(TileState.Sorted);

            if (arrayManager.Tiles.Count > 1)
                arrayManager.Tiles[1].SetState(TileState.Sorted);

            SetStep(4);
        }

        private void SelectThirdCurrent(MonsterTile tile)
        {
            // First sorted: 1, 2
            // Next first unsorted monster is 5
            if (tile.CurrentIndex == 2 && tile.Value == 5)
            {
                firstSelected = tile;
                tile.SetState(TileState.Selected);

                SetStep(5);
                SetFeedback("Correct! 5 is now the first unsorted monster.");
            }
            else
            {
                SetFeedback("The first two positions are sorted. Now select the next first unsorted monster: 5.");
            }
        }

        private void SelectThirdMinimum(MonsterTile tile)
        {
            // Remaining unsorted part: 5,4,3
            // Smallest is 3
            if (tile.Value == 3)
            {
                GameEvents.TilesCompared(firstSelected.CurrentIndex, tile.CurrentIndex);

                firstSelected.SetState(TileState.Comparing);
                tile.SetState(TileState.Comparing);

                SetFeedback("Correct! 3 is the smallest remaining monster. Swapping 5 and 3...");

                arrayManager.SwapTiles(firstSelected.CurrentIndex, tile.CurrentIndex);

                firstSelected = null;
                waiting = true;

                Invoke(nameof(AfterThirdSwap), swapAnimationDuration + 0.35f);
            }
            else
            {
                SetFeedback("From the remaining unsorted part, the smallest monster is 3.");
            }
        }

        private void AfterThirdSwap()
        {
            waiting = false;
            ResetStates();

            // Array is now: 1, 2, 3, 4, 5
            foreach (var tile in arrayManager.Tiles)
                tile.SetState(TileState.Sorted);

            SetStep(6);
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
                        "Selection Sort rule:\n" +
                        "First select the first unsorted monster.\n" +
                        "Then select the smallest monster in the unsorted part.\n\n" +
                        "Move to 4 and press E."
                    );
                    SetFeedback("");
                    break;

                case 1:
                    SetInstruction(
                        "Good!\n\n" +
                        "Now find the smallest monster in the unsorted row.\n" +
                        "Move to 1 and press E."
                    );
                    break;

                case 2:
                    SetInstruction(
                        "Great! The first position is now sorted.\n\n" +
                        "Now select the next first unsorted monster.\n" +
                        "Move to 3 and press E."
                    );
                    break;

                case 3:
                    SetInstruction(
                        "Good!\n\n" +
                        "Now select the smallest monster in the remaining unsorted part.\n" +
                        "Move to 2 and press E."
                    );
                    break;

                case 4:
                    SetInstruction(
                        "Nice! The first two positions are sorted.\n\n" +
                        "Now select the next first unsorted monster.\n" +
                        "Move to 5 and press E."
                    );
                    break;

                case 5:
                    SetInstruction(
                        "Good!\n\n" +
                        "Now select the smallest monster in the remaining unsorted part.\n" +
                        "Move to 3 and press E."
                    );
                    break;

                case 6:
                    SetInstruction(
                        "Excellent!\n\n" +
                        "You sorted the monsters using Selection Sort:\n" +
                        "1. Select the first unsorted monster.\n" +
                        "2. Select the smallest monster in the unsorted part.\n" +
                        "3. Swap them.\n\n" +
                        "3 mistakes in a dungeon means Game Over."
                    );

                    SetFeedback("You are ready!");

                    if (startButton != null)
                        startButton.gameObject.SetActive(true);
                    else
                        Debug.LogError("Start Button is not assigned in SelectionTutorialRoom.");
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
            SceneManager.LoadScene(firstSelectionLevel);
        }
    }
}