using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [Header("Scoring Configuration")]
    [SerializeField] private float startingScore = 5000f;
    [SerializeField] private float timePenaltyRate = 10f; // Points lost per second

    [Header("Deafeat Condition")]
    [SerializeField] private int maxAllowedPenalties = 10;

    // Properties to satisfy assignment display requirements[span_6](start_span)[span_6](end_span)
    public float CompletionTime { get; private set; }
    public int PenaltyCount { get; private set; }
    public float CurrentScore { get; private set; }

    private bool isGameActive = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CurrentScore = startingScore;
        CompletionTime = 0f;
        PenaltyCount = 0;
        isGameActive = true;
    }

    private void Update()
    {
        if (!isGameActive) return;

        // 1. Track Completion Time[span_7](start_span)[span_7](end_span)
        CompletionTime += Time.deltaTime;

        // 2. Continuous time penalty (the longer you take, the lower the score)[span_8](start_span)[span_8](end_span)
        CurrentScore -= timePenaltyRate * Time.deltaTime;

        // Clamp score so it never drops below zero
        CurrentScore = Mathf.Max(CurrentScore, 0f);

        if (CurrentScore <= 0f)
        {
            TriggerScoreFailure();
        }
    }

    
    public void ApplyObstaclePenalty(float penaltyAmount)
    {
        PenaltyCount++;
        CurrentScore -= penaltyAmount;
        CurrentScore = Mathf.Max(CurrentScore, 0f);

        Debug.Log($"ScoreManager: Penalty Applied! Total Penalties: {PenaltyCount}/{maxAllowedPenalties} | Remaining Score: {CurrentScore}");

        // 1. Check if the player has exceeded the 10 penalty limit
        if (PenaltyCount >= maxAllowedPenalties)
        {
            TriggerScoreFailure();
            return; // Exit early to prevent the player from respawning since they lost
        }

        // 2. Check if the score itself hit zero
        if (CurrentScore <= 0f)
        {
            TriggerScoreFailure();
            return;
        }

        // Automatically respawn the player back to Ichigo's Shinigami Pass checkpoint if still alive
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RespawnPlayer();
        }
    }

    public void StopTracking()
    {
        isGameActive = false;
    }

    private void TriggerScoreFailure()
    {
        isGameActive = false;
        Debug.Log("ScoreManager: Defeat condition met. Triggering Lose Screen.");

        // Clear the placeholder and officially trigger the UI layer panel
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowLoseScreen();
        }
        else
        {
            Debug.LogError("ScoreManager: UIManager instance not found to show Lose Screen!");
        }
    }
}