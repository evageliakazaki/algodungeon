using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using AlgoDungeon.Core;

namespace AlgoDungeon.UI
{
    public class MainMenuController : MonoBehaviour
    {
        private const string MusicEnabledKey = "MusicEnabled";
        private const string SfxEnabledKey = "SfxEnabled";

        [Header("Scene Names")]
        [SerializeField] private string algorithmSelectScene = "AlgorithmSelect";

        [Header("Main Menu")]
        [SerializeField] private GameObject mainPanel;

        [Header("Quit Panel")]
        [SerializeField] private GameObject quitConfirmPanel;

        [Header("Settings Panel")]
        [SerializeField] private GameObject settingsPanel;
        [SerializeField] private GameObject resetConfirmPanel;

        [Header("Settings Buttons")]
        [SerializeField] private Button settingsGearButton;
        [SerializeField] private Button closeSettingsButton;
        [SerializeField] private Button musicButton;
        [SerializeField] private Button sfxButton;
        [SerializeField] private Button resetProgressButton;
        [SerializeField] private Button confirmResetButton;
        [SerializeField] private Button cancelResetButton;

        [Header("Settings Texts - Optional")]
        [SerializeField] private TextMeshProUGUI musicButtonText;
        [SerializeField] private TextMeshProUGUI sfxButtonText;

        [Header("Settings Visuals")]
        [SerializeField] private GameObject musicOffSlash;
        [SerializeField] private GameObject sfxOffSlash;

        [Header("Help Panel")]
        [SerializeField] private GameObject helpPanel;
        [SerializeField] private Button helpBookButton;
        [SerializeField] private Button closeHelpButton;

        private bool musicEnabled;
        private bool sfxEnabled;

        private void Start()
        {
            musicEnabled = PlayerPrefs.GetInt(MusicEnabledKey, 1) == 1;
            sfxEnabled = PlayerPrefs.GetInt(SfxEnabledKey, 1) == 1;

            ShowMainOnly();

            if (settingsGearButton != null)
                settingsGearButton.onClick.AddListener(OpenSettings);

            if (closeSettingsButton != null)
                closeSettingsButton.onClick.AddListener(CloseSettings);

            if (musicButton != null)
                musicButton.onClick.AddListener(ToggleMusic);

            if (sfxButton != null)
                sfxButton.onClick.AddListener(ToggleSfx);

            if (resetProgressButton != null)
                resetProgressButton.onClick.AddListener(OpenResetConfirmation);

            if (confirmResetButton != null)
                confirmResetButton.onClick.AddListener(ConfirmResetProgress);

            if (cancelResetButton != null)
                cancelResetButton.onClick.AddListener(HideResetConfirmation);

            if (helpBookButton != null)
                helpBookButton.onClick.AddListener(OpenHelp);

            if (closeHelpButton != null)
                closeHelpButton.onClick.AddListener(CloseHelp);

            RefreshSettingsVisuals();
            ApplyAudioSettings();
        }

        public void Play()
        {
            SceneManager.LoadScene(algorithmSelectScene);
        }

        public void ShowQuitConfirmation()
        {
            if (quitConfirmPanel != null)
                quitConfirmPanel.SetActive(true);
        }

        public void HideQuitConfirmation()
        {
            if (quitConfirmPanel != null)
                quitConfirmPanel.SetActive(false);
        }

        public void ConfirmQuitGame()
        {
            Debug.Log("Quit Game");

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private void OpenSettings()
        {
            if (mainPanel != null)
                mainPanel.SetActive(false);

            if (quitConfirmPanel != null)
                quitConfirmPanel.SetActive(false);

            if (helpPanel != null)
                helpPanel.SetActive(false);

            if (settingsPanel != null)
                settingsPanel.SetActive(true);

            if (resetConfirmPanel != null)
                resetConfirmPanel.SetActive(false);
        }

        private void CloseSettings()
        {
            ShowMainOnly();
        }

        private void OpenHelp()
        {
            if (mainPanel != null)
                mainPanel.SetActive(false);

            if (quitConfirmPanel != null)
                quitConfirmPanel.SetActive(false);

            if (settingsPanel != null)
                settingsPanel.SetActive(false);

            if (resetConfirmPanel != null)
                resetConfirmPanel.SetActive(false);

            if (helpPanel != null)
                helpPanel.SetActive(true);
        }

        private void CloseHelp()
        {
            ShowMainOnly();
        }

        private void ShowMainOnly()
        {
            if (mainPanel != null)
                mainPanel.SetActive(true);

            if (quitConfirmPanel != null)
                quitConfirmPanel.SetActive(false);

            if (settingsPanel != null)
                settingsPanel.SetActive(false);

            if (resetConfirmPanel != null)
                resetConfirmPanel.SetActive(false);

            if (helpPanel != null)
                helpPanel.SetActive(false);
        }

        private void ToggleMusic()
        {
            musicEnabled = !musicEnabled;

            PlayerPrefs.SetInt(MusicEnabledKey, musicEnabled ? 1 : 0);
            PlayerPrefs.Save();

            RefreshSettingsVisuals();
            ApplyAudioSettings();
        }

        private void ToggleSfx()
        {
            sfxEnabled = !sfxEnabled;

            PlayerPrefs.SetInt(SfxEnabledKey, sfxEnabled ? 1 : 0);
            PlayerPrefs.Save();

            RefreshSettingsVisuals();
            ApplyAudioSettings();
        }

        private void OpenResetConfirmation()
        {
            if (resetConfirmPanel != null)
                resetConfirmPanel.SetActive(true);
        }

        private void HideResetConfirmation()
        {
            if (resetConfirmPanel != null)
                resetConfirmPanel.SetActive(false);
        }

        private void ConfirmResetProgress()
        {
            GameProgressManager.ResetProgress();

            if (resetConfirmPanel != null)
                resetConfirmPanel.SetActive(false);

            Debug.Log("Progress reset from Main Menu.");
        }

        private void RefreshSettingsVisuals()
        {
            if (musicButtonText != null)
                musicButtonText.text = musicEnabled ? "Music: ON" : "Music: OFF";

            if (sfxButtonText != null)
                sfxButtonText.text = sfxEnabled ? "SFX: ON" : "SFX: OFF";

            if (musicOffSlash != null)
                musicOffSlash.SetActive(!musicEnabled);

            if (sfxOffSlash != null)
                sfxOffSlash.SetActive(!sfxEnabled);
        }

        private void ApplyAudioSettings()
        {
            AudioListener.volume = (musicEnabled || sfxEnabled) ? 1f : 0f;
        }

        public static bool IsMusicEnabled()
        {
            return PlayerPrefs.GetInt(MusicEnabledKey, 1) == 1;
        }

        public static bool IsSfxEnabled()
        {
            return PlayerPrefs.GetInt(SfxEnabledKey, 1) == 1;
        }
    }
}