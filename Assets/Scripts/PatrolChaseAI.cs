using UnityEngine;
using UnityEngine.AI;

public class PatrolChaseAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;

    // Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange = 10f;

    // Vision
    public float sightRange = 10f;
    public float visionAngle = 90f;
    private bool playerInSight;

    private void Awake()
    {
        // Find player by tag
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
        else
            Debug.LogError("Player tag not found in the scene!");

        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        CheckPlayerInSight();

        if (playerInSight)
            ChasePlayer();
        else
            Patroling();

        FaceMovementDirection();
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        // Pick a random point within walkPointRange
        Vector3 randomDirection = Random.insideUnitSphere * walkPointRange;
        randomDirection += transform.position;

        // Find nearest point on NavMesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, walkPointRange, NavMesh.AllAreas))
        {
            walkPoint = hit.position;
            walkPointSet = true;
        }
    }

    private void CheckPlayerInSight()
    {
        if (player == null)
        {
            playerInSight = false;
            return;
        }

        Vector3 directionToPlayer = player.position - transform.position;
        float distance = directionToPlayer.magnitude;

        if (distance <= sightRange)
        {
            float angle = Vector3.Angle(transform.forward, directionToPlayer);
            if (angle <= visionAngle / 2)
            {
                // Optional: add raycast to check obstacles
                playerInSight = true;
                return;
            }
        }

        playerInSight = false;
    }

    private void ChasePlayer()
    {
        if (player != null)
            agent.SetDestination(player.position);
    }

    private void FaceMovementDirection()
    {
        if (agent.velocity.sqrMagnitude > 0.1f)
        {
            Vector3 lookDirection = agent.velocity.normalized;
            lookDirection.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), Time.deltaTime * 5f);
        }
    }
}
