using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameplayUI : MonoBehaviour
{
    /// <summary>
    /// Components and variables for "text" for UI updating
    /// First set is for updating player health, lives and score
    /// </summary>


    private void Awake()
    {
        //scoreText = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();
        //healthText = GameObject.Find("HealthText").GetComponent<TextMeshProUGUI>();
        //livesText = GameObject.Find("LivesText").GetComponent<TextMeshProUGUI>();
    }

    //method to update the score in the UI
    public void UpdateScore(float newScore)
    {

    }

    //method to update the health in the UI
    public void UpdateHealth(float health)
    {

    }

    //method to update the lives in
    public void UpdateLives(int lives)
    {

    }
}
