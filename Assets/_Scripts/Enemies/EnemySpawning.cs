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

    private bool bossSpawned = false;       //flag to check if the boss has spawned in

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemies());             //start spawning enemies   
        StartCoroutine(BossSpawnTimer());           //start timer for the boss
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Enemies and Boss Management
    //method to spawn enemies
    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            //if the boss has spawned, stop spawning enemies
            if (bossSpawned)
                yield break;

            //choosing a random enemy within the array to spawn
            int randomEnemyIndex = Random.Range(0, enemyPrefabs.Length);

            //choosing a random spawn point to spawn enemies
            int randomSpawnPointIndex = Random.Range(0, enemySpawnPoints.Length);

            //instantiate the enemy at the chosen spawn points
            Instantiate(enemyPrefabs[randomEnemyIndex], enemySpawnPoints[randomSpawnPointIndex].position, Quaternion.identity);

            //wait for next spawn
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

        //setting the flag to stop regular enemy spawns
        bossSpawned = true;
    }

    //method to spawn boss
    void SpawnBoss()
    {
        //choosing a random spawn point to spawn enemies
        int randomSpawnPointIndex = Random.Range(0, enemySpawnPoints.Length);

        //instantiate the enemy at the chosen spawn points
        Instantiate(bossPrefab, enemySpawnPoints[randomSpawnPointIndex].position, Quaternion.identity);
    }
    #endregion
}
