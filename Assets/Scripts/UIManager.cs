using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("In-Game UI Overlays")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;

    [Header("Win Screen Displays")]
    [SerializeField] private TextMeshProUGUI txtTime;
    [SerializeField] private TextMeshProUGUI txtPenalties;
    [SerializeField] private TextMeshProUGUI txtFinalScore;
    [SerializeField] private TMP_InputField nameInputField;

    private bool isPaused = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // Make sure game physics are moving when entering the maze scene
        Time.timeScale = 1f;

        pausePanel.SetActive(false);
        winPanel.SetActive(false);
        losePanel.SetActive(false);
    }

    private void Update()
    {
        // Pressing Escape or Android Back button triggers the pause menu[span_6](start_span)[span_6](end_span)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (winPanel.activeSelf || losePanel.activeSelf) return;

            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; // Freeze physics calculations[span_7](start_span)[span_7](end_span)
        pausePanel.SetActive(true);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // Unfreeze game updates
        pausePanel.SetActive(false);
    }

    public void ShowWinScreen()
    {
        Time.timeScale = 0f;
        winPanel.SetActive(true);

        // Populate metrics derived from the active game score manager instance[span_8](start_span)[span_8](end_span)
        if (ScoreManager.Instance != null)
        {
            txtTime.text = $"Time: {ScoreManager.Instance.CompletionTime:F2}s";
            txtPenalties.text = $"Penalties: {ScoreManager.Instance.PenaltyCount}";
            txtFinalScore.text = $"Final Score: {Mathf.RoundToInt(ScoreManager.Instance.CurrentScore)}";
        }
    }
        public void ClickSubmitHighScore()
    {
        string username = nameInputField.text;
        // Default to standard substitute name if they leave it empty
        if (string.IsNullOrEmpty(username)) username = "SHINIGAMI";

        int scoreValue = Mathf.RoundToInt(ScoreManager.Instance.CurrentScore);

        // Submit the data to be sorted and serialized locally
        if (LeaderboardManager.Instance != null)
        {
            LeaderboardManager.Instance.SubmitScore(username, scoreValue);
        }

        // Disable the input field after clicking so they don't submit twice
        nameInputField.interactable = false;
    }


    public void ShowLoseScreen()
    {
        Time.timeScale = 0f;
        losePanel.SetActive(true);
    }

    /// <summary>
    /// Restarts the active maze scene layout cleanly[span_9](start_span)[span_9](end_span).
    /// </summary>
    public void RestartLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Exits the gameplay grid entirely and safely returns to the main menu scene[span_10](start_span)[span_10](end_span).
    /// </summary>
    public void ExitToMainMenu()
    {
        SceneManager.LoadScene(0); // Load Scene Index 0 (MainMenu)
    }
}