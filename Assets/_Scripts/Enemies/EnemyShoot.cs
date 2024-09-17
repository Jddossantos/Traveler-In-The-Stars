using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public GameObject projectilePrefab;     //enemy projectile prefab
    public float shootInterval = 2f;        //interval between shots
    public Transform shootPoint;            //Where the projectile will spawn from

    public Transform playerTarget;         //player target for projectiles

    // Start is called before the first frame update
    void Start()
    {
        //finding the player within the scene
        playerTarget = GameObject.FindGameObjectWithTag("PlayerShip").transform;

        //start shooting
        StartCoroutine(ShootAtPlayer());
    }

    void Update()
    {
        //finding the player within the scene
        playerTarget = GameObject.FindGameObjectWithTag("PlayerShip").transform;
    }

    IEnumerator ShootAtPlayer()
    {
        while (true)
        {
            //instantiating the projectile and set its direction towards the player
            GameObject enemyProjectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
            enemyProjectile.GetComponent<Rigidbody2D>().velocity = (playerTarget.position - shootPoint.position).normalized * 5;

            //waiting for the next shot
            yield return new WaitForSeconds(shootInterval);
        }
    }
}
