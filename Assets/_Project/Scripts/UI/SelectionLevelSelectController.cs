using UnityEngine;
using UnityEngine.SceneManagement;
using AlgoDungeon.Core;

namespace AlgoDungeon.UI
{
    public class SelectionLevelSelectController : MonoBehaviour
    {
        [Header("Back Scene")]
        [SerializeField] private string algorithmSelectScene = "AlgorithmSelect";

        [Header("Cards")]
        [SerializeField] private LevelCardUI tutorialCard;
        [SerializeField] private LevelCardUI level1Card;
        [SerializeField] private LevelCardUI level2Card;
        [SerializeField] private LevelCardUI level3Card;
        [SerializeField] private LevelCardUI level4Card;

        private const string TutorialScene = "SelectionTutorial";
        private const string Level1Scene = "Selection_Level_01";
        private const string Level2Scene = "Selection_Level_02";
        private const string Level3Scene = "Selection_Level_03";
        private const string Level4Scene = "Selection_Level_04";

        private void Start()
        {
            // Always make the first selection level available.
            GameProgressManager.UnlockLevel(Level1Scene);

            RefreshCards();
        }

        private void RefreshCards()
        {
            if (tutorialCard != null)
            {
                tutorialCard.Setup(
                    "TUTORIAL",
                    "Learn the rules",
                    TutorialScene,
                    true,
                    0,
                    true
                );
            }

            if (level1Card != null)
            {
                level1Card.Setup(
                    "LEVEL 1",
                    "Easy",
                    Level1Scene,
                    true,
                    GameProgressManager.GetStars(Level1Scene)
                );
            }

            if (level2Card != null)
            {
                level2Card.Setup(
                    "LEVEL 2",
                    "Medium",
                    Level2Scene,
                    GameProgressManager.IsUnlocked(Level2Scene),
                    GameProgressManager.GetStars(Level2Scene)
                );
            }

            if (level3Card != null)
            {
                level3Card.Setup(
                    "LEVEL 3",
                    "Hard",
                    Level3Scene,
                    GameProgressManager.IsUnlocked(Level3Scene),
                    GameProgressManager.GetStars(Level3Scene)
                );
            }

            if (level4Card != null)
            {
                level4Card.Setup(
                    "LEVEL 4",
                    "Master",
                    Level4Scene,
                    GameProgressManager.IsUnlocked(Level4Scene),
                    GameProgressManager.GetStars(Level4Scene)
                );
            }
        }

        public void BackToAlgorithmSelect()
        {
            SceneManager.LoadScene(algorithmSelectScene);
        }
    }
}