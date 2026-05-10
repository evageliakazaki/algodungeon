using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using AlgoDungeon.Core;

namespace AlgoDungeon.UI
{
    public class HUDController : MonoBehaviour
    {
        [Header("HUD References")]
        [SerializeField] private TextMeshProUGUI comparisonsText;
        [SerializeField] private TextMeshProUGUI swapsText;
        [SerializeField] private TextMeshProUGUI algorithmNameText;

        [Header("Result Panel")]
        [SerializeField] private GameObject winPanel;
        [SerializeField] private TextMeshProUGUI resultText;
        [SerializeField] private Image[] starImages;

        [Header("Primary Result Button")]
        [SerializeField] private Button resultButton;
        [SerializeField] private TextMeshProUGUI resultButtonText;

        [Header("Final Level Button")]
        [SerializeField] private Button mainMenuButton;
        [SerializeField] private TextMeshProUGUI mainMenuButtonText;

        [Header("Level Flow")]
        [SerializeField] private string firstSelectionLevelName = "Selection_Level_01";
        [SerializeField] private string mainMenuSceneName = "MainMenu";

        private int comparisons;
        private int swaps;

        private void OnEnable()
        {
            GameEvents.OnTilesCompared += HandleCompared;
            GameEvents.OnTilesSwapped += HandleSwapped;
            GameEvents.OnRoomCompleted += HandleRoomCompleted;
            GameEvents.OnAlgorithmStarted += HandleAlgorithmStarted;
        }

        private void OnDisable()
        {
            GameEvents.OnTilesCompared -= HandleCompared;
            GameEvents.OnTilesSwapped -= HandleSwapped;
            GameEvents.OnRoomCompleted -= HandleRoomCompleted;
            GameEvents.OnAlgorithmStarted -= HandleAlgorithmStarted;
        }

        private void Start()
        {
            comparisons = 0;
            swaps = 0;

            if (winPanel != null)
                winPanel.SetActive(false);

            if (mainMenuButton != null)
                mainMenuButton.gameObject.SetActive(false);

            SetStars(0);
            UpdateUI();
        }

        private void HandleCompared(int a, int b)
        {
            comparisons++;
            UpdateUI();
        }

        private void HandleSwapped(int a, int b)
        {
            swaps++;
            UpdateUI();
        }

        private void HandleAlgorithmStarted(string name)
        {
            if (algorithmNameText != null)
                algorithmNameText.text = name;
        }

        private void HandleRoomCompleted(int stars)
        {
            if (winPanel != null)
                winPanel.SetActive(true);

            if (stars <= 0)
            {
                ShowGameOver();
            }
            else
            {
                ShowVictory(stars);
            }

            Debug.Log(stars <= 0
                ? "[HUD] Game Over!"
                : $"[HUD] Νίκη! {stars} stars");
        }

        private void ShowVictory(int stars)
        {
            if (resultText != null)
                resultText.text = "Dungeon Cleared!";

            SetStars(stars);

            if (IsLastLevel())
            {
                ShowFinalLevelButtons();
            }
            else
            {
                ShowNextLevelButton();
            }
        }

        private void ShowGameOver()
        {
            if (resultText != null)
                resultText.text = "Game Over!\nTry Again";

            SetStars(0);

            if (mainMenuButton != null)
                mainMenuButton.gameObject.SetActive(false);

            if (resultButtonText != null)
                resultButtonText.text = "Restart";

            ConfigurePrimaryButton(RestartLevel);
        }

        private void ShowNextLevelButton()
        {
            if (mainMenuButton != null)
                mainMenuButton.gameObject.SetActive(false);

            if (resultButtonText != null)
                resultButtonText.text = "Next";

            ConfigurePrimaryButton(LoadNextLevel);
        }

        private void ShowFinalLevelButtons()
        {
            if (resultButtonText != null)
                resultButtonText.text = "Play Again";

            ConfigurePrimaryButton(PlayAgainFromStart);

            if (mainMenuButton != null)
            {
                mainMenuButton.gameObject.SetActive(true);
                mainMenuButton.onClick.RemoveAllListeners();
                mainMenuButton.onClick.AddListener(GoToMainMenu);
            }

            if (mainMenuButtonText != null)
                mainMenuButtonText.text = "Main Menu";
        }

        private void ConfigurePrimaryButton(UnityEngine.Events.UnityAction action)
        {
            if (resultButton == null)
                return;

            resultButton.onClick.RemoveAllListeners();
            resultButton.onClick.AddListener(action);
        }

        private void RestartLevel()
        {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.buildIndex);
        }

        private void LoadNextLevel()
        {
            int currentIndex = SceneManager.GetActiveScene().buildIndex;
            int nextIndex = currentIndex + 1;

            if (nextIndex < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(nextIndex);
            }
            else
            {
                PlayAgainFromStart();
            }
        }

        private void PlayAgainFromStart()
        {
            if (Application.CanStreamedLevelBeLoaded(firstSelectionLevelName))
            {
                SceneManager.LoadScene(firstSelectionLevelName);
            }
            else
            {
                Debug.LogWarning($"Δεν βρέθηκε η σκηνή {firstSelectionLevelName}. Κάνω restart την τρέχουσα.");
                RestartLevel();
            }
        }

        private void GoToMainMenu()
        {
            if (Application.CanStreamedLevelBeLoaded(mainMenuSceneName))
            {
                SceneManager.LoadScene(mainMenuSceneName);
            }
            else
            {
                Debug.LogWarning($"Δεν υπάρχει ακόμα σκηνή {mainMenuSceneName}. Προσωρινά πάω στην πρώτη Selection πίστα.");
                PlayAgainFromStart();
            }
        }

        private bool IsLastLevel()
        {
            return SceneManager.GetActiveScene().name == "Selection_Level_04";
        }
        private void UpdateUI()
        {
            if (comparisonsText != null)
                comparisonsText.text = $"Συγκρίσεις: {comparisons}";

            if (swapsText != null)
                swapsText.text = $"Ανταλλαγές: {swaps}";
        }

        private void SetStars(int stars)
        {
            if (starImages == null)
                return;

            for (int i = 0; i < starImages.Length; i++)
            {
                if (starImages[i] != null)
                    starImages[i].gameObject.SetActive(i < stars);
            }
        }
    }
}