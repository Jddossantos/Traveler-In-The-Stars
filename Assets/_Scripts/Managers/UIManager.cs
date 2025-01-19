using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;  //importing the TextMeshPro namespace
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class UIManager : MonoBehaviour
{
    public AudioMixer audioMixer;

    [Header("Main Menu")]
    public GameObject mainMenu;
    public Button startButton;          //start button
    public Button optionsButton;        //options button
    public Button shopButton;           //shop button

    [Header("Level Menu")]
    public GameObject levelMenu;
    public Button levelOneButton;       //level 1 button
    public Button levelTwoButton;       //level 2 button
    public Button levelThreeButton;     //level 3 button
    public Button levelFourButton;      //level 4 button
    public Button levelBackButton;       

    [Header("Options Menu")]
    public GameObject optionsMenu;
    public Button backButton;           //back button

    [Header("Option Menu Sliders")]
    public Slider masterVolSlider;
    public Slider musicVolSlider;
    public Slider sfxVolSlider;

    [Header("Shop Menu")]
    public GameObject shopMenu;
    public Button applySkinButton;      //apply skin button (added for skin application)

    [Header("Leaderboard Menu")]
    public GameObject leaderboardMenu;
    public TextMeshProUGUI[] leaderboardTexts;
    public Button leaderboardButton;
    public Button leaderboardBackButton;

    [Header("Pause Menu (In Game)")]
    public GameObject pauseMenu;
    public Button pauseButton;          //pause button
    public Button resumeButton;         //resume button

    [Header("GameOverUI")]
    public Button restartButton;        //restart button
    public Button returnToMenuButton;   //return to menu button
    public TextMeshProUGUI lastScoreText;
    public TextMeshProUGUI currentHighScoreText;

    [Header("Texts")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI masterVolumeText;
    public TextMeshProUGUI musicVolumeText;
    public TextMeshProUGUI sfxVolumeText;
    public TextMeshProUGUI welcomeMessage;

    [Header("Player Skins")]
    public GameObject playerShip;       //Player ship object to apply the skin
    public GameObject[] playerSkins;    //Array of different player skins
    private int selectedSkinIndex = 0;  //Default selected skin index

    [Header("Skin Selection Buttons")]
    public Button[] skinButtons;        //Array of buttons for skin selection

    private void Start()
    {
        if (startButton)
            startButton.onClick.AddListener(() => SetMenus(levelMenu, mainMenu, optionsMenu, shopMenu, leaderboardMenu));

        if (optionsButton)
            optionsButton.onClick.AddListener(() => SetMenus(optionsMenu, mainMenu, shopMenu, levelMenu, leaderboardMenu));

        if (shopButton)
            shopButton.onClick.AddListener(() => SetMenus(shopMenu, mainMenu, optionsMenu, levelMenu, leaderboardMenu));

        if (leaderboardButton)
        {
            leaderboardButton.onClick.AddListener(DisplayLeaderboard);
            leaderboardButton.onClick.AddListener(() => SetMenus(leaderboardMenu, mainMenu, optionsMenu, levelMenu, shopMenu));
        }

        if (backButton)
            backButton.onClick.AddListener(() => SetMenus(mainMenu, optionsMenu, shopMenu, levelMenu, leaderboardMenu));

        if (levelBackButton)
            levelBackButton.onClick.AddListener(() => SetMenus(mainMenu, optionsMenu, shopMenu, levelMenu, leaderboardMenu));

        if (leaderboardBackButton)
            leaderboardBackButton.onClick.AddListener(() => SetMenus(mainMenu, optionsMenu, shopMenu, levelMenu, leaderboardMenu));

        if (returnToMenuButton)
        {
            returnToMenuButton.onClick.AddListener(() => GameManager.Instance.ChangeScene(0));
            GameManager.Instance.ResetGameData();
        }

        if (restartButton)
            restartButton.onClick.AddListener(RestartGame);

        if (pauseButton)
            pauseButton.onClick.AddListener(() => { pauseMenu.SetActive(!pauseMenu.activeSelf); PauseMenu(); });

        if (resumeButton)
            resumeButton.onClick.AddListener(() => { SetMenus(null, null, null, null, pauseMenu); PauseMenu(); });

        if (levelOneButton)
        {
            levelOneButton.onClick.AddListener(() => GameManager.Instance.ChangeScene(2));
            GameManager.Instance.ResetGameData();
        }

        if (levelTwoButton)
        {
            levelTwoButton.onClick.AddListener(() => GameManager.Instance.ChangeScene(3));
            GameManager.Instance.ResetGameData();
        }

        if (levelThreeButton)
        {
            levelThreeButton.onClick.AddListener(() => GameManager.Instance.ChangeScene(4));
            GameManager.Instance.ResetGameData();
        }

        if (levelFourButton)
        {
            levelFourButton.onClick.AddListener(() => GameManager.Instance.ChangeScene(5));
            GameManager.Instance.ResetGameData();
        }

        if (masterVolSlider)
        {
            masterVolSlider.value = AudioManager.instance.masterVolume * 100;
            masterVolSlider.onValueChanged.AddListener((value) =>
            {
                AudioManager.instance.SetMasterVolume(value / 100);
                UpdateVolumeText(masterVolumeText, value);
            });
        }

        if (musicVolSlider)
        {
            musicVolSlider.value = AudioManager.instance.musicVolume * 100;
            musicVolSlider.onValueChanged.AddListener((value) =>
            {
                AudioManager.instance.SetMusicVolume(value / 100);
                UpdateVolumeText(musicVolumeText, value);
            });
        }

        if (sfxVolSlider)
        {
            sfxVolSlider.value = AudioManager.instance.sfxVolume * 100;
            sfxVolSlider.onValueChanged.AddListener((value) =>
            {
                AudioManager.instance.SetSFXVolume(value / 100);
                UpdateVolumeText(sfxVolumeText, value);
            });
        }

        //score, health, and lives setup
        if (scoreText)
        {
            GameManager.Instance.OnScoreValueChanged.AddListener(UpdateScore);
            scoreText.text = GameManager.Instance.score.ToString();
        }

        if (healthText)
        {
            GameManager.Instance.OnHealthValueChanged.AddListener(UpdateHealth);
            healthText.text = "HP: " + GameManager.Instance.Health.ToString();
        }

        if (livesText)
        {
            GameManager.Instance.OnLifeValueChanged.AddListener(UpdateLives);
            livesText.text = "L: " + GameManager.Instance.Lives.ToString();
        }

        if (timerText)
        {
            GameManager.Instance.OnTimerUpdate.AddListener(UpdateTimer);
            UpdateTimer(GameManager.Instance.GetLevelTimer());
        }

        //setting up skin selection buttons
        for (int i = 0; i < skinButtons.Length; i++)
        {
            int index = i;  // Save the current index for the listener
            skinButtons[i].onClick.AddListener(() => SelectSkin(index));
        }

        //setting up apply skin button
        if (applySkinButton)
        {
            applySkinButton.onClick.AddListener(ApplySelectedSkin);
        }

        //if it's the game over scene, display the scores
        if (SceneManager.GetActiveScene().name == "GameOver")
        {
            DisplayGameOverScores();
        }

        // If the scene is the Main Menu, display the welcome message
        if (SceneManager.GetActiveScene().name == "Main Menu")
        {
            DisplayWelcomeMessage();
        }
    }

    void Update()
    {
    }

    public void UpdateVolumeText(TextMeshProUGUI volumeText, float value)
    {
        if (volumeText != null)
        {
            volumeText.text = Mathf.RoundToInt(value).ToString();
        }
    }

    /// <summary>
    /// Method to toggle between different menus
    /// </summary>
    void SetMenus(GameObject menuToActivate, GameObject menuToDeactivate, GameObject menuToDeactivate2, GameObject menuToDeactivate3, GameObject menuToDeactivate4)
    {
        if (menuToActivate)
            menuToActivate.SetActive(true);
        if (menuToDeactivate)
            menuToDeactivate.SetActive(false);
        if (menuToDeactivate2)
            menuToDeactivate2.SetActive(false);
        if (menuToDeactivate3)
            menuToDeactivate3.SetActive(false);
        if (menuToDeactivate4)
            menuToDeactivate4.SetActive(false);
    }

    void OnSliderValueChanged(float value, TMP_Text volumeText, string sliderName)
    {
        volumeText.text = value.ToString();
        audioMixer.SetFloat(sliderName, value - 80);
    }

    /// <summary>
    /// Method to restart the game by loading the last level
    /// </summary>
    private void RestartGame()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager.Instance is null. Ensure GameManager is initialized and persistent.");
            return;
        }

        if (!string.IsNullOrEmpty(GameManager.Instance.lastLevel))
        {
            GameManager.Instance.ResetGameData();
            SceneManager.LoadScene(GameManager.Instance.lastLevel);
        }
        else
        {
            Debug.LogError("No Level currently stored.");
        }
    }

    /// <summary>
    /// Method to pause/unpause the game
    /// </summary>
    private void PauseMenu()
    {
        if (pauseMenu.activeSelf)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    #region Player UI Management
    private void UpdateScore(int newScore)
    {
        if (scoreText != null)
        {
            scoreText.text = newScore.ToString();
        }
    }

    private void UpdateHealth(int health)
    {
        if (healthText != null)
        {
            healthText.text = "HP: " + health.ToString();
        }
    }

    private void UpdateLives(int lives)
    {
        if (livesText != null)
        {
            livesText.text = "L: " + lives.ToString();
        }
    }

    private void UpdateTimer(float timer)
    {
        if (timerText != null)
        {
            //format time as minutes and seconds
            int minutes = Mathf.FloorToInt(timer / 60);
            int seconds = Mathf.FloorToInt(timer % 60);
            timerText.text = $"{minutes:00}:{seconds:00}";
        }
    }

    private void DisplayGameOverScores()
    {
        int lastScore = StorageManager.Instance.GetLastScore();
        int highScore = StorageManager.Instance.GetHighScore();

        if (lastScoreText != null)
        {
            lastScoreText.text = "Score: " + lastScore.ToString();
        }
        if (currentHighScoreText != null)
        {
            currentHighScoreText.text = "High Score: " + highScore.ToString();
        }
    }

    #endregion

    #region Skin Selection
    /// <summary>
    /// Method to select the skin based on button index.
    /// </summary>
    void SelectSkin(int index)
    {
        selectedSkinIndex = index;
        Debug.Log($"Selected Skin: {selectedSkinIndex}");
    }

    /// <summary>
    /// Method to apply the selected skin to the player.
    /// </summary>
    void ApplySelectedSkin()
    {
        Debug.Log($"Applying Skin {selectedSkinIndex}");

        //ensure the selected skin index is valid
        if (selectedSkinIndex >= 0 && selectedSkinIndex < playerSkins.Length)
        {
            playerShip.GetComponent<SpriteRenderer>().sprite = playerSkins[selectedSkinIndex].GetComponent<SpriteRenderer>().sprite;
        }
    }
    #endregion

    public async void DisplayLeaderboard()
    {
        if (leaderboardTexts.Length == 0)
        {
            Debug.LogError("Leaderboard UI slots not assigned.");
            return;
        }

        //Fetch top 10 scores
        var topScores = await CloudSaveManager.Instance.GetTopScoresAsync();

        if (topScores != null)  //Checking if topScores is not null
        {
            for (int i = 0; i < leaderboardTexts.Length; i++)   //Loops through the scores the total amount of the length of the leaderboard
            {
                if (i < topScores.Count)
                {
                    var playerName = await CloudSaveManager.Instance.GetPlayerNameAsync(topScores[i].Key);
                    var score = topScores[i].Value;
                    leaderboardTexts[i].text = $"{i + 1}. {playerName} : {score}";    //Displays the score
                }
                else
                {
                    leaderboardTexts[i].text = $"{i + 1}. ---"; //Fill empty slots
                }
            }
        }
        else
        {
            Debug.LogError("Failed to fetch leaderboard data.");
        }
    }

    private async void DisplayWelcomeMessage()
    {
        if (welcomeMessage == null)
        {
            Debug.LogError("WelcomeMessage TextMeshProUGUI is not assigned in the inspector.");
            return;
        }

        // Fetch the player name
        string playerName = await CloudSaveManager.Instance.GetCurrentPlayerNameAsync();

        // Update the welcome message
        welcomeMessage.text = $"Welcome! {playerName}!";
    }
}
