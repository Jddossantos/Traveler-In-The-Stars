using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StorageManager : MonoBehaviour
{
    public static StorageManager Instance { get; private set; }

    private const string HighScoreKey = "HighScore";        //high score key for storing
    private const string LastScoreKey = "LastScore";        //last score key for storing
    private const string SelectedSkinKey = "SelectedSkin";  //skin key for storing

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
    }

    private void Start()
    {
        gm = GameManager.Instance;
    }

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

    #region Set/Getting Last Score

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
    #endregion

    #region Set/Getting Highscores
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
    #endregion

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
