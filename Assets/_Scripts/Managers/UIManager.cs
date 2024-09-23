using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;  //importing the TextMeshPro namespace
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using UnityEngine.Audio;

public class UIManager : MonoBehaviour
{
    public AudioMixer audioMixer;

    [Header("Menus")]
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject shopMenu;
    public GameObject pauseMenu;
    public GameObject levelMenu;

    [Header("Buttons")]
    public Button startButton;          //start button
    public Button optionsButton;        //options button
    public Button shopButton;           //shop button
    public Button pauseButton;          //pause button
    public Button resumeButton;         //resume button
    public Button backButton;           //back button
    public Button levelOneButton;       //level 1 button
    public Button levelTwoButton;       //level 2 button
    public Button applySkinButton;      //apply skin button (added for skin application)

    [Header("GameOverUI")]
    public Button restartButton;        //restart button
    public Button returnToMenuButton;   //return to menu button
    public TextMeshProUGUI lastScoreText;
    public TextMeshProUGUI currentHighScoreText;

    [Header("Sliders")]
    public Slider masterVolSlider;
    public Slider musicVolSlider;
    public Slider sfxVolSlider;

    [Header("Texts")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI masterVolumeText;
    public TextMeshProUGUI musicVolumeText;
    public TextMeshProUGUI sfxVolumeText;

    [Header("Player Skins")]
    public GameObject playerShip;       //Player ship object to apply the skin
    public GameObject[] playerSkins;    //Array of different player skins
    private int selectedSkinIndex = 0;  //Default selected skin index

    [Header("Skin Selection Buttons")]
    public Button[] skinButtons;        //Array of buttons for skin selection

    private void Start()
    {
        if (startButton)
            startButton.onClick.AddListener(() => SetMenus(levelMenu, mainMenu, optionsMenu, shopMenu));

        if (optionsButton)
            optionsButton.onClick.AddListener(() => SetMenus(optionsMenu, mainMenu, shopMenu, levelMenu));

        if (shopButton)
            shopButton.onClick.AddListener(() => SetMenus(shopMenu, mainMenu, optionsMenu, levelMenu));

        if (backButton)
            backButton.onClick.AddListener(() => SetMenus(mainMenu, optionsMenu, shopMenu, levelMenu));

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
            resumeButton.onClick.AddListener(() => { SetMenus(null, null, null, pauseMenu); PauseMenu(); });

        if (levelOneButton)
        {
            levelOneButton.onClick.AddListener(() => GameManager.Instance.ChangeScene(1));
            GameManager.Instance.ResetGameData();
        }

        if (levelTwoButton)
        {
            levelTwoButton.onClick.AddListener(() => GameManager.Instance.ChangeScene(2));
            GameManager.Instance.ResetGameData();
        }

        if (masterVolSlider)
        {
            masterVolSlider.onValueChanged.AddListener((value) => OnSliderValueChanged(value, masterVolumeText, "MasterVol"));
            float mixerValue;
            audioMixer.GetFloat("MasterVol", out mixerValue);
            masterVolSlider.value = mixerValue + 80;
            if (masterVolumeText)
                masterVolumeText.text = masterVolSlider.value.ToString();
        }

        if (musicVolSlider)
        {
            musicVolSlider.onValueChanged.AddListener((value) => OnSliderValueChanged(value, musicVolumeText, "MusicVol"));
            float mixerValue;
            audioMixer.GetFloat("MusicVol", out mixerValue);
            musicVolSlider.value = mixerValue + 80;
            if (musicVolumeText)
                musicVolumeText.text = musicVolSlider.value.ToString();
        }

        if (sfxVolSlider)
        {
            sfxVolSlider.onValueChanged.AddListener((value) => OnSliderValueChanged(value, sfxVolumeText, "SFXVol"));
            float mixerValue;
            audioMixer.GetFloat("SFXVol", out mixerValue);
            sfxVolSlider.value = mixerValue + 80;
            if (sfxVolumeText)
                sfxVolumeText.text = sfxVolSlider.value.ToString();
        }

        //score, health, and lives setup
        if (scoreText)
        {
            GameManager.Instance.OnScoreValueChanged.AddListener(UpdateScore);
            scoreText.text = "S: " + GameManager.Instance.score.ToString();
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
    }

    void Update()
    {
    }

    /// <summary>
    /// Method to toggle between different menus
    /// </summary>
    void SetMenus(GameObject menuToActivate, GameObject menuToDeactivate, GameObject menuToDeactivate2, GameObject menuToDeactivate3)
    {
        if (menuToActivate)
            menuToActivate.SetActive(true);
        if (menuToDeactivate)
            menuToDeactivate.SetActive(false);
        if (menuToDeactivate2)
            menuToDeactivate2.SetActive(false);
        if (menuToDeactivate3)
            menuToDeactivate3.SetActive(false);
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
            scoreText.text = "S: " + newScore.ToString();
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

    private void DisplayGameOverScores()
    {
        int lastScore = StorageManager.Instance.GetLastScore();
        int highScore = StorageManager.Instance.GetHighScore();

        if (lastScoreText != null)
        {
            lastScoreText.text = "This Rounds Score: " + lastScore.ToString();
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
}
