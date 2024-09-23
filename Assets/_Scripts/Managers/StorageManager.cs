using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageManager : MonoBehaviour
{
    public static StorageManager Instance { get; private set; }

    private const string HighScoreKey = "HighScore";        //high score key for storing
    private const string LastScoreKey = "LastScore";        //last score key for storing
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

    //function to save last score
    public void SaveLastScore(int score)
    {
        PlayerPrefs.SetInt(LastScoreKey, score);    //saves last score in key based on score input parameter
        PlayerPrefs.Save();
    }

    //function to get last score
    public int GetLastScore()
    {
        return PlayerPrefs.GetInt(LastScoreKey, 0);
    }

    //function to save the highscore by setting last score to the new highscore if the score is greater
    public void SaveHighScore(int score)
    {
        int highScore = GetHighScore();
        if (score > highScore)
        {
            PlayerPrefs.SetInt(HighScoreKey, score);    //sets it as the new score based on parameter
            PlayerPrefs.Save(); //saves the score
        }
    }

    //function to get high score
    public int GetHighScore()
    {
        return PlayerPrefs.GetInt(HighScoreKey, 0);
    }

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
}
