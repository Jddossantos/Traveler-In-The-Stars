using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopMenuController : MonoBehaviour
{
    [Header("Skin UI Elements")]
    public GameObject[] skinButtons;    //Array of buttons to switch skins
    public Button backButton;           //Button to return to the main menu
    public Button applyButton;          //Button to apply the selected skin

    [Header("Player Skins")]
    public GameObject playerShip;       //Player ship object to apply the skin
    public GameObject[] playerSkins;    //Array of different player skins

    private int selectedSkinIndex;      //Index of the selected skin

    void Start()
    {
        //adding listeners to the skin buttons
        for (int i = 0; i < skinButtons.Length; i++)
        {
            int index = i;  // Store the current index for the listener
            skinButtons[i].GetComponent<Button>().onClick.AddListener(() => SelectSkin(index));
        }

        //listener for the back button
        if (backButton)
            backButton.onClick.AddListener(() => SceneManager.LoadScene(0));

        //listener for the apply button
        if (applyButton)
            applyButton.onClick.AddListener(() =>
            {
                StorageManager.Instance.SaveSelectedSkin(selectedSkinIndex);
                ApplySelectedSkin();
            });
    }

    //method to highlight the selected skin button
    void SelectSkin(int index)
    {
        selectedSkinIndex = index;
        Debug.Log($"Selected Skin: {selectedSkinIndex}");

        HighlightSelectedSkin();
    }

    //apply the selected skin to the player
    void ApplySelectedSkin()
    {
        Debug.Log($"Applying Skin {selectedSkinIndex}");

        //ensure that the selected skin is within the array bounds
        if (selectedSkinIndex >= 0 && selectedSkinIndex < playerSkins.Length)
        {
            //switch the player ship skin
            playerShip.GetComponent<SpriteRenderer>().sprite = playerSkins[selectedSkinIndex].GetComponent<SpriteRenderer>().sprite;
        }
    }

    void HighlightSelectedSkin()
    {
        foreach (GameObject button in skinButtons)
        {
            button.GetComponent<Image>().color = Color.white;   //default color set
        }
        
        //highlight the selected skin button
        skinButtons[selectedSkinIndex].GetComponent <Image>().color = Color.green;  //highlight color
    }
}
