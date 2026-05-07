using UnityEngine;
using AlgoDungeon.Core;
using AlgoDungeon.Sorting;

namespace AlgoDungeon.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 5f;

        [Header("References")]
        [SerializeField] private Animator animator;

        private Rigidbody2D rb;
        private Vector2 inputDirection;
        private bool canMove = true;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (!canMove)
            {
                inputDirection = Vector2.zero;
                return;
            }

            // Διαβάζουμε input (παλιό Input system για απλότητα)
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            inputDirection = new Vector2(h, v).normalized;

            // Animation parameters (αν υπάρχει animator)
            if (animator != null)
            {
                animator.SetFloat("MoveX", h);
                animator.SetFloat("MoveY", v);
                animator.SetBool("IsMoving", inputDirection.sqrMagnitude > 0.01f);
            }

            // Interaction (για swap τεράτων)
            if (Input.GetKeyDown(KeyCode.E))
            {
                TryInteract();
            }
        }

        private void FixedUpdate()
        {
            rb.linearVelocity = inputDirection * moveSpeed;
            if (inputDirection.sqrMagnitude > 0.01f)
                GameEvents.PlayerMoved(rb.position);
        }

        private void TryInteract()
        {
            // Έλεγχος αν υπάρχει MonsterTile σε απόσταση 1 unit
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 1.2f);
            foreach (var hit in hits)
            {
                if (hit.TryGetComponent<MonsterTile>(out var tile))
                {
                    tile.OnInteract();
                    break;
                }
            }
        }

        public void SetMovementEnabled(bool enabled) => canMove = enabled;
    }
}

