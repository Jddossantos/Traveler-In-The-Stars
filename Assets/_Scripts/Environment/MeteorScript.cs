using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class MeteorScript : MonoBehaviour
{
    //meteor variables
    private float travelSpeed = 5f;     //base speed of the meteors
    private float rotationSpeed = 0f;   //base rotation speed of the meteors
    public int meteorHealth = 20;       //base health of the meteors

    PlayerHealth playerHealth;

    //components
    private Rigidbody2D meteorRB;

    //enum to define Meteor types
    public enum MeteorType {Weak ,Medium ,Strong}
    public MeteorType meteorTypes;      //the different types of meteors

    // Start is called before the first frame update
    void Start()
    {
        //getting rigidbody component
        meteorRB = GetComponent<Rigidbody2D>();

        playerHealth = GetComponent<PlayerHealth>();

        //adjusting the health and speeds of each meteor typess
        //if any of these types of meteors spawn in then the health and speed will be set accordingly
        switch (meteorTypes)
        {
            case MeteorType.Weak:
                meteorHealth = 20;  //weak meteors have less health
                break;
            case MeteorType.Medium:
                meteorHealth = 30;  //medium size meteors have average/moderate health
                break;
            case MeteorType.Strong:
                meteorHealth = 40;  //strong meteors have the most health
                break;
        }

    }

    // Update is called once per frame
    void Update()
    {
        //moving the meteors downwards
        meteorRB.velocity = new Vector2(0, -travelSpeed);
        //transform.Translate(Vector3.down * travelSpeed * Time.deltaTime);

        //rotates the meteors based on the rotation speed applied
        meteorRB.angularVelocity = rotationSpeed;
        //transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

        //destroying the meteor game object if it leaves the camera border (goes off screen/ out of camera view)
        if (transform.position.y < -Camera.main.orthographicSize)
        {
            Destroy(gameObject);
        }
    }

    //method to set the speed of the meteors
    public void SetSpeed(float newSpeed)
    {
        travelSpeed = newSpeed;   //assigning the speed passed through the EnvironmentManager
    }

    //method to set the rotation speed of the meteors
    public void SetRotationSpeed(float newRotationSpeed)
    {
        rotationSpeed = newRotationSpeed; 
    }


    //method to apply damage to the meteors
    public void TakeDamage(int damage)
    {
        meteorHealth -= damage;     //reduce the meteor's health by the damage amount

        //checking if the meteor's health is depleted
        if (meteorHealth <= 0)
        {
            Destroy(gameObject);    //destroy the meteor if its health reaches 0
            GameManager.Instance.AddScore(5);   //when destroyed, add 5 to score
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        //check if the meteor collides with the players ship
        if (collision.gameObject.CompareTag("PlayerShip"))
        {
            //accessing the PlayerController component on the player
            PlayerController playerController = gameObject.GetComponent<PlayerController>();

            //applying damage to the player
            if (playerController != null)
            {
                playerHealth.TakeDamage(5);  //applying damage the player that is equal to the meteor's health
            }

            Destroy(gameObject);    //destroying the meteor after it hits the player
        }
    }
}
