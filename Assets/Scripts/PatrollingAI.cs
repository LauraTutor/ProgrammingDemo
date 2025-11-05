using UnityEngine;
using UnityEngine.AI;

public class PatrollingAI : MonoBehaviour
{
    [Header("Patrol Settings")]
    public float patrolRange = 10f;
    public float waitTime = 2f;

    [Header("Detection Settings")]
    public float detectionRange = 12f;
    public float fieldOfView = 90f; // degrees
    public float loseSightTime = 3f; // seconds before stopping chase after losing sight

    private NavMeshAgent agent;
    private Transform player;
    private Vector3 patrolPoint;
    private float waitTimer = 0f;
    private bool hasPatrolPoint = false;
    private float loseSightTimer = 0f;
    private bool isChasing = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
            Debug.LogError("No NavMeshAgent found on " + gameObject.name);

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }

    bool CanSeePlayer()
    {
        if (player == null) return false;

        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer > detectionRange) return false;

        // Check within FOV
        float angle = Vector3.Angle(transform.forward, directionToPlayer);
        if (angle > fieldOfView * 0.5f) return false;

        // Raycast to check line of sight
        if (Physics.Raycast(transform.position + Vector3.up, directionToPlayer.normalized, out RaycastHit hit, detectionRange))
        {
            if (hit.collider.CompareTag("Player"))
            {
                Debug.DrawLine(transform.position + Vector3.up, hit.point, Color.red);
                return true;
            }
        }
        return false;
    }

    void Update()
    {
        if (player == null) return;

        bool canSee = CanSeePlayer();

        if (canSee)
        {
            isChasing = true;
            loseSightTimer = 0f;
            agent.SetDestination(player.position);
        }
        else if (isChasing)
        {
            loseSightTimer += Time.deltaTime;
            if (loseSightTimer > loseSightTime)
            {
                isChasing = false;
                loseSightTimer = 0f;
            }
        }

        if (isChasing)
        {
            // keep chasing
            agent.SetDestination(player.position);
            return;
        }

        // === PATROLLING BEHAVIOR ===
        if (!hasPatrolPoint || (!agent.pathPending && agent.remainingDistance < 0.5f))
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitTime)
            {
                if (RandomPoint(transform.position, patrolRange, out patrolPoint))
                {
                    agent.SetDestination(patrolPoint);
                    Debug.DrawRay(patrolPoint, Vector3.up, Color.blue, 1.0f);
                    hasPatrolPoint = true;
                    waitTimer = 0f;
                }
            }
        }
    }
}
