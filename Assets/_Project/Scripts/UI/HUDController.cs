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
        [SerializeField] private string mainMenuSceneName = "MainMenu";
        [SerializeField] private string algorithmSelectSceneName = "AlgorithmSelect";

        private int comparisons;
        private int swaps;
        private bool resultShown;

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
            resultShown = false;

            if (winPanel != null)
                winPanel.SetActive(false);

            if (mainMenuButton != null)
            {
                mainMenuButton.gameObject.SetActive(false);
                mainMenuButton.onClick.RemoveAllListeners();
            }

            if (resultButton != null)
            {
                resultButton.gameObject.SetActive(true);
                resultButton.onClick.RemoveAllListeners();
            }

            if (resultButtonText != null)
                resultButtonText.text = "";

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
            if (resultShown)
                return;

            resultShown = true;

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
                : $"[HUD] Victory! {stars} stars");
        }

        private void ShowVictory(int stars)
        {
            if (resultText != null)
                resultText.text = "Dungeon Cleared!";

            SetStars(stars);

            string currentSceneName = SceneManager.GetActiveScene().name;
            GameProgressManager.CompleteLevel(currentSceneName, stars);

            if (IsLastLevel(currentSceneName))
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
            {
                mainMenuButton.gameObject.SetActive(false);
                mainMenuButton.onClick.RemoveAllListeners();
            }

            if (resultButton != null)
                resultButton.gameObject.SetActive(true);

            if (resultButtonText != null)
                resultButtonText.text = "Restart";

            ConfigurePrimaryButton(RestartLevel);
        }

        private void ShowNextLevelButton()
        {
            if (mainMenuButton != null)
            {
                mainMenuButton.gameObject.SetActive(false);
                mainMenuButton.onClick.RemoveAllListeners();
            }

            if (resultButton != null)
                resultButton.gameObject.SetActive(true);

            if (resultButtonText != null)
                resultButtonText.text = "Next";

            ConfigurePrimaryButton(LoadNextLevelByName);
        }

        private void ShowFinalLevelButtons()
        {
            if (resultButton != null)
                resultButton.gameObject.SetActive(true);

            if (resultButtonText != null)
                resultButtonText.text = "Play Again";

            ConfigurePrimaryButton(PlayAgainFromCurrentAlgorithm);

            if (mainMenuButton != null)
            {
                mainMenuButton.gameObject.SetActive(true);
                mainMenuButton.onClick.RemoveAllListeners();
                mainMenuButton.onClick.AddListener(GoToAlgorithmSelect);
            }

            if (mainMenuButtonText != null)
                mainMenuButtonText.text = "Algorithm Select";
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
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }

        private void LoadNextLevelByName()
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            string nextSceneName = GetNextLevelName(currentSceneName);

            if (string.IsNullOrWhiteSpace(nextSceneName))
            {
                GoToAlgorithmSelect();
                return;
            }

            if (Application.CanStreamedLevelBeLoaded(nextSceneName))
            {
                SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                Debug.LogWarning($"Δεν βρέθηκε η επόμενη σκηνή: {nextSceneName}. Πηγαίνω στο AlgorithmSelect.");
                GoToAlgorithmSelect();
            }
        }

        private void PlayAgainFromCurrentAlgorithm()
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            string firstSceneName = GetFirstLevelName(currentSceneName);

            if (Application.CanStreamedLevelBeLoaded(firstSceneName))
            {
                SceneManager.LoadScene(firstSceneName);
            }
            else
            {
                Debug.LogWarning($"Δεν βρέθηκε η πρώτη πίστα: {firstSceneName}. Κάνω restart.");
                RestartLevel();
            }
        }

        private void GoToAlgorithmSelect()
        {
            if (Application.CanStreamedLevelBeLoaded(algorithmSelectSceneName))
            {
                SceneManager.LoadScene(algorithmSelectSceneName);
                return;
            }

            if (Application.CanStreamedLevelBeLoaded(mainMenuSceneName))
            {
                SceneManager.LoadScene(mainMenuSceneName);
                return;
            }

            RestartLevel();
        }

        private bool IsLastLevel(string sceneName)
        {
            return sceneName.EndsWith("_Level_04");
        }

        private string GetNextLevelName(string currentSceneName)
        {
            const string marker = "_Level_";

            int markerIndex = currentSceneName.LastIndexOf(marker);

            if (markerIndex < 0)
                return null;

            string prefix = currentSceneName.Substring(0, markerIndex);
            string numberText = currentSceneName.Substring(markerIndex + marker.Length);

            if (!int.TryParse(numberText, out int currentNumber))
                return null;

            int nextNumber = currentNumber + 1;

            if (nextNumber > 4)
                return null;

            return $"{prefix}_Level_{nextNumber:00}";
        }

        private string GetFirstLevelName(string currentSceneName)
        {
            const string marker = "_Level_";

            int markerIndex = currentSceneName.LastIndexOf(marker);

            if (markerIndex < 0)
                return currentSceneName;

            string prefix = currentSceneName.Substring(0, markerIndex);

            return $"{prefix}_Level_01";
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