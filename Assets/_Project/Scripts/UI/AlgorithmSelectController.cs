using UnityEngine;
using UnityEngine.SceneManagement;

namespace AlgoDungeon.UI
{
    public class AlgorithmSelectController : MonoBehaviour
    {
        [Header("Scene Names")]
        [SerializeField] private string mainMenuScene = "MainMenu";
        [SerializeField] private string selectionFirstLevel = "Selection_Level_01";
        [SerializeField] private string bubbleFirstLevel = "Bubble_Level_01";

        public void PlaySelectionSort()
        {
            SceneManager.LoadScene(selectionFirstLevel);
        }

        public void PlayBubbleSort()
        {
            SceneManager.LoadScene(bubbleFirstLevel);
        }

        public void BackToMainMenu()
        {
            SceneManager.LoadScene(mainMenuScene);
        }
    }
}