using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileScript : MonoBehaviour
{
    //speed and lifeTime variables
    public float projectileSpeed = 5f;      //speed at which the projectile travels
    public float lifeTime = 3f;             //the amount of time (in seconds) before the projectile is destroyed

    //damage
    public int projectileDamage = 25;       //the amount of damage the projectile will deal to enemies

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifeTime);  //destroys the projectile after the 'lifeTime' seconds to prevent it from lingering in the scene indefinitely
        //Debug.Log("EnemyProjectile: Start - Projectile initialized with speed " + projectileSpeed + " and lifetime " + lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        //moving the projectile upwards along the y-axis at the specified speed
        transform.Translate(Vector3.down * projectileSpeed * Time.deltaTime);
        //Debug.Log("EnemyProjectile: Update - Projectile moving downwards");
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("EnemyProjectile: OnCollisionEnter2D - Collsion with " + collision.gameObject.name);

        //checking if the projectile collides with an object tagged as "PlayerShip"
        if (collision.gameObject.CompareTag("PlayerShip"))
        {
            //attempting to get the PlayerController for projectile ccollision
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();

            //if the player has a PlayerController, reduce it's health by the damage amount specified variable
            if (playerController != null)
            {
                GameManager.Instance.TakeDamage(projectileDamage);
            }
            Destroy(gameObject);        //destroy the projectile up collsion
        }

        //checking if the projectile collides with a meteor instead
        else if (collision.gameObject.CompareTag("Meteors"))
        {
            Destroy(gameObject);            //destroys the projectile after it collides with the meteor to prevent further collisions
        }
    }
}
