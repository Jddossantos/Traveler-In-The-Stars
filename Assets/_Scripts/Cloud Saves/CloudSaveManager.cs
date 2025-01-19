using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq; // Required for LINQ operations
using Unity.Services.CloudSave;
using UnityEngine;

public class LeaderboardEntry
{
    public string PlayerName;
    public int Score;

    public LeaderboardEntry(string playerName, int score)
    {
        PlayerName = playerName;
        Score = score;
    }
}

public class CloudSaveManager : MonoBehaviour
{
    // Singleton instance for easy access
    public static CloudSaveManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Saves data to the cloud in JSON format.
    /// </summary>
    /// <param name="data">Dictionary of key-value pairs to save.</param>
    public async Task SaveToCloudAsync(Dictionary<string, object> data)
    {
        try
        {
            await CloudSaveService.Instance.Data.Player.SaveAsync(data); //Saving data to the cloud
            Debug.Log("Data successfully saved to the cloud.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to save data to the cloud: {e.Message}");
        }
    }

    /// <summary>
    /// Loads all data from the cloud.
    /// </summary>
    public async Task<Dictionary<string, string>> LoadAllFromCloudAsync()
    {
        try
        {
            var results = await CloudSaveService.Instance.Data.LoadAllAsync(); //Load all keys and values
            Debug.Log("All data successfully loaded from the cloud.");
            return results; //Returns results
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to load all data from the cloud: " + e.Message); 
            return null;    //Returns null
        }
    }

    public async Task<List<KeyValuePair<string, int>>> GetTopScoresAsync()
    {
        try
        {
            var results = await CloudSaveService.Instance.Data.LoadAllAsync();
            Debug.Log("Data successfully fetched for leaderboard.");

            //Extract only high scores from the data and parse them
            var scores = results
                .Where(entry => entry.Key.Contains("highScore"))
                .Select(entry => new KeyValuePair<string, int>(entry.Key, int.Parse(entry.Value)))
                .OrderByDescending(entry => entry.Value) //Sort scores from highest to lowest
                .Take(10)       //Get top 10 scores
                .ToList();

            return scores;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to fetch top scores: " + e.Message);
            return null;
        }
    }

    /// <summary>
    /// Gets the player name associated with a high score key.
    /// </summary>
    /// <param name="key">The key for which to fetch the player name.</param>
    /// <returns>The player name as a string, or null if not found.</returns>
    public async Task<string> GetPlayerNameAsync(string key)
    {
        try
        {
            var results = await CloudSaveService.Instance.Data.LoadAllAsync();

            // Attempt to find the corresponding player name key
            if (results.ContainsKey("playerName"))
            {
                Debug.Log($"Player name found: {results["playerName"]}");
                return results["playerName"];
            }
            else
            {
                Debug.LogWarning("Player name key not found in cloud data.");
                return "Unknown Player";
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to fetch player name for key {"highScore"}: {e.Message}");
            return "Unknown Player";
        }
    }

    public async Task<string> GetCurrentPlayerNameAsync()
    {
        try
        {
            var results = await CloudSaveService.Instance.Data.LoadAllAsync();

            // Check if "playerName" exists in cloud data
            if (results.TryGetValue("playerName", out var playerName))
            {
                Debug.Log($"Fetched player name: {playerName}");
                return playerName;
            }
            else
            {
                Debug.LogWarning("Player name not found in cloud data. Returning default.");
                return "Player";
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error fetching current player name: {e.Message}");
            return "Player";
        }
    }
}
