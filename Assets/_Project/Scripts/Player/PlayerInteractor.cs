using UnityEngine;
using AlgoDungeon.Sorting;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private float interactRange = 10f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("ΠΑΤΗΘΗΚΕ ΤΟ E");
            TryInteract();
        }
    }

    private void TryInteract()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactRange);

        Debug.Log("Βρέθηκαν colliders: " + hits.Length);

        MonsterTile closestTile = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider2D hit in hits)
        {
            Debug.Log("Collider: " + hit.name);

            MonsterTile tile = hit.GetComponent<MonsterTile>();

            if (tile == null)
                continue;

            float distance = Vector2.Distance(transform.position, tile.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTile = tile;
            }
        }

        if (closestTile == null)
        {
            Debug.Log("Δεν βρέθηκε MonsterTile κοντά.");
            return;
        }

        Debug.Log("Interact με MonsterTile value: " + closestTile.Value);
        closestTile.OnInteract();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}