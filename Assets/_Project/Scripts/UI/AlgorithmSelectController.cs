using UnityEngine;
using UnityEngine.SceneManagement;

namespace AlgoDungeon.UI
{
    public class AlgorithmSelectController : MonoBehaviour
    {
        [Header("Scene Names")]
        [SerializeField] private string mainMenuScene = "MainMenu";
        [SerializeField] private string selectionTutorialScene = "SelectionTutorial";
        [SerializeField] private string bubbleLevelSelectScene = "BubbleLevelSelect";

        public void PlaySelectionSort()
        {
            SceneManager.LoadScene(selectionTutorialScene);
        }

        public void PlayBubbleSort()
        {
            SceneManager.LoadScene(bubbleLevelSelectScene);
        }

        public void BackToMainMenu()
        {
            SceneManager.LoadScene(mainMenuScene);
        }
    }
}