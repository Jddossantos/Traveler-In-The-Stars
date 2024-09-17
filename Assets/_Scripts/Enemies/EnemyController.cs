using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //enemy variables
    public float enemyHealth = 50f;
    public float moveSpeed = 2f;
    public float stopYPosition = 3f;
    [SerializeField] int scorePerEnemyTierOne = 15;

    //for tracking the player
    public Transform playerTarget;
    public float trackingSpeed;

    //components
    private Rigidbody2D enemyRB;

    // Start is called before the first frame update
    void Start()
    {
        //getting rigidbody component
        enemyRB = GetComponent<Rigidbody2D>();

        //finding the player by tag
        playerTarget = GameObject.FindGameObjectWithTag("PlayerShip").transform;
    }

    // Update is called once per frame
    void Update()
    {
        //moving the enemy downward until it reaches its max y position
        if (transform.position.y > stopYPosition)
        {
            transform.Translate(Vector2.down * moveSpeed *  Time.deltaTime);
        }
        else
        {
            //tracking the players horizontal position
            TrackPlayer();
        }
    }

    public void TrackPlayer()
    {
        if (playerTarget != null)
        {
            //calculating the direction towards the player's current x position
            Vector2 targetPosition = new Vector2(playerTarget.transform.position.x, playerTarget.transform.position.y);
            Vector2 newPosition = Vector2.MoveTowards(transform.position, targetPosition, trackingSpeed * Time.deltaTime);
            transform.position = new Vector3(newPosition.x, transform.position.y, transform.position.z);
        }
    }

    public void TakeDamage(int projectileDamage)
    {
        //subtracting the incoming damage from the player's current health
        enemyHealth -= projectileDamage;
        Debug.Log($"EnemyController: TakeDamage - Enemy took {projectileDamage} damage, current enemy health: {enemyHealth}");

        //checking if the enemys's health is zero or less
        if (enemyHealth <= 0)
        {
            Debug.Log("EnemyController: TakeDamage - Enemy is dead");
            EnemyDeath();
        }
    }

    private void EnemyDeath()
    {
        Destroy(gameObject);
        GameManager.Instance.AddScore(scorePerEnemyTierOne);  //when destroyed, add 10 to score
    }
}
