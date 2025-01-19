using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class NameEntryManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField nameInputField;   // TMP Input field for the player's name
    public Button submitButton;             // Button to submit the name
    public TextMeshProUGUI feedbackText;    // Text for displaying feedback

    private string playerNameKey = "playerName"; // Cloud key for the player's name

    private async void Start()
    {
        // Check if a player name already exists
        var existingName = await GetSavedPlayerNameAsync();
        if (!string.IsNullOrEmpty(existingName))
        {
            // Display welcome back message and navigate to main menu
            ShowWelcomeMessage(existingName);
            GoToMainMenu();
        }
    }

    private void ShowWelcomeMessage(string name)
    {
        if (feedbackText != null)
        {
            feedbackText.text = $"Welcome back, {name}!";
            feedbackText.color = Color.green;
        }
    }

    private async Task<string> GetSavedPlayerNameAsync()
    {
        try
        {
            var cloudData = await CloudSaveManager.Instance.LoadAllFromCloudAsync();
            if (cloudData != null && cloudData.ContainsKey(playerNameKey))
            {
                return cloudData[playerNameKey];
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to load player name: {e.Message}");
        }

        return null; // No name found
    }

    public void OnSubmitName()
    {
        string playerName = nameInputField.text.Trim(); // Get the player's input

        if (string.IsNullOrEmpty(playerName))
        {
            SetFeedback("Name cannot be empty!", Color.red);
            return;
        }

        if (playerName.Length > 15) // Optional: restrict name length
        {
            SetFeedback("Name is too long! Max 15 characters.", Color.red);
            return;
        }

        // Save the name and navigate to the main menu
        SavePlayerNameAndNavigate(playerName);
    }

    private async void SavePlayerNameAndNavigate(string playerName)
    {
        try
        {
            // Save player name to the cloud
            await CloudSaveManager.Instance.SaveToCloudAsync(new Dictionary<string, object>
            {
                { playerNameKey, playerName }
            });

            SetFeedback($"Name saved: {playerName}", Color.green);

            // Navigate to main menu
            GoToMainMenu();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to save player name: {e.Message}");
            SetFeedback("Failed to save name. Please try again.", Color.red);
        }
    }

    private void SetFeedback(string message, Color color)
    {
        if (feedbackText != null)
        {
            feedbackText.text = message;
            feedbackText.color = color;
        }
    }

    private void GoToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
