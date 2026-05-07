using UnityEngine;
using TMPro;
using System.Collections;
using AlgoDungeon.Core;

namespace AlgoDungeon.Sorting
{
    public enum TileState { Idle, Selected, Comparing, Sorted }

    public class MonsterTile : MonoBehaviour
    {
        [Header("Data")]
        public int Value { get; private set; }
        public int CurrentIndex { get; set; }

        [Header("References")]
        [SerializeField] private TextMeshPro valueText;
        [SerializeField] private SpriteRenderer spriteRenderer;

        [Header("State Colors")]
        [SerializeField] private Color idleColor      = Color.white;
        [SerializeField] private Color selectedColor  = Color.yellow;
        [SerializeField] private Color comparingColor = new Color(1f, 0.6f, 0f);
        [SerializeField] private Color sortedColor    = Color.green;

        private TileState state = TileState.Idle;
        private ArrayManager arrayManager;

        public void Initialize(int value, int index, ArrayManager manager)
        {
            Value        = value;
            CurrentIndex = index;
            arrayManager = manager;
            valueText.text = value.ToString();
            SetState(TileState.Idle);
        }

        public void SetState(TileState newState)
        {
            state = newState;
            spriteRenderer.color = newState switch
            {
                TileState.Selected  => selectedColor,
                TileState.Comparing => comparingColor,
                TileState.Sorted    => sortedColor,
                _                   => idleColor
            };

            // FIX: Στέλνουμε το event όταν ένα tile γίνεται Sorted,
            // ώστε το HUD / ScoreTracker / άλλα systems να ενημερώνονται.
            if (newState == TileState.Sorted)
                GameEvents.TileMarkedSorted(CurrentIndex);
        }

        public TileState GetState() => state;

        // Καλείται όταν ο παίκτης πατήσει E δίπλα στο τέρας
        public void OnInteract()
        {
            arrayManager?.OnTileInteracted(this);
        }

        // Smooth animation κίνησης σε νέα θέση
        public IEnumerator MoveTo(Vector3 targetPosition, float duration)
        {
            Vector3 start   = transform.position;
            float   elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.SmoothStep(0, 1, elapsed / duration);
                transform.position = Vector3.Lerp(start, targetPosition, t);
                yield return null;
            }
            transform.position = targetPosition;
        }
    }
}