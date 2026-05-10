using UnityEngine;
using UnityEngine.SceneManagement;

namespace AlgoDungeon.UI
{
    public class MainMenuController : MonoBehaviour
    {
        [Header("Scene Names")]
        [SerializeField] private string algorithmSelectScene = "AlgorithmSelect";

        [Header("Panels")]
        [SerializeField] private GameObject quitConfirmPanel;

        private void Start()
        {
            if (quitConfirmPanel != null)
                quitConfirmPanel.SetActive(false);
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
    }
}