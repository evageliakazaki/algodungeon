using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AlgoDungeon.UI
{
    public class LevelCardUI : MonoBehaviour
    {
        [Header("Card")]
        [SerializeField] private Button button;
        [SerializeField] private Image cardImage;

        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI levelNameText;
        [SerializeField] private TextMeshProUGUI difficultyText;
        [SerializeField] private TextMeshProUGUI starsText;

        [Header("Locked State")]
        [SerializeField] private GameObject lockOverlay;

        [Header("Colors")]
        [SerializeField] private Color unlockedColor = new Color(0.22f, 0.28f, 0.36f, 1f);
        [SerializeField] private Color lockedColor = new Color(0.10f, 0.11f, 0.13f, 1f);
        [SerializeField] private Color tutorialColor = new Color(0.75f, 0.47f, 0.12f, 1f);

        private string targetScene;
        private bool isUnlocked;

        public void Setup(
            string levelName,
            string difficulty,
            string sceneName,
            bool unlocked,
            int stars,
            bool isTutorial = false
        )
        {
            targetScene = sceneName;
            isUnlocked = unlocked;

            if (levelNameText != null)
                levelNameText.text = levelName;

            if (difficultyText != null)
                difficultyText.text = difficulty;

            if (starsText != null)
                starsText.text = isTutorial ? "START" : StarsToText(stars);

            if (lockOverlay != null)
                lockOverlay.SetActive(!isUnlocked);

            if (cardImage != null)
            {
                if (isTutorial)
                    cardImage.color = tutorialColor;
                else
                    cardImage.color = isUnlocked ? unlockedColor : lockedColor;
            }

            if (button != null)
            {
                button.interactable = isUnlocked;
                button.onClick.RemoveAllListeners();

                if (isUnlocked)
                    button.onClick.AddListener(LoadTargetScene);
            }
        }

        private string StarsToText(int stars)
        {
            if (stars <= 0)
                return "---";

            if (stars == 1)
                return "*--";

            if (stars == 2)
                return "**-";

            return "***";
        }

        private void LoadTargetScene()
        {
            if (!isUnlocked)
                return;

            if (string.IsNullOrWhiteSpace(targetScene))
                return;

            SceneManager.LoadScene(targetScene);
        }
    }
}