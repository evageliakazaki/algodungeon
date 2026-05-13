using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AlgoDungeon.UI
{
    public class SelectionTutorialController : MonoBehaviour
    {
        [Header("Scene Names")]
        [SerializeField] private string algorithmSelectScene = "AlgorithmSelect";
        [SerializeField] private string firstSelectionLevel = "Selection_Level_01";

        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI instructionText;
        [SerializeField] private TextMeshProUGUI feedbackText;
        [SerializeField] private Transform demoTilesParent;
        [SerializeField] private Button startButton;

        [Header("Tile Visuals")]
        [SerializeField] private Sprite slimeSprite;

        private readonly List<Button> tileButtons = new List<Button>();
        private readonly List<TextMeshProUGUI> tileTexts = new List<TextMeshProUGUI>();

        private int[] values = { 5, 1, 4, 2, 8 };

        private int tutorialStep = 0;

        private readonly Color normalColor = new Color(0.35f, 0.85f, 0.55f);
        private readonly Color selectedColor = new Color(1f, 0.85f, 0.2f);
        private readonly Color correctColor = new Color(0.4f, 1f, 0.45f);

        private void Start()
        {
            if (startButton != null)
                startButton.gameObject.SetActive(false);

            CreateDemoTiles();
            SetStep(0);
        }

        private void CreateDemoTiles()
        {
            ClearDemoTiles();

            float spacing = 130f;
            float startX = -((values.Length - 1) * spacing) / 2f;

            for (int i = 0; i < values.Length; i++)
            {
                int index = i;

                GameObject tileObject = new GameObject(
                    $"DemoTile_{i}",
                    typeof(RectTransform),
                    typeof(Image),
                    typeof(Button)
                );

                tileObject.transform.SetParent(demoTilesParent, false);

                RectTransform rect = tileObject.GetComponent<RectTransform>();
                rect.sizeDelta = new Vector2(100, 100);
                rect.anchoredPosition = new Vector2(startX + i * spacing, 0);

                Image image = tileObject.GetComponent<Image>();
                image.sprite = slimeSprite;
                image.color = normalColor;
                image.raycastTarget = true;

                Button button = tileObject.GetComponent<Button>();
                button.onClick.AddListener(() => OnTileClicked(index));

                GameObject textObject = new GameObject(
                    "ValueText",
                    typeof(RectTransform),
                    typeof(TextMeshProUGUI)
                );

                textObject.transform.SetParent(tileObject.transform, false);

                RectTransform textRect = textObject.GetComponent<RectTransform>();
                textRect.anchorMin = new Vector2(0.5f, 0.5f);
                textRect.anchorMax = new Vector2(0.5f, 0.5f);
                textRect.pivot = new Vector2(0.5f, 0.5f);
                textRect.anchoredPosition = new Vector2(0, 58);
                textRect.sizeDelta = new Vector2(120, 70);

                TextMeshProUGUI valueText = textObject.GetComponent<TextMeshProUGUI>();
                valueText.text = values[i].ToString();
                valueText.fontSize = 42;
                valueText.alignment = TextAlignmentOptions.Center;
                valueText.color = Color.white;
                valueText.fontStyle = FontStyles.Bold;
                valueText.raycastTarget = false;

                tileButtons.Add(button);
                tileTexts.Add(valueText);
            }
        }

        private void ClearDemoTiles()
        {
            tileButtons.Clear();
            tileTexts.Clear();

            if (demoTilesParent == null)
                return;

            for (int i = demoTilesParent.childCount - 1; i >= 0; i--)
            {
                Destroy(demoTilesParent.GetChild(i).gameObject);
            }
        }

        private void SetStep(int step)
        {
            tutorialStep = step;
            ResetTileColors();

            if (feedbackText != null)
                feedbackText.text = "";

            switch (tutorialStep)
            {
                case 0:
                    instructionText.text =
                        "Controls:\n" +
                        "Move with W A S D or the Arrow Keys.\n" +
                        "Press E near a monster to select it.\n\n" +
                        "In this tutorial, click the monsters to practice.\n" +
                        "Click any monster to begin.";
                    break;

                case 1:
                    instructionText.text =
                        "Selection Sort works in steps.\n" +
                        "First, choose the first unsorted monster.\n" +
                        "Then choose the smallest monster in the unsorted part.\n\n" +
                        "Step 1: Select the first unsorted monster.\n" +
                        "In this row, select 5.";
                    HighlightTile(0, selectedColor);
                    break;

                case 2:
                    instructionText.text =
                        "Good! Now select the smallest monster in the unsorted part.\n" +
                        "In this row, select 1.";
                    HighlightTile(0, selectedColor);
                    break;

                case 3:
                    instructionText.text =
                        "Correct! Selection Sort swaps the first unsorted monster with the smallest one.";
                    SwapValues(0, 1);
                    feedbackText.text = "5 and 1 swapped!";
                    Invoke(nameof(GoToStep4), 1.2f);
                    break;

                case 4:
                    instructionText.text =
                        "The first position is now sorted.\n" +
                        "Next, select the first unsorted monster again.\n" +
                        "This time, select 5.";
                    HighlightTile(1, selectedColor);
                    break;

                case 5:
                    instructionText.text =
                        "Now select the smallest monster in the remaining unsorted part.\n" +
                        "This time, select 2.";
                    HighlightTile(1, selectedColor);
                    break;

                case 6:
                    instructionText.text =
                        "Correct! Selection Sort swaps the first unsorted monster with the smallest one.";
                    SwapValues(1, 3);
                    feedbackText.text = "5 and 2 swapped!";
                    Invoke(nameof(GoToStep7), 1.2f);
                    break;

                case 7:
                    instructionText.text =
                        "Excellent! You now know the Selection Sort rule:\n" +
                        "first unsorted monster, then smallest monster.\n\n" +
                        "3 mistakes in a dungeon means Game Over.";
                    feedbackText.text = "You are ready!";

                    if (startButton != null)
                        startButton.gameObject.SetActive(true);
                    break;
            }
        }

        private void OnTileClicked(int index)
        {
            switch (tutorialStep)
            {
                case 0:
                    SetStep(1);
                    break;

                case 1:
                    if (index == 0)
                    {
                        feedbackText.text = "Correct! 5 is the first unsorted monster.";
                        HighlightTile(index, correctColor);
                        Invoke(nameof(GoToStep2), 0.7f);
                    }
                    else
                    {
                        ShowWrong("Not yet. First select the first unsorted monster: 5.");
                    }
                    break;

                case 2:
                    if (index == 1)
                    {
                        feedbackText.text = "Correct! 1 is the smallest monster.";
                        HighlightTile(index, correctColor);
                        Invoke(nameof(GoToStep3), 0.7f);
                    }
                    else
                    {
                        ShowWrong("Look for the smallest monster in the unsorted part. It is 1.");
                    }
                    break;

                case 4:
                    if (index == 1)
                    {
                        feedbackText.text = "Correct! This is the next first unsorted monster.";
                        HighlightTile(index, correctColor);
                        Invoke(nameof(GoToStep5), 0.7f);
                    }
                    else
                    {
                        ShowWrong("The next first unsorted monster is 5.");
                    }
                    break;

                case 5:
                    if (index == 3)
                    {
                        feedbackText.text = "Correct! 2 is the smallest in the remaining unsorted part.";
                        HighlightTile(index, correctColor);
                        Invoke(nameof(GoToStep6), 0.7f);
                    }
                    else
                    {
                        ShowWrong("From the remaining unsorted part, the smallest monster is 2.");
                    }
                    break;
            }
        }

        private void GoToStep2()
        {
            SetStep(2);
        }

        private void GoToStep3()
        {
            SetStep(3);
        }

        private void GoToStep4()
        {
            SetStep(4);
        }

        private void GoToStep5()
        {
            SetStep(5);
        }

        private void GoToStep6()
        {
            SetStep(6);
        }

        private void GoToStep7()
        {
            SetStep(7);
        }

        private void ShowWrong(string message)
        {
            if (feedbackText != null)
                feedbackText.text = message;

            ResetTileColors();

            switch (tutorialStep)
            {
                case 1:
                    HighlightTile(0, selectedColor);
                    break;

                case 2:
                    HighlightTile(0, selectedColor);
                    break;

                case 4:
                    HighlightTile(1, selectedColor);
                    break;

                case 5:
                    HighlightTile(1, selectedColor);
                    break;
            }
        }

        private void SwapValues(int indexA, int indexB)
        {
            int temp = values[indexA];
            values[indexA] = values[indexB];
            values[indexB] = temp;

            tileTexts[indexA].text = values[indexA].ToString();
            tileTexts[indexB].text = values[indexB].ToString();

            ResetTileColors();
            HighlightTile(indexA, correctColor);
            HighlightTile(indexB, correctColor);
        }

        private void ResetTileColors()
        {
            foreach (Button button in tileButtons)
            {
                if (button != null)
                    button.image.color = normalColor;
            }
        }

        private void HighlightTile(int index, Color color)
        {
            if (index < 0 || index >= tileButtons.Count)
                return;

            tileButtons[index].image.color = color;
        }

        public void StartDungeon()
        {
            SceneManager.LoadScene(firstSelectionLevel);
        }

        public void BackToAlgorithmSelect()
        {
            SceneManager.LoadScene(algorithmSelectScene);
        }
    }
}