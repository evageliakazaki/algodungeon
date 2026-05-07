using UnityEngine;

namespace AlgoDungeon.Core
{
    /// <summary>
    /// Singleton που διαχειρίζεται τη ροή του παιχνιδιού.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("State")]
        public bool IsGamePaused { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            GameEvents.OnRoomCompleted += HandleRoomCompleted;
        }

        private void OnDisable()
        {
            GameEvents.OnRoomCompleted -= HandleRoomCompleted;
        }

        private void HandleRoomCompleted(int stars)
        {
            Debug.Log($"[GameManager] Δωμάτιο ολοκληρώθηκε με {stars} αστέρια!");
            // TODO: εμφάνισε Win Screen, αποθήκευσε αποτέλεσμα κ.λπ.
        }

        public void PauseGame()
        {
            IsGamePaused = true;
            Time.timeScale = 0f;
        }

        public void ResumeGame()
        {
            IsGamePaused = false;
            Time.timeScale = 1f;
        }
    }
}