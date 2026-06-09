using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool isTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object passing through is the Player ball
        // Make sure your Sphere has the "Player" Tag assigned in the Inspector!
        if (other.CompareTag("Player") && !isTriggered)
        {
            isTriggered = true; // Prevents re-triggering the same checkpoint multiple times

            // Tell the GameManager to update the active respawn state coordinates
            // We save the position slightly higher (Y + 0.5) so the ball drops in nicely on respawn
            Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
            GameManager.Instance.UpdateCheckpoint(spawnPosition);
        }
    }
}