using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;

public class MainMenuController : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject mainButtonsPanel;
    [SerializeField] private GameObject leaderboardPanel;

    [Header("Leaderboard Text Slots (Assign 5 Slots)")]
    [SerializeField] private TextMeshProUGUI[] highPercentDisplayTexts;

    // These data classes match the exact names used in your LeaderboardManager script
    [System.Serializable]
    public class ScoreEntry
    {
        public string playerName;
        public int score;
    }

    [System.Serializable]
    private class LeaderboardData
    {
        public List<ScoreEntry> highScores = new List<ScoreEntry>();
    }

    private const string SavedDataKey = "LocalLeaderboardData";

    private void Start()
    {
        Time.timeScale = 1f;

        if (mainButtonsPanel != null) mainButtonsPanel.SetActive(true);
        if (leaderboardPanel != null) leaderboardPanel.SetActive(false);

        // Load and format text displays immediately when the menu starts up
        RenderLeaderboardUI();
    }

    public void ClickPlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenLeaderboard()
    {
        mainButtonsPanel.SetActive(false);
        leaderboardPanel.SetActive(true);
        RenderLeaderboardUI(); // Refresh text lists upon opening panel layer
    }

    public void CloseLeaderboard()
    {
        mainButtonsPanel.SetActive(true);
        leaderboardPanel.SetActive(false);
    }

    /// <summary>
    /// Reads cross-scene saved profile data and populates text display matrices.
    /// </summary>
    private void RenderLeaderboardUI()
    {
        // 1. Safety verification check
        if (highPercentDisplayTexts == null || highPercentDisplayTexts.Length != 5)
        {
            Debug.LogWarning("MainMenuController: Please link exactly 5 text elements in the inspector slot array!");
            return;
        }

        // 2. Fetch data out of local registry storage
        if (PlayerPrefs.HasKey(SavedDataKey))
        {
            string jsonText = PlayerPrefs.GetString(SavedDataKey);
            LeaderboardData data = JsonUtility.FromJson<LeaderboardData>(jsonText);

            // 3. Loop through available records and draw them onto screen spaces
            for (int i = 0; i < highPercentDisplayTexts.Length; i++)
            {
                if (i < data.highScores.Count)
                {
                    highPercentDisplayTexts[i].text = $"{i + 1}. {data.highScores[i].playerName} - {data.highScores[i].score}";
                }
                else
                {
                    // Fallback formatting for unassigned ranking slots
                    highPercentDisplayTexts[i].text = $"{i + 1}. --- EMPTY ---";
                }
            }
        }
    }
}