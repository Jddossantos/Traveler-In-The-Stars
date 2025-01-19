using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PlayerData
{
    public int highScore;       //high score key for storing
    public int lastScore;       //last score key for storing
    public int selectedSkin;    //skin key for storing
    public string playerName;   //player name for storing
}

public class StorageManager : MonoBehaviour
{
    public static StorageManager Instance { get; private set; }

    private string saveFilePath;
    private PlayerData playerData;

    public bool useCloudSave = true;    //Toggle for cloud save

    private int multiplier;

    GameManager gm;

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

        //Setting the file path where data will be saved
        saveFilePath = Path.Combine(Application.persistentDataPath, "playerdata.json");
        _ = LoadGameDataAsync();         //Loading data at the start
    }

    private void Start()
    {
        gm = GameManager.Instance;
    }

    /// <summary>
    /// Saves game data locally and optionally to the cloud.
    /// </summary>
    public async void SaveGameDataAsync()
    {
        // Save locally
        SaveGameData();

        if (useCloudSave)
        {
            // Prepare cloud data
            Dictionary<string, object> cloudData = new Dictionary<string, object>
            {
                { "highScore", playerData.highScore },
                { "lastScore", playerData.lastScore },
                { "selectedSkin", playerData.selectedSkin },
                { "playerName", playerData.playerName }
            };

            // Save to cloud
            await CloudSaveManager.Instance.SaveToCloudAsync(cloudData);
        }
    }

    /// <summary>
    /// Loads game data from local storage and optionally syncs with the cloud.
    /// </summary>
    private async Task LoadGameDataAsync()
    {
        //Load locally first
        LoadGameData();

        if (useCloudSave)
        {
            var cloudData = await CloudSaveManager.Instance.LoadAllFromCloudAsync();
            if (cloudData != null)
            {
                // Sync cloud data with local data
                playerData.highScore = cloudData.ContainsKey("highScore") ? int.Parse(cloudData["highScore"]) : playerData.highScore;
                playerData.lastScore = cloudData.ContainsKey("lastScore") ? int.Parse(cloudData["lastScore"]) : playerData.lastScore;
                playerData.selectedSkin = cloudData.ContainsKey("selectedSkin") ? int.Parse(cloudData["selectedSkin"]) : playerData.selectedSkin;
                playerData.playerName = cloudData.ContainsKey("playerName") ? cloudData["playerName"].ToString() : playerData.playerName;

                SaveGameData(); //Ensure local data matches cloud
            }
        }
    }

    #region Local Save data
    /// <summary>
    /// Saves all player data to a JSON file.
    /// </summary>
    public void SaveGameData()
    {
        try
        {
            string json = JsonUtility.ToJson(playerData, true); //Convert player data to JSON
            File.WriteAllText(saveFilePath, json);              //Write to file
            Debug.Log("Game data saved to " + saveFilePath);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to save game data: " + e.Message);
        }
    }

    /// <summary>
    /// Loads player data from the JSON file.
    /// </summary>
    private void LoadGameData()
    {
        if (File.Exists(saveFilePath))
        {
            try
            {
                string json = File.ReadAllText(saveFilePath);           //Read file
                playerData = JsonUtility.FromJson<PlayerData>(json);    //Deserialize JSON into PlayerData
                Debug.Log("Game data loaded from " + saveFilePath);
            }
            catch (System.Exception e)
            {
                Debug.LogError("Failed to load game data: " + e.Message);
                ResetGameData(); //Reset data if file is corrupted
            }
        }
        else
        {
            Debug.Log("No save file found. Creating new data.");
            ResetGameData();    //Create default data if file doesn't exist
        }
    }
    #endregion

    /// <summary>
    /// Resets player data to default values.
    /// </summary>
    private void ResetGameData()
    {
        playerData = new PlayerData
        {
            highScore = 0,
            lastScore = 0,
            selectedSkin = 0
        };
        SaveGameData();     //Save default data
    }

    #region Level Multiplier/ Caluclations
    //getting a multiplier for each level
    private int GetMultiplierLevel(int level)
    {
        switch (level)
        {
            case 1:
                return 1;   //returns a multiplier of 1 if level 1
            case 2:
                return 2;   //returns a multiplier of 2 if level 2
            case 3:
                return 3;   //returns a multiplier of 3 if level 3
            case 4:
                return 4;   //returns a multiplier of 4 if level 4
            default:
                return 1;   //returns a default multipler of 1
        }
    }

    //function to calculate the final score per level
    public int CalculateFinalScore (int currentScore, int level, float timer)
    {
        timer = GameManager.Instance.GetLevelTimer();

        int multiplier = GetMultiplierLevel(level);     //get level multiplier from function
        int timerInt = Mathf.RoundToInt(timer);         //converting float timer to an integet for calculations
        int finalScore = (currentScore + timerInt) * multiplier;    //calculations for the finalScore
        return finalScore;
    }
    #endregion

    #region Set/Getting Scores and Names
    //function to get last score
    public int GetLastScore()
    {
        return playerData.lastScore;
    }

    //function to save last score
    public void SaveLastScore(int score)
    {
        playerData.lastScore = score;
        SaveGameDataAsync();
    }

    //function to get high score
    public int GetHighScore()
    {
        return playerData.highScore;
    }

    //function to save the highscore by setting last score to the new highscore if the score is greater
    public void SaveHighScore(int score)
    {
        if (score > playerData.highScore)
        {
            playerData.highScore = score;
            SaveGameDataAsync();
        }

    }

    public string GetPlayerName()
    {
        return playerData.playerName;
    }

    public void SavePlayerName(string name)
    {
        playerData.playerName = name;
        SaveGameDataAsync();
    }
    #endregion

    #region Skin Storage
    //function for saving selected skin 
    public void SaveSelectedSkin(int skinIndex)
    {
        playerData.selectedSkin = skinIndex;
        SaveGameDataAsync();
    }

    public int GetSelectedSkin()
    {
        return playerData.selectedSkin;
    }
    #endregion
}
