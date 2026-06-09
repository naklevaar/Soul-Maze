using UnityEngine;

public class FinishZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Finish Zone: Spirit Orb reached the end!");

            // Stop the game timer
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.StopTracking();
            }

            // Tell the UI Manager to show the Win Screen
            if (UIManager.Instance != null)
            {
                UIManager.Instance.ShowWinScreen();
            }
        }
    }
}