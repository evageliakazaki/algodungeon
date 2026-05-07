using UnityEngine;
using TMPro;
using AlgoDungeon.Core;

namespace AlgoDungeon.UI
{
    /// <summary>
    /// Ελέγχει το HUD (Heads-Up Display) του παιχνιδιού:
    /// εμφανίζει comparisons, swaps, και αστέρια.
    /// </summary>
    public class HUDController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TextMeshProUGUI comparisonsText;
        [SerializeField] private TextMeshProUGUI swapsText;
        [SerializeField] private TextMeshProUGUI algorithmNameText;
        [SerializeField] private GameObject winPanel;

        private int comparisons;
        private int swaps;

        private void OnEnable()
        {
            GameEvents.OnTilesCompared    += HandleCompared;
            GameEvents.OnTilesSwapped     += HandleSwapped;
            GameEvents.OnRoomCompleted    += HandleRoomCompleted;
            GameEvents.OnAlgorithmStarted += HandleAlgorithmStarted;
        }

        private void OnDisable()
        {
            GameEvents.OnTilesCompared    -= HandleCompared;
            GameEvents.OnTilesSwapped     -= HandleSwapped;
            GameEvents.OnRoomCompleted    -= HandleRoomCompleted;
            GameEvents.OnAlgorithmStarted -= HandleAlgorithmStarted;
        }

        private void Start()
        {
            comparisons = 0;
            swaps       = 0;
            if (winPanel) winPanel.SetActive(false);
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

        private void HandleRoomCompleted(int stars)
        {
            if (winPanel) winPanel.SetActive(true);
            Debug.Log($"[HUD] Νίκη! {stars} ⭐");
        }

        private void HandleAlgorithmStarted(string name)
        {
            if (algorithmNameText) algorithmNameText.text = name;
        }

        private void UpdateUI()
        {
            if (comparisonsText) comparisonsText.text = $"Συγκρίσεις: {comparisons}";
            if (swapsText)       swapsText.text       = $"Ανταλλαγές: {swaps}";
        }
    }
}
