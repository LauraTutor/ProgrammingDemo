using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class PatrolChaseAI : MonoBehaviour
{
    [Header("Patrol Settings")]
    public float wanderRadius = 10f;
    public float wanderTimer = 5f;

    [Header("Player Detection")]
    public Transform player;
    public float detectionRange = 10f;
    public float stopDistance = 1.5f;
    public float moveSpeed = 3f;
    public float fieldOfView = 90f;
    public LayerMask obstacleMask;

    private NavMeshAgent agent;
    private float timer;
    private bool chasingPlayer = false;

    void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.stoppingDistance = stopDistance;
            agent.speed = moveSpeed;
        }
        timer = wanderTimer;
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        bool canSeePlayer = CanSeePlayer(distanceToPlayer);

        // Check if player is within detection range and visible
        chasingPlayer = canSeePlayer && distanceToPlayer <= detectionRange;

        if (chasingPlayer)
        {
            FaceTarget(player.position);
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0; // prevent floating
            if (distanceToPlayer > stopDistance)
            {
                transform.position += direction * moveSpeed * Time.deltaTime;
            }

            // Face the player
            if (direction.magnitude > 0.1f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }
            else
            {
                // Patrol randomly
                timer += Time.deltaTime;
                if (timer >= wanderTimer)
                {
                    Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                    agent.SetDestination(newPos);
                    timer = 0;
                }
            }
        }
    }

    // Smoothly rotate to face the target
    private void FaceTarget(Vector3 targetPos)
    {
        Vector3 direction = (targetPos - transform.position).normalized;
        direction.y = 0;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    // Check if the player is in FOV and not blocked by obstacles
    private bool CanSeePlayer(float distanceToPlayer)
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);

        if (angle < fieldOfView / 2f)
        {
            if (!Physics.Raycast(transform.position + Vector3.up, directionToPlayer, distanceToPlayer, obstacleMask))
            {
                return true;
            }
        }
        return false;
    }

    // Get a random point for patrolling
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist + origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        return navHit.position;
    }

    // Debug visualization
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Vector3 leftLimit = Quaternion.Euler(0, -fieldOfView / 2, 0) * transform.forward;
        Vector3 rightLimit = Quaternion.Euler(0, fieldOfView / 2, 0) * transform.forward;
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, leftLimit * detectionRange);
        Gizmos.DrawRay(transform.position, rightLimit * detectionRange);
    }
}