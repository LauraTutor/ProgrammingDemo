using UnityEngine;
using UnityEngine.AI;

public class EnemyAILS : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    private NavMeshAgent agent;
    private Animator animator;

    [Header("AI Settings")]
    public float detectionRange = 10f;
    public float patrolRadius = 15f;
    public float patrolWaitTime = 3f;

    private Vector3 patrolDestination;
    private bool isPatrolling = true;
    private float patrolTimer = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        SetNewPatrolPoint();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            isPatrolling = false;
            agent.SetDestination(player.position);
        }
        else
        {
            if (!isPatrolling)
            {
                isPatrolling = true;
                SetNewPatrolPoint();
            }
            Patrol();
        }

        bool moving = agent.velocity.magnitude > 0.2f;
        animator.SetBool("isMoving", moving);
    }

    void Patrol()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            patrolTimer += Time.deltaTime;
            if (patrolTimer >= patrolWaitTime)
            {
                SetNewPatrolPoint();
                patrolTimer = 0f;
            }
        }
    }

    void SetNewPatrolPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius + transform.position;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, patrolRadius, NavMesh.AllAreas))
        {
            patrolDestination = hit.position;
            agent.SetDestination(patrolDestination);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, patrolRadius);
    }
}
