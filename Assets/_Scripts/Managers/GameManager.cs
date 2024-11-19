using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    //singleton instance
    public static GameManager Instance { get; private set; }
    static GameManager instance;

    //player variables and management
    [SerializeField] PlayerController playerController;
    [SerializeField] PlayerSpawn playerSpawn;

    //skin management
    public GameObject playerShip;       //player ship object for skin switching
    public GameObject[] playerSkins;    //array for different player skins
    private int selectedSkinIndex;      //currently selected skin index

    //Transform playerSpawnLocation;

    private int playerScore;            //player score variable

    //health and lives management
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int maxLives = 3;
    private int currentHealth;
    private int currentLives;

    //event to update Game UI
    public UnityEvent<int> OnLifeValueChanged;
    public UnityEvent<int> OnHealthValueChanged;
    public UnityEvent<int> OnScoreValueChanged;
    public UnityEvent<float> OnTimerUpdate;
    public string lastLevel;

    private float inGameTimer;
    private void Awake()
    {
        //if statement to ensure only one instance of game manager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  //ensuring it persists across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        int sceneIndex = (SceneManager.GetActiveScene().name == "Level1" ? 0 : 2);

        playerScore = 0;
        currentHealth = maxHealth;
        currentLives = maxLives;

        //initialize UI set up
        OnScoreValueChanged?.Invoke(playerScore);
        OnHealthValueChanged?.Invoke(currentHealth);
        OnLifeValueChanged?.Invoke(currentLives);
    }

    // Update is called once per frame
    void Update()
    {
        //updating the timer every second and notfiying UI
        inGameTimer += Time.deltaTime;
        OnTimerUpdate?.Invoke(inGameTimer);
    }

    #region Score Management

    public int score
    {
        get => playerScore;
        set
        {
            playerScore = value;

            OnScoreValueChanged?.Invoke(playerScore);
        }
    }

    /// <summary>
    /// method to add score
    /// </summary>
    /// <param></param>
    public void AddScore(int addScore)
    {
        score += addScore;   //adding to score
        Debug.Log($"GameManager: AddScore - Player's score: {playerScore}");
    }

    #endregion

    #region Player Health and Lives Management
    public int Lives
    {
        get => currentLives;
        set
        {
            currentLives = Mathf.Clamp(value, 0, maxLives);         //clamps lives to not exceed the total amount of maxLives
            OnLifeValueChanged?.Invoke(currentLives);

            if (currentLives <= 0)
            {
                GameOver();         //if lives is less than or equal to 0, call game over function
            }
        }
    }

    public int Health
    {
        get => currentHealth;
        set
        {
            currentHealth = Mathf.Clamp(value, 0, maxHealth);       //clamps player health to the maxHealth value
            OnHealthValueChanged?.Invoke(currentHealth);

            if (currentHealth <= 0)
            {
                PlayerDeath();      //if health is less than or equal to 0, call player death function
            }
        }
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
    }

    private void PlayerDeath()
    {
        Lives--;
        if (currentLives > 0)
        {
            Respawn();
        }
        else
        {
            GameOver();
        }
    }

    private void Respawn()
    {
        Health = maxHealth;
        playerController.transform.position = playerSpawn.spawnLocation.position;
    }
    #endregion

    #region Scene Management
    //method to Scnene change
    public void ChangeScene(int sceneIndex)
    {
        //load the game scene
        SceneManager.LoadScene(sceneIndex);
        Time.timeScale = 1;
    }

    public float GetLevelTimer()
    {
        return inGameTimer;
    }

    public void ResetGameTimer()
    {
        inGameTimer = 0f;
    }

    #endregion

    #region Level Management/ Restart & Game Over

    private Dictionary<string, int> levelMapping = new Dictionary<string, int>
    { {"Level1", 1 }, {"Level2", 2}, {"Level3", 3}, {"Level4", 4} }; 

    public int GetLevel()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (levelMapping.TryGetValue(currentSceneName, out int level))
        {
            return level;
        }
        Debug.LogWarning($"GetLevel : Unknown scene name '{currentSceneName}'. defaulting to level 1.");
        return 1;
    }

    public void ResetGameData()
    {
        score = 0;
        currentHealth = maxHealth;
        currentLives = maxLives;

        OnScoreValueChanged?.Invoke(playerScore);
        OnHealthValueChanged?.Invoke(currentHealth);
        OnLifeValueChanged?.Invoke(currentLives);

        Respawn();
        ResetGameTimer();

    }

    public void GameOver()
    {
        int currentLevel = GetLevel();

        //saving the current score
        StorageManager.Instance.SaveLastScore(playerScore);
        StorageManager.Instance.SaveHighScore(playerScore); //checking if it's a new high score

        StorageManager.Instance.CalculateFinalScore(currentLevel, score, inGameTimer);

        SceneManager.LoadScene("GameOver");
    }

    #endregion

    #region Skin Management
    //method to change the skin of the player
    public void ChangePlayerSkin(int index)
    {
        //ensuring that the selected skin is within the array
        if (index >= 0 && index < playerSkins.Length)
        {
            selectedSkinIndex = index;  //updating the selected skin

            //switch the player ship skin
            playerShip.GetComponent<SpriteRenderer>().sprite = playerSkins[selectedSkinIndex].GetComponent<SpriteRenderer>().sprite;
        }
    }
    #endregion
}