using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton instance allows other scripts to easily talk to the GameManager
    public static GameManager Instance { get; private set; }

    [Header("Player Management")]
    [SerializeField] private GameObject playerGameObject;

    // Tracks the current state of where the player should restart
    private Vector3 activeCheckpointPosition;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (playerGameObject != null)
        {
            // Set the initial spawn point to wherever the ball starts the game
            activeCheckpointPosition = playerGameObject.transform.position;
        }
        else
        {
            Debug.LogError("GameManager: Player GameObject reference missing!");
        }
    }

    /// <summary>
    /// Updates the current active respawn state when a checkpoint is triggered.
    /// </summary>
    public void UpdateCheckpoint(Vector3 newCheckpointPosition)
    {
        activeCheckpointPosition = newCheckpointPosition;
        Debug.Log("GameManager: State Updated! Checkpoint saved at: " + newCheckpointPosition);

        // TODO: This is a perfect place to trigger a visual effect or sound later!
    }

    /// <summary>
    /// Resets the player's position back to the last saved checkpoint state.
    /// </summary>
    public void RespawnPlayer()
    {
        if (playerGameObject != null)
        {
            Rigidbody rb = playerGameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Stop all velocity/momentum so the ball doesn't keep rolling instantly upon respawning
                rb.linearVelocity = Vector3.zero; // Kept linearVelocity since you are on Unity 6
                rb.angularVelocity = Vector3.zero;
            }

            // Snap the player back to the saved state position
            playerGameObject.transform.position = activeCheckpointPosition;
        }
    }
}