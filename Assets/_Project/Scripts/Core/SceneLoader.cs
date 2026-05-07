using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace AlgoDungeon.Core
{
    /// <summary>
    /// Utility για φόρτωση scenes με προαιρετικό fade-out.
    /// </summary>
    public class SceneLoader : MonoBehaviour
    {
        public static SceneLoader Instance { get; private set; }

        [SerializeField] private float fadeOutDuration = 0.5f;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>Φορτώνει scene με το όνομά της.</summary>
        public void LoadScene(string sceneName)
        {
            StartCoroutine(LoadRoutine(sceneName));
        }

        /// <summary>Φορτώνει scene με τον αριθμό της (Build Settings).</summary>
        public void LoadScene(int buildIndex)
        {
            StartCoroutine(LoadRoutine(buildIndex));
        }

        public void ReloadCurrentScene()
        {
            LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private IEnumerator LoadRoutine(string name)
        {
            yield return new WaitForSeconds(fadeOutDuration);
            SceneManager.LoadScene(name);
        }

        private IEnumerator LoadRoutine(int index)
        {
            yield return new WaitForSeconds(fadeOutDuration);
            SceneManager.LoadScene(index);
        }
    }
}
