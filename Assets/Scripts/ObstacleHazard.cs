using UnityEngine;

public class ObstacleHazard : MonoBehaviour
{
    [Header("Penalty Settings")]
    [Tooltip("How many points to deduct from the player's score upon collision.")]
    [SerializeField] private float scorePenalty = 500f;

    [Tooltip("Prevents multiple rapid accidental hits in a fraction of a second.")]
    [SerializeField] private float penaltyCooldown = 1f;

    private float nextPenaltyTime = 0f;

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the colliding object is the player sphere
        if (collision.gameObject.CompareTag("Player"))
        {
            EvaluatePenalty();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // In case your animated PressTrap or moving blade uses trigger colliders instead of solid ones
        if (other.CompareTag("Player"))
        {
            EvaluatePenalty();
        }
    }

    private void EvaluatePenalty()
    {
        // Enforce cooldown safety check
        if (Time.time >= nextPenaltyTime)
        {
            nextPenaltyTime = Time.time + penaltyCooldown;

            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.ApplyObstaclePenalty(scorePenalty);
            }
            else
            {
                Debug.LogWarning("ObstacleHazard: ScoreManager instance not found in scene!");
            }
        }
    }
}