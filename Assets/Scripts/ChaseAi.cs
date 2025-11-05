using UnityEngine;
using UnityEngine.AI;

public class ChaseAi : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 10f;
    public float moveSpeed = 3f;
    public float stopDistance = 1.5f;

    private bool chasing = false;

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // Detect player
        if (distance <= detectionRange)
        {
            chasing = true;
        }
        else if (distance > detectionRange + 2f)
        {
            chasing = false;
        }

        // Move towards player if chasing
        if (chasing)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0; // prevent floating
            if (distance > stopDistance)
            {
                transform.position += direction * moveSpeed * Time.deltaTime;
            }

            // Face the player
            if (direction.magnitude > 0.1f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}