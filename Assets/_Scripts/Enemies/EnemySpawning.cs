using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawning : MonoBehaviour
{
    //enemy and boss spawning variables and flags
    public GameObject[] enemyPrefabs;       //array for enemy prefabs for spawning in
    public GameObject bossPrefab;           //boss object pre fab for spawning in at certain timed events
    public Transform[] enemySpawnPoints;    //aaray for enemy spawns

    public float spawnInterval = 5f;       //interval for enemy spawns
    public float bossSpawnTime = 120f;      //time for when boss spawn

    public bool bossSpawned = false;        //flag to check if the boss has spawned in
    public bool canEnemiesSpawn = true;     //flag to check if enemies can spawn

    private BossController bossController;

    private Coroutine spawnEnemies;
    private Coroutine bossCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        spawnEnemies = StartCoroutine(SpawnEnemies());  //start spawning enemies   
        bossCoroutine = StartCoroutine(BossSpawnTimer());               //start timer for the boss
    }

    // Update is called once per frame
    void Update()
    {
        /*if (bossSpawned && canEnemiesSpawn)
        {
            Debug.Log("Boss has spawned. Stopping enemy spawning...");
            canEnemiesSpawn = false;
            StopCoroutine(spawnEnemies);
        }

        if (bossSpawned && bossController != null && bossController.bossHealth <= 0)
        {
            Debug.Log("Enemies respawn, Boss defeated");
            bossSpawned  = false;
            canEnemiesSpawn = true;

            if (spawnEnemies == null)
            {
                spawnEnemies = StartCoroutine(SpawnEnemies());
            }
        }*/
    }

    #region Enemies and Boss Management
    //method to spawn enemies
    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            if (canEnemiesSpawn)
            {
                //choosing a random enemy within the array to spawn
                int randomEnemyIndex = Random.Range(0, enemyPrefabs.Length);

                //choosing a random spawn point to spawn enemies
                int randomSpawnPointIndex = Random.Range(0, enemySpawnPoints.Length);

                //instantiate the enemy at the chosen spawn points
                Instantiate(enemyPrefabs[randomEnemyIndex], enemySpawnPoints[randomSpawnPointIndex].position, Quaternion.identity);

                Debug.Log("Enemy has spawned");
            }
            //wait for next spawn with spawn interval float
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    //method for boss timer
    IEnumerator BossSpawnTimer()
    {
        //waiting for the specified boss time
        yield return new WaitForSeconds(bossSpawnTime);

        //spawning the boss
        SpawnBoss();

        //setting the flag to stop regular enemy spawns while the boss is active
        bossSpawned = true;
        canEnemiesSpawn = false;
    }

    //method to spawn boss
    void SpawnBoss()
    {
        //choosing a random spawn point to spawn enemies
        int randomSpawnPointIndex = Random.Range(0, enemySpawnPoints.Length);

        //instantiate the enemy at the chosen spawn points
        GameObject boss = Instantiate(bossPrefab, enemySpawnPoints[randomSpawnPointIndex].position, Quaternion.identity);

        bossController = boss.GetComponent<BossController>();

        //debugs for checking if the boss controller is actually being called
        if (bossController == null)
        {
            Debug.Log("Boss Controller is null!");
        }
        else
        {
            Debug.Log("Boss Controller is not null");
        }
    }

    public void OnBossDefeated()
    {
        
        //Debug.Log("EnemySpawning: Boss Defeated, resuming enemy spawning");

        //reseting the flags to resume normal play
        bossSpawned = false;
        canEnemiesSpawn = true;

        //restart the enemy spawn coroutine if it isn't running
        if (spawnEnemies == null)
        {
            spawnEnemies = StartCoroutine(SpawnEnemies());
        }

        bossCoroutine = StartCoroutine(BossSpawnTimer());
    }
    #endregion
}
