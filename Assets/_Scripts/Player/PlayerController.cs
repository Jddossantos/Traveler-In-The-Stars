using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //player controller instance
    public bool playerActive = true;                       //flag to check if the player is active

    //player variables
    [SerializeField] float playerSpeed = 40f;               //the players movement speed

    //player components
    [SerializeField] public GameObject playerPrefab;
    private Rigidbody2D playerRB;

    public bool isTouching = false;         //flag to check if the screen is being touched

    //players touch input vectors
    private Vector2 touchStartPosition;     //initial position of the touch
    private Vector2 touchEndPosition;       //end position of the touch

    //variables to store screen boundries
    public float minX, maxX, minY, maxY;

    void Start()
    {
        //getting rigidbody component
        playerRB = GetComponent<Rigidbody2D>();

        CalculateScreenBounds();
        //Debug.Log($"PlayerController: Start Bounds Calculated: MinX = {minX}, MaxX = {maxX}, MinY = {minY}, MaxY = {maxY}");
    }

    //method to enable touch input
    private void OnEnable()
    {
        //Subsribing to touch events from the InputManger class
        InputManager.OnStartTouch += StartTouch;
        InputManager.OnEndTouch += EndTouch;
        //Debug.Log("PlayerController: OnEnable - Subscribed to InputManager Events");
    }

    //method to disable touch input
    private void OnDisable()
    {
        //Unsubscribe from the touch events from the InputManger class
        InputManager.OnStartTouch -= StartTouch;
        InputManager.OnEndTouch -= EndTouch;
        //Debug.Log("PlayerController: OnDisable - Unsubscribed to InputManager Events");
    }

    //method to detect when the touch starts
    private void StartTouch(Vector2 position, float time)
    {
        //recording the start position of the touch and set the isTouching flag
        touchStartPosition = position;
        isTouching = true;
        //Debug.Log($"PlayerController: StartTouch - Start Touch at Position: {position}, Time: {time}");
    }

    //method to detect when the touch ends
    private void EndTouch(Vector2 position, float time)
    {
        //recording the end position of the touch and reseting the isTouching flag
        //touchEndPosition = position;
        isTouching = false;
        //Debug.Log($"PlayerController: EndTouch - End Touch at Position: {position}, Time: {time}");
    }

    // Update is called once per frame
    void Update()
    {
        //if the screen is being touched, claculate the direction of movement
        if (isTouching)
        {
            //getting current touch position
            Vector2 currentTouchPosition = InputManager.Instance.PrimaryPosition();

            //calculating movement direction of player
            Vector2 direction = currentTouchPosition - touchStartPosition;

            //moving the player in the desired direction of touch
            MovePlayer(direction);

            //Updating the start position for continuous movement
            touchStartPosition = currentTouchPosition;
            //Debug.Log($"PlayerController: Update - Current Touch Position: {currentTouchPosition}, Direction: {direction}");
        }
    }

    //method to move the player with touch input
    private void MovePlayer(Vector2 direction)
    {

        //calculating the new position of the players ship
        Vector2 newPosition = (Vector2)transform.position + direction * playerSpeed * Time.deltaTime;

        //clamping the player's position to prevent it from going off screen
        //adjustingthe clamp values accoring to your game's screen boundries
        float clampedX = Mathf.Clamp(newPosition.x, minX, maxX);   //allowing movement in X (side to side)
        float clampedY = Mathf.Clamp(newPosition.y, minY, maxY);   //allowing movement in Y (up and down)


        //applying the clamped position to the player's transform
        transform.position = new Vector3(clampedX, clampedY, transform.position.z);

        //Debug.Log($"PlayerController: MovePlayer - New Position: {transform.position}");
    }

    public void CalculateScreenBounds()
    {
        //getting the main camera's view of the screen bounds
        Camera levelCamera = Camera.main;

        //calculating the worlds position of the screen's edges
        minX = levelCamera.ViewportToWorldPoint(new Vector3(0, 0, levelCamera.nearClipPlane)).x;
        maxX = levelCamera.ViewportToWorldPoint(new Vector3(1, 0, levelCamera.nearClipPlane)).x;
        minY = levelCamera.ViewportToWorldPoint(new Vector3(0, 0, levelCamera.nearClipPlane)).y;
        maxY = levelCamera.ViewportToWorldPoint(new Vector3(0, 1, levelCamera.nearClipPlane)).y;

        //adding a small padding 
        float padding = 0.1f;
        minX += padding;
        maxX -= padding;
        minY += padding;
        maxY -= padding;
    }

    /*public void TakeDamage(int damage)
    {
        //subtracting the incoming damage from the player's current health
        playerHealth -= damage;
        //Debug.Log($"PlayerController: TakeDamage - Player took {damage} damage, current health: {playerHealth}");

        //checking if the player's health is zero or less
        if (playerHealth <= 0)
        {
            //Debug.Log("PlayerController: TakeDamage - Player is dead");
            PlayerDeath();
        }
        else
        {
            UIManager.Instance.UpdateHealth(playerHealth);
        }
    }

    

    public void PlayerDeath()
    {
        playerLives--;                                   //reduces life by 1
        UIManager.Instance.UpdateLives(playerLives);     //updating UI

        if (playerLives >= 0)
        {
            Destroy(gameObject);
            GameManager.Instance.PlayerRespawn();
        }
        else
        {
            //load game over menu or end the game
            
            Debug.Log("GameManager: PlayerDeath - Game Over");
            Destroy(gameObject); //destroy the player object
        }

    }*/
}