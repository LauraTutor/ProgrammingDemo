using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class PatrollingAI : MonoBehaviour
{
    private NavMeshAgent nav;

    [Header("AI Settings")]
    public float patrolRadius = 15f;
    public float startWaitTime = 1f;

    [Header("Vision Settings")]
    public float viewRadius = 10f;
    [Range(0, 360)]
    public float viewAngle = 90f;

    private float waitTime;
    private Vector3 randomPatrolPoint;

    private Transform player;
    private bool canSeePlayer;

    void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        nav.enabled = true;
    }

    void Start()
    {
        waitTime = startWaitTime;
        randomPatrolPoint = GetRandomPointOnNavMesh(transform.position, patrolRadius);
        nav.SetDestination(randomPatrolPoint);

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        StartCoroutine(FOVRoutine());
    }

    void Update()
    {
        if (canSeePlayer && player != null)
        {
            // Chase the player
            nav.SetDestination(player.position);
        }
        else
        {
            // Patrol behavior
            if (!nav.pathPending && nav.remainingDistance <= nav.stoppingDistance)
            {
                if (waitTime <= 0)
                {
                    randomPatrolPoint = GetRandomPointOnNavMesh(transform.position, patrolRadius);
                    nav.SetDestination(randomPatrolPoint);
                    waitTime = startWaitTime;
                }
                else
                {
                    waitTime -= Time.deltaTime;
                }
            }
        }
    }

    Vector3 GetRandomPointOnNavMesh(Vector3 center, float radius)
    {
        Vector3 randomPos = center + Random.insideUnitSphere * radius;
        NavMeshHit hit;

        if (NavMesh.SamplePosition(randomPos, out hit, radius, NavMesh.AllAreas))
            return hit.position;

        // Retry if not found
        return GetRandomPointOnNavMesh(center, radius);
    }

    IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    void FieldOfViewCheck()
    {
        if (player == null) return;

        Vector3 rayOrigin = transform.position + Vector3.up * 1.5f;
        Vector3 targetPoint = player.position + Vector3.up;
        Vector3 directionToPlayer = (targetPoint - rayOrigin).normalized;

        float distanceToPlayer = Vector3.Distance(rayOrigin, targetPoint);

        // Draw debug line (yellow = checking)
        Debug.DrawLine(rayOrigin, targetPoint, Color.yellow);

        if (distanceToPlayer <= viewRadius)
        {
            float angle = Vector3.Angle(transform.forward, directionToPlayer);
            if (angle <= viewAngle / 2f)
            {
                // Only consider an obstacle if ray hits something *other than player*
                if (Physics.Raycast(rayOrigin, directionToPlayer, out RaycastHit hit, distanceToPlayer))
                {
                    if (hit.transform.CompareTag("Player"))
                    {
                        canSeePlayer = true;
                        Debug.DrawLine(rayOrigin, targetPoint, Color.red); // Red = detected
                        return;
                    }
                }
            }
        }

        canSeePlayer = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        Vector3 angleA = DirectionFromAngle(-viewAngle / 2, false);
        Vector3 angleB = DirectionFromAngle(viewAngle / 2, false);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + angleA * viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + angleB * viewRadius);

        if (canSeePlayer && player != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, player.position);
        }
    }

    Vector3 DirectionFromAngle(float angleInDegrees, bool global)
    {
        if (!global)
            angleInDegrees += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
