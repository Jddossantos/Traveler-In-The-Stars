using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    //player projectile variables and components
    public GameObject playerProjectile;         //the projectile prefab to be instantiated
    public Transform playerProjectileSpawn;     //the spawnpoint for the projectile
    public float projectileDamage = 100f;       //the damage of eacch projectile
    public float fireRate = 0.1f;               //the rate at which the player fires projectiles

    private bool isShooting = false;            //flag to check if the player is currently shooting
    private float nextFireTime = 0f;            //time until next shot fired

    public AudioClip fireShotClip;              //audio clip for firing a lazer

    void OnEnable()
    {
        //subsribing to touch events from InputManager
        InputManager.OnStartTouch += StartShooting;
        InputManager.OnEndTouch += StopShooting;
        //Debug.Log("PlayerShoot: OnEnable - Subsribed to InputManager touch events");
    }

    void OnDisable()
    {
        //unsubsribing to touch events from InputManager when this script is disabled
        InputManager.OnStartTouch -= StartShooting;
        InputManager.OnEndTouch -= StopShooting;
        //Debug.Log("PlayerShoot: OnDisable - Unsubsribed to InputManager touch events");
    }

    private void StartShooting(Vector2 position,float time)
    {
        //start shooting when the player touches the screen
        isShooting = true;          //sets isShooting flag to true when the players finger is on the screen
        //Debug.Log("PlayerShoot: StartShooting - Shots fired at time: " + time);
    }

    private void StopShooting(Vector2 position, float time)
    {
        //stops shooting when the players stops touching the screen
        isShooting = false;         //sets isShooting flag to false when the players finger is off the screen
        //Debug.Log("PlayerShoot: StopShooting - Shooting stoped at time: " + time);
    }

    // Update is called once per frame
    void Update()
    {
        //checking if the player is shooting and the time between shots
        if (isShooting && Time.time >= nextFireTime)
        {
            //firing the projectile
            Shoot();

            //setting the time for the next shot
            nextFireTime = Time.time + fireRate;
            //Debug.Log("PlayerShoot: Update - Projectile Fired");
        }
    }

    private void Shoot()
    {
        //Instantiate the projectile at the fire point's position and rotation
        Instantiate(playerProjectile, playerProjectileSpawn.position, playerProjectileSpawn.rotation);

        AudioManager.instance.PlaySFX(fireShotClip);
        //Debug.Log("PlayerShoot: Shoot - Projectile Instantiated");
    }
}
