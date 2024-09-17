using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    public PlayerController playerPrefab;           //reference to the player prefab
    public Transform spawnLocation;                 //spawn location for the player

    /// <summary>
    /// Spawns the player at the designated spawn location
    /// </summary>
    public void SpawnPlayer(Transform spawn)
    {

        //instantiate the player prefab at the given spawn location
        Instantiate(playerPrefab, spawn.position, spawn.rotation);
        Debug.Log("Player spawned at: " + spawn.position);
    }
}
