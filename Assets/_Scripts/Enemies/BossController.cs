using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    //boss variables
    public float bossHealth = 300f;
    public float moveSpeed = 2f;
    public float stopYPosition = 3f;
    [SerializeField] int bossScoreAdd = 150;

    //for tracking the player
    public Transform playerTarget;
    public float trackingSpeed;

    //components
    private Rigidbody2D bossRB;

    // Start is called before the first frame update
    void Start()
    {
        //getting rigidbody component
        bossRB = GetComponent<Rigidbody2D>();

        //finding the player by tag
        playerTarget = GameObject.FindGameObjectWithTag("PlayerShip").transform;
    }

    // Update is called once per frame
    void Update()
    {
        //moving the enemy downward until it reaches its max y position
        if (transform.position.y > stopYPosition)
        {
            transform.Translate(Vector2.down * moveSpeed * Time.deltaTime);
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
        bossHealth -= projectileDamage;
        //Debug.Log($"BossController: TakeDamage - Boss took {projectileDamage} damage, current Boss health: {bossHealth}");

        //checking if the enemys's health is zero or less
        if (bossHealth <= 0)
        {
            Debug.Log("BossController: TakeDamage - Boss is dead");
            BossDeath();
        }
    }

    private void BossDeath()
    {
        //Debug.Log("BossController: Boss has been defeated.");

        //notify the EnemySpawning script that the boss is defeated
        EnemySpawning enemySpawning = FindAnyObjectByType<EnemySpawning>();
        if (enemySpawning != null)
        {
            enemySpawning.OnBossDefeated();
        }

        GameManager.Instance.AddScore(bossScoreAdd);  //when destroyed, add boss score
        Destroy(gameObject);    //destroy boss
    }
}
