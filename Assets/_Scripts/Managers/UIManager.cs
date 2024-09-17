using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;  //importing the TextMeshPro namespace
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    [SerializeField] public PlayerHealth playerHealth;

    /// <summary>
    /// Menu game objects for enabling and disabling 
    /// </summary>
    [Header("Menus")]
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject shopMenu;
    public GameObject pauseMenu;

    /// <summary>
    /// Button for setting up UI in menus
    /// </summary>
    [Header("Buttons")]
    public Button startButton;          //start button
    public Button optionsButton;        //options button
    public Button shopButton;           //shop button
    public Button restartButton;        //restart button
    public Button pauseButton;          //pause button
    public Button resumeButton;         //resume button
    public Button backButton;           //back button
    public Button returnToMenuButton;   //return to menu button
    public Button applySkinButton;      //apply skin button (added for skin application)

    [Header("Gameplay UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI livesText;

    [Header("Player Skins")]
    public GameObject playerShip;       //Player ship object to apply the skin
    public GameObject[] playerSkins;    //Array of different player skins
    private int selectedSkinIndex = 0;  //Default selected skin index

    [Header("Skin Selection Buttons")]
    public Button[] skinButtons;        //Array of buttons for skin selection

    void Start()
    {
        if (startButton)
            startButton.onClick.AddListener(() => GameManager.Instance.ChangeScene(1));

        if (optionsButton)
            optionsButton.onClick.AddListener(() => SetMenus(optionsMenu, mainMenu, shopMenu));

        if (shopButton)
            shopButton.onClick.AddListener(() => SetMenus(shopMenu, mainMenu, optionsMenu));

        if (backButton)
            backButton.onClick.AddListener(() => SetMenus(mainMenu, optionsMenu, shopMenu));

        if (returnToMenuButton)
            returnToMenuButton.onClick.AddListener(() => GameManager.Instance.ChangeScene(0));

        if (restartButton)
            restartButton.onClick.AddListener(RestartGame);

        if (pauseButton)
            pauseButton.onClick.AddListener(() => { pauseMenu.SetActive(!pauseMenu.activeSelf); PauseMenu(); });

        if (resumeButton)
            resumeButton.onClick.AddListener(() => { SetMenus(null, null, pauseMenu); PauseMenu(); });

        //score, health, and lives setup
        if (scoreText)
        {
            GameManager.Instance.OnScoreValueChanged.AddListener(UpdateScore);
            scoreText.text = "S: " + GameManager.Instance.score.ToString();
        }

        if (healthText)
        {
            playerHealth.OnHealthChanged.AddListener(UpdateHealth);
            healthText.text = "HP: " + playerHealth.Health.ToString();
        }

        if (livesText)
        {
            playerHealth.OnLivesChanged.AddListener(UpdateLives);
            livesText.text = "L: " + playerHealth.Lives.ToString();
        }

        //setting up skin selection buttons
        for (int i = 0; i < skinButtons.Length; i++)
        {
            int index = i;  // Save the current index for the listener
            skinButtons[i].onClick.AddListener(() => SelectSkin(index));
        }

        //setting up apply skin button
        if (applySkinButton)
            applySkinButton.onClick.AddListener(ApplySelectedSkin);
    }

    void Update()
    {
    }

    /// <summary>
    /// Method to toggle between different menus
    /// </summary>
    void SetMenus(GameObject menuToActivate, GameObject menuToDeactivate, GameObject menuToDeactivate2)
    {
        if (menuToActivate)
            menuToActivate.SetActive(true);
        if (menuToDeactivate)
            menuToDeactivate.SetActive(false);
        if (menuToDeactivate2)
            menuToDeactivate2.SetActive(false);
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
