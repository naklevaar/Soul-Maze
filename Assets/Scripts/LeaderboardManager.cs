using UnityEngine;
using System.Collections.Generic;

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager Instance { get; private set; }

    // System.Serializable allows Unity's JsonUtility to convert this data into text
    [System.Serializable]
    public class ScoreEntry
    {
        public string playerName;
        public int score;

        public ScoreEntry(string name, int scoreValue)
        {
            playerName = name;
            score = scoreValue;
        }
    }

    // Wrapper class because Unity's JsonUtility cannot directly serialize a raw List array
    [System.Serializable]
    private class LeaderboardData
    {
        public List<ScoreEntry> highScores = new List<ScoreEntry>();
    }

    private const string SavedDataKey = "LocalLeaderboardData";
    private LeaderboardData currentLeaderboard = new LeaderboardData();

    private void Awake()
    {
        Instance = this;
        LoadLeaderboard();
    }

    /// <summary>
    /// Evaluates and inserts a new score into the local top 5 leaderboard list.
    /// </summary>
    public void SubmitScore(string name, int newScore)
    {
        // 1. Create a new data entry object
        ScoreEntry entry = new ScoreEntry(name, newScore);
        currentLeaderboard.highScores.Add(entry);

        // 2. Sort the data structure correctly (Highest score first)
        currentLeaderboard.highScores.Sort((x, y) => y.score.CompareTo(x.score));

        // 3. Constrain the data structure size to store only the Top 5 scores
        if (currentLeaderboard.highScores.Count > 5)
        {
            currentLeaderboard.highScores.RemoveAt(5); // Remove anything ranking 6th or lower
        }

        // 4. Persist the updated data structure state between game sessions
        SaveLeaderboard();
    }

    /// <summary>
    /// Retrieves the current stored array list of high scores.
    /// </summary>
    public List<ScoreEntry> GetTopScores()
    {
        return currentLeaderboard.highScores;
    }

    private void SaveLeaderboard()
    {
        // Serialize our object data structure into a JSON clean string text format
        string jsonText = JsonUtility.ToJson(currentLeaderboard);

        // Save the string locally on the device profile registry
        PlayerPrefs.SetString(SavedDataKey, jsonText);
        PlayerPrefs.Save();
        Debug.Log("LeaderboardManager: High score data serialized and saved successfully.");
    }

    private void LoadLeaderboard()
    {
        if (PlayerPrefs.HasKey(SavedDataKey))
        {
            string jsonText = PlayerPrefs.GetString(SavedDataKey);
            // Deserialize the text data back into our active list object structure
            currentLeaderboard = JsonUtility.FromJson<LeaderboardData>(jsonText);
        }
        else
        {
            // Initialize a clean default list if no save data exists yet
            currentLeaderboard = new LeaderboardData();
        }
    }
}