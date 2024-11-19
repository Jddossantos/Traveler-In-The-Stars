using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShoot : MonoBehaviour
{
    public GameObject projectilePrefab;     //enemy projectile prefab
    public float shootInterval = 2f;        //interval between shots

    //Where the projectiles will spawn from
    public Transform shootPoint;            
    public Transform shootPoint2;           

    public Transform playerTarget;         //player target for projectiles

    public AudioClip lazerClip;

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
            GameObject enemyProjectile2 = Instantiate(projectilePrefab, shootPoint2.position, Quaternion.identity);
            enemyProjectile.GetComponent<Rigidbody2D>().velocity = (playerTarget.position - shootPoint.position).normalized * 5;
            enemyProjectile2.GetComponent<Rigidbody2D>().velocity = (playerTarget.position - shootPoint2.position).normalized * 5;
            AudioManager.instance.PlaySFX(lazerClip);

            //waiting for the next shot
            yield return new WaitForSeconds(shootInterval);
        }
    }
}
