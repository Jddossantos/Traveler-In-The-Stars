using Unity.Services.Core;
using Unity.Services.Authentication;
using UnityEngine;

public class CloudSaveInitializer : MonoBehaviour
{
    //This method is called when the script starts
    private async void Start()
    {
        try
        {
            //Initialize Unity Gaming Servies (UGS)
            await UnityServices.InitializeAsync();

            //Checking if the player is already signed in
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                //Signing in the player anonymously (No login needed)
                await AuthenticationService.Instance.SignInAnonymouslyAsync();

                //Log the Player ID to verify authentication
                Debug.Log("Signed in anonymously as: " + AuthenticationService.Instance.PlayerId);
            }

            Debug.Log("Unity Services initialized successfully.");
        }
        catch (System.Exception e)
        {
            //Log an error message if initialization or authentication fails
            Debug.LogError("Failed to initialize Unity Services or authenticate: " + e.Message);
        }
    }
}
