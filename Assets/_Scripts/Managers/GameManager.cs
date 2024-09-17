using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.Audio;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    //singleton instance
    public static GameManager Instance { get; private set; }
    static GameManager instance;

    //player instances
    [SerializeField] public PlayerController playerPrefab;
    [SerializeField] public PlayerHealth playerHealth;

    Transform playerSpawnLocation;

    private int playerScore;            //player score variable

    public GameObject playerShip;       //player ship object for skin switching
    public GameObject[] playerSkins;    //array for different player skins
    private int selectedSkinIndex;      //currently selected skin index

    //event to update Game UI
    public UnityEvent<int> OnScoreValueChanged;
    public string lastLevel;

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
        playerHealth = GetComponent<PlayerHealth>();

        int sceneIndex = (SceneManager.GetActiveScene().name == "Level1" ? 0 : 2);

        playerScore = 0;

        //initialize UI set up
        OnScoreValueChanged?.Invoke(playerScore);
    }

    // Update is called once per frame
    void Update()
    {
        
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

    //method to Scnene change
    public void ChangeScene(int sceneIndex)
    {
        //load the game scene
        SceneManager.LoadScene(sceneIndex);
        Time.timeScale = 1;
    }

    public void ResetGameData()
    {
        playerHealth.ResetPlayerState();
        score = 0;
    }

    public void GameOver()
    {
        lastLevel = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("GameOver");
    }

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
}