using UnityEngine;
using UnityEngine.SceneManagement;

namespace AlgoDungeon.UI
{
    public class AlgorithmSelectController : MonoBehaviour
    {
        [Header("Scene Names")]
        [SerializeField] private string mainMenuScene = "MainMenu";
        [SerializeField] private string selectionTutorialScene = "SelectionTutorial";
        [SerializeField] private string bubbleFirstLevel = "Bubble_Level_01";

        public void PlaySelectionSort()
        {
            SceneManager.LoadScene(selectionTutorialScene);
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