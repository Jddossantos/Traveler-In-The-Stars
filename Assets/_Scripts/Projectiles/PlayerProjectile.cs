using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    //speed and lifeTime variables
    public float projectileSpeed = 10f;     //speed at which the projectile travels
    public float lifeTime = 1f;             //the amount of time (in seconds) before the projectile is destroyed

    private Rigidbody2D projectileRB;

    //damage
    public int projectileDamage = 25;       //the amount of damage the projectile will deal to enemies

    // Start is called before the first frame update
    void Start()
    {
        //getting rigidbody component
        projectileRB = GetComponent<Rigidbody2D>();

        Destroy(gameObject, lifeTime);  //destroys the projectile after the 'lifeTime' seconds to prevent it from lingering in the scene indefinitely
        //Debug.Log("PlayerProjectile: Start - Projectile initialized with speed " + projectileSpeed + " and lifetime " +  lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        //moving the projectile upwards along the y-axis at the specified speed
        transform.Translate(Vector3.up * projectileSpeed * Time.deltaTime);
        //Debug.Log("PlayerProjectile: Update - Projectile moving upwards");
    }

    public void OnCollisionEnter2D (Collision2D collision)
    {
        //Debug.Log("PlayerProjectile: OnCollisionEnter2D - Collsion with " + collision.gameObject.name);
        
        //checking if the projectile collides with an object tagged as "Enemy"
        if (collision.gameObject.CompareTag("EnemiesTier1"))
        {
            //attempting to get the EnemyController script from the collided enemy object
            EnemyController enemyController = collision.gameObject.GetComponent<EnemyController>();

            //if the enemy has a EnemyController, reduce it's health by the damage amount specified variable, i.e it applys the damage to the enemy
            if (enemyController != null)
            {
                enemyController.TakeDamage(projectileDamage);   //calling the take damage method on the EnemyController
            }
            Destroy(gameObject);            //destroy the projectile after it collides with the enemy to prevent further collisions
        }

        //checking if the projectile collides with an object tagged as "Enemy"
        else if (collision.gameObject.CompareTag("EnemiesTier2"))
        {
            //attempting to get the EnemyController script from the collided enemy object
            EnemyController enemyController = collision.gameObject.GetComponent<EnemyController>();

            //if the enemy has a EnemyController, reduce it's health by the damage amount specified variable, i.e it applys the damage to the enemy
            if (enemyController != null)
            {
                enemyController.TakeDamage(projectileDamage);   //calling the take damage method on the EnemyController
            }
            Destroy(gameObject);            //destroy the projectile after it collides with the enemy to prevent further collisions
        }

        //checking if the projectile collides with an object tagged as "Enemy"
        else if (collision.gameObject.CompareTag("EnemiesTier3"))
        {
            //attempting to get the EnemyController script from the collided enemy object
            EnemyController enemyController = collision.gameObject.GetComponent<EnemyController>();

            //if the enemy has a EnemyController, reduce it's health by the damage amount specified variable, i.e it applys the damage to the enemy
            if (enemyController != null)
            {
                enemyController.TakeDamage(projectileDamage);   //calling the take damage method on the EnemyController
            }
            Destroy(gameObject);            //destroy the projectile after it collides with the enemy to prevent further collisions
        }

        //checking if the projectile collides with an object tagged as "Enemy"
        else if (collision.gameObject.CompareTag("EnemiesTier4"))
        {
            //attempting to get the EnemyController script from the collided enemy object
            EnemyController enemyController = collision.gameObject.GetComponent<EnemyController>();

            //if the enemy has a EnemyController, reduce it's health by the damage amount specified variable, i.e it applys the damage to the enemy
            if (enemyController != null)
            {
                enemyController.TakeDamage(projectileDamage);   //calling the take damage method on the EnemyController
            }
            Destroy(gameObject);            //destroy the projectile after it collides with the enemy to prevent further collisions
        }

        //checking if the projectile collides with a meteor instead
        else if (collision.gameObject.CompareTag("Meteors"))
        {
            //access the meteor script on the meteor
            MeteorScript meteor = collision.gameObject.GetComponent<MeteorScript>();

            //apply damage to the meteor
            if (meteor != null)
            {
                meteor.TakeDamage(projectileDamage);
            }
            Destroy(gameObject);            //destroys the projectile after it collides with the meteor to prevent further collisions
        }

        else if (collision.gameObject.CompareTag("EnemyProjectile"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
