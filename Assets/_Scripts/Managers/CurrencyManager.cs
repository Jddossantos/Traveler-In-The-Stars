/*
 * THIS IS A TEMP FOLDER FOR OTHER CODE FOR NOW
 * DO NOT DELETE! 
 * 
 * using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StorageManager : MonoBehaviour
{
    public static StorageManager Instance { get; private set; }

    private const string SelectedSkinKey = "SelectedSkin";  //skin key for storing

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

    //function to calculate the final score per level
    public void CalculateFinalScore(int currentScore, int level, float timer)
    {
        int multiplier = level;     //get level multiplier from function
        int timerInt = Mathf.FloorToInt(1000 / timer);         //higher the time, the lower the bonus
        int finalScore = currentScore + timerInt * multiplier;

        string finalScoreKey = $"Level{level}_FinalScore";
        PlayerPrefs.SetInt(finalScoreKey, finalScore);  //save the final score
        PlayerPrefs.Save();
    }

    public int GetLevelFinalScore(int level)
    {
        string key = $"Level{level}_FinalScore";
        return PlayerPrefs.GetInt(key, 0);  //default to 0 if no final score is found
    }

    #region Set/Getting Highscores
    public void SaveLevelScore(int score)
    {
        int level = GameManager.Instance.GetLevel();    //retrieve current level
        string levelKey = $"Level{level}_Score";        //generate a key for this level

        PlayerPrefs.SetInt(levelKey, score);    //save the score to the key
        PlayerPrefs.Save();

        Debug.Log($"StorageManager: Saved score {score} for {levelKey}");
    }

    public void SaveLevelHighScore(int score)
    {
        int level = GameManager.Instance.GetLevel();
        string highScoreKey = $"Level{level}_HighScore";

        int currentHighScore = PlayerPrefs.GetInt(highScoreKey, 0); //retrieve current highscore key or default to 0
        if (score > currentHighScore)
        {
            PlayerPrefs.SetInt(highScoreKey, score);
            PlayerPrefs.Save();

            Debug.Log($"New high score {score} saved to {highScoreKey}");
        }
        else
        {
            {
                Debug.Log("No new high score saved!");
            }
        }
    }

    public int GetLevelHighScore(int level)
    {
        string highScoreKey = $"Level{level}_HighScore";
        return PlayerPrefs.GetInt(highScoreKey, 0);     //default to 0 if no highscore is saved
    }
    #endregion

    #region Skin Storage
    //function for saving selected skin 
    public void SaveSelectedSkin(int skinIndex)
    {
        PlayerPrefs.SetInt(SelectedSkinKey, skinIndex);     //saves selected skin based on parameter
        PlayerPrefs.Save();
    }

    public int GetSelectedSkin()
    {
        return PlayerPrefs.GetInt(SelectedSkinKey, 0); //default skin is set to 0, if no skin is selected
    }
    #endregion
}*/
