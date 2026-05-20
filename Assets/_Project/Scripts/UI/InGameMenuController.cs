using UnityEngine;
using UnityEngine.SceneManagement;

namespace AlgoDungeon.UI
{
    public class InGameMenuController : MonoBehaviour
    {
        [Header("Scene Names")]
        [SerializeField] private string algorithmSelectScene = "AlgorithmSelect";
        [SerializeField] private string bubbleLevelSelectScene = "BubbleLevelSelect";
        [SerializeField] private string selectionLevelSelectScene = "SelectionLevelSelect";
        [SerializeField] private string insertionLevelSelectScene = "InsertionLevelSelect";

        [Header("Panels")]
        [SerializeField] private GameObject menuPanel;
        [SerializeField] private GameObject backConfirmPanel;

        [Header("Off Slash Visuals")]
        [SerializeField] private GameObject musicOffSlash;
        [SerializeField] private GameObject sfxOffSlash;

        private bool musicEnabled = true;
        private bool sfxEnabled = true;

        private void Start()
        {
            if (menuPanel != null)
                menuPanel.SetActive(false);

            if (backConfirmPanel != null)
                backConfirmPanel.SetActive(false);

            UpdateAudioVisuals();
        }

        public void ToggleMenu()
        {
            if (menuPanel == null)
                return;

            bool newState = !menuPanel.activeSelf;
            menuPanel.SetActive(newState);

            if (!newState && backConfirmPanel != null)
                backConfirmPanel.SetActive(false);
        }

        public void CloseMenu()
        {
            if (menuPanel != null)
                menuPanel.SetActive(false);

            if (backConfirmPanel != null)
                backConfirmPanel.SetActive(false);
        }

        public void ShowBackConfirmation()
        {
            if (backConfirmPanel != null)
                backConfirmPanel.SetActive(true);
        }

        public void CancelBack()
        {
            if (backConfirmPanel != null)
                backConfirmPanel.SetActive(false);
        }

        public void ConfirmBackToAlgorithmSelect()
        {
            string currentScene = SceneManager.GetActiveScene().name;

            if (currentScene.StartsWith("Bubble_Level_") || currentScene == "Bubble_Tutorial")
            {
                SceneManager.LoadScene(bubbleLevelSelectScene);
                return;
            }

            if (currentScene.StartsWith("Selection_Level_") || currentScene == "SelectionTutorial")
            {
                SceneManager.LoadScene(selectionLevelSelectScene);
                return;
            }

            if (currentScene.StartsWith("Insertion_Level_") || currentScene == "Insertion_Tutorial")
            {
                SceneManager.LoadScene(insertionLevelSelectScene);
                return;
            }

            SceneManager.LoadScene(algorithmSelectScene);
        }

        public void ToggleMusic()
        {
            musicEnabled = !musicEnabled;

            Debug.Log($"Music: {(musicEnabled ? "On" : "Off")}");

            UpdateAudioVisuals();

            // Later:
            // AudioManager.Instance.SetMusicEnabled(musicEnabled);
        }

        public void ToggleSfx()
        {
            sfxEnabled = !sfxEnabled;

            Debug.Log($"SFX: {(sfxEnabled ? "On" : "Off")}");

            UpdateAudioVisuals();

            // Later:
            // AudioManager.Instance.SetSfxEnabled(sfxEnabled);
        }

        private void UpdateAudioVisuals()
        {
            if (musicOffSlash != null)
                musicOffSlash.SetActive(!musicEnabled);

            if (sfxOffSlash != null)
                sfxOffSlash.SetActive(!sfxEnabled);
        }
    }
}