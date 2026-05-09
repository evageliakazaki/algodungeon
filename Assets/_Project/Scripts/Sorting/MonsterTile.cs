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
        [SerializeField] private Color selectedColor  = Color.yellow;
        [SerializeField] private Color comparingColor = new Color(1f, 0.6f, 0f);
        [SerializeField] private Color sortedColor    = Color.green;

        private TileState state = TileState.Idle;
        private ArrayManager arrayManager;
        private Color baseColor;

        public void Initialize(int value, int index, ArrayManager manager)
        {
            Value = value;
            CurrentIndex = index;
            arrayManager = manager;

            baseColor = GetColorByValue(value);

            if (valueText != null)
                valueText.text = value.ToString();

            SetState(TileState.Idle);
        }

        public void SetState(TileState newState)
        {
            state = newState;

            if (spriteRenderer == null)
                return;

            spriteRenderer.color = newState switch
            {
                TileState.Selected  => selectedColor,
                TileState.Comparing => comparingColor,
                TileState.Sorted    => sortedColor,
                _                   => baseColor
            };

            if (newState == TileState.Sorted)
                GameEvents.TileMarkedSorted(CurrentIndex);
        }

        public TileState GetState()
        {
            return state;
        }

        public void OnInteract()
        {
            arrayManager?.OnTileInteracted(this);
        }

        public IEnumerator MoveTo(Vector3 targetPosition, float duration)
        {
            Vector3 start = transform.position;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.SmoothStep(0, 1, elapsed / duration);
                transform.position = Vector3.Lerp(start, targetPosition, t);
                yield return null;
            }

            transform.position = targetPosition;
        }

        private Color GetColorByValue(int value)
        {
            return value switch
            {
                1 => new Color(0.4f, 1f, 0.5f),    // πράσινο
                2 => new Color(0.3f, 0.8f, 1f),    // μπλε
                4 => new Color(1f, 0.45f, 0.45f),  // κόκκινο
                5 => new Color(1f, 0.8f, 0.25f),   // κίτρινο
                8 => new Color(0.8f, 0.45f, 1f),   // μωβ
                _ => Color.white
            };
        }
    }
}