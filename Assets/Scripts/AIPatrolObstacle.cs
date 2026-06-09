using UnityEngine;

public class AIPatrolObstacle : MonoBehaviour
{
    [Header("Patrol Route")]
    [Tooltip("Drop your path markers here in the exact order the obstacle should follow them.")]
    [SerializeField] private Transform[] patrolRoute;

    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed = 3f;
    [SerializeField] private float arrivalThreshold = 0.1f;

    private int currentTargetIndex = 0;
    private bool isForwardPatrol = true; // Tracks whether walking forward (0->1->2) or backward (2->1->0)

    private void Start()
    {
        // Safety check to ensure the array has points assigned
        if (patrolRoute == null || patrolRoute.Length < 2)
        {
            Debug.LogError($"AIPatrolObstacle on {gameObject.name}: You must assign at least 2 points in the patrol route array!");
            return;
        }

        // Snap the obstacle directly to the first node at startup
        transform.position = patrolRoute[0].position;
        currentTargetIndex = 1; // Target the next node in sequence
    }

    private void Update()
    {
        if (patrolRoute == null || patrolRoute.Length == 0) return;

        MoveTowardsTarget();
        CheckWaypointArrival();
    }

    private void MoveTowardsTarget()
    {
        Transform targetNode = patrolRoute[currentTargetIndex];

        // Move step-by-step towards the active node coordinate
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetNode.position,
            movementSpeed * Time.deltaTime
        );
    }

    private void CheckWaypointArrival()
    {
        Transform targetNode = patrolRoute[currentTargetIndex];
        float distanceToTarget = Vector3.Distance(transform.position, targetNode.position);

        // Check if the obstacle has arrived at the current node corner
        if (distanceToTarget <= arrivalThreshold)
        {
            DetermineNextTarget();
        }
    }

    private void DetermineNextTarget()
    {
        if (isForwardPatrol)
        {
            currentTargetIndex++;
            // If we reached the final node in the array, turn back
            if (currentTargetIndex >= patrolRoute.Length)
            {
                isForwardPatrol = false;
                currentTargetIndex = patrolRoute.Length - 2; // Set target back to the previous node
            }
        }
        else
        {
            currentTargetIndex--;
            // If we reached the starting node, head forward again
            if (currentTargetIndex < 0)
            {
                isForwardPatrol = true;
                currentTargetIndex = 1; // Set target to the second node
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.ApplyObstaclePenalty(500f);
            }
        }
    }

    
}