/*using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    //References to other components (PlayerController, etc.)
    [SerializeField] private PlayerSpawn playerSpawn;
    [SerializeField] private PlayerController playerController;

    //Health and lives settings
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int maxLives = 3;

    public int currentHealth = 0;
    public int currentLives = 0;

    //Unity Events for updating UI
    public UnityEvent<int> OnHealthChanged;
    public UnityEvent<int> OnLivesChanged;

    void Start()
    {
        //initialize player's lives and health
        ResetPlayerState();
    }

    //function to reset player's health and lives (called when game starts or respawns)
    public void ResetPlayerState()
    {
        currentLives = maxLives;
        currentHealth = maxHealth;

        //trigger UI updates
        OnLivesChanged?.Invoke(currentLives);
        OnHealthChanged?.Invoke(currentHealth);
    }

    //property to control lives
    public int Lives
    {
        get => currentLives;
        set
        {
            if (currentLives > value)
                Respawn();

            currentLives = value;

            if (currentLives > maxLives)
                currentLives = maxLives;

            //check if player is out of lives
            if (currentLives <= 0)
            {
                GameOver();
            }
            OnLivesChanged?.Invoke(currentLives);
        }
    }

    //property to control health
    public int Health
    {
        get => currentHealth;
        set
        {
            currentHealth = Mathf.Clamp(value, 0, maxHealth);
            Debug.Log("Player's health: " + currentHealth);

            if (currentHealth <= 0)
            {
                PlayerDeath();
            }
            OnHealthChanged?.Invoke(currentHealth);
        }
    }

    //function to handle player taking damage
    public void TakeDamage(int damageAmount)
    {
        Debug.Log("Player took damage: " + damageAmount);
        Health -= damageAmount;
    }

    //handle player death logic
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

    //handle respawn logic
    private void Respawn()
    {
        Health = maxHealth;

        // Move player to spawn location
        playerController.transform.position = playerSpawn.spawnLocation.position;
    }

    //handle game over logic
    private void GameOver()
    {
        Debug.Log("Game Over");
        GameManager.Instance.GameOver();
    }
}*/
