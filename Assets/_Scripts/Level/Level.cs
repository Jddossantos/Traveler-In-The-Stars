using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public PlayerSpawn playerSpawn;
    public Transform playerStart;

    // Start is called before the first frame update
    void Start()
    {
        playerSpawn.SpawnPlayer(playerStart);
    }
}
