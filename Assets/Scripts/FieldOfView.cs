using UnityEngine;
using System.Collections;

public class FieldOfView : MonoBehaviour
{
    [Header("FOV Settings")]
    public float radius = 10f;
    [Range(0, 360)]
    public float angle = 90f;

    [Header("References")]
    public Transform player;
    public LayerMask targetMask;        // For player
    public LayerMask obstructionMask;   // For walls, obstacles

    [Header("Debug Info")]
    public bool canSeePlayer;

    private void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            CheckFieldOfView();
        }
    }

    private void CheckFieldOfView()
    {
        canSeePlayer = false;

        if (player == null) return;

        // Check if player is within radius
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer > radius) return;

        // Check if player is within 90° cone
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        if (angleToPlayer < angle / 2)
        {
            // Check for obstruction
            if (!Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstructionMask))
            {
                canSeePlayer = true;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw vision radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);

        // Draw 90° cone lines
        Vector3 leftBoundary = Quaternion.Euler(0, -angle / 2, 0) * transform.forward * radius;
        Vector3 rightBoundary = Quaternion.Euler(0, angle / 2, 0) * transform.forward * radius;

        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, leftBoundary);
        Gizmos.DrawRay(transform.position, rightBoundary);

        // Draw line to player if visible
        if (canSeePlayer && player != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, player.position);
        }
    }
}
