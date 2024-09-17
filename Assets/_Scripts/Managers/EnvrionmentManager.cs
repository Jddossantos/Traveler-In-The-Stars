using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvrionmentManager : MonoBehaviour
{
    //prefabs for the different type of meteors
    public GameObject[] weakMeteors;
    public GameObject[] mediumMeteors;
    public GameObject[] strongMeteors;

    //time for spawns
    public float spawnInterval = 4f;

    //variables for speed and rotation speed
    public float minSpeed = 0.5f;
    public float maxSpeed = 2.0f;
    public float minRotationSpeed = -100.0f;
    public float maxRotationSpeed = 100.0f;

    // Start is called before the first frame update
    void Start()
    {
        //start spawning meteors continuously
        StartCoroutine(SpawnMeteors());
    }

    //coroutine that spawns the different meteors at regular intervals
   IEnumerator SpawnMeteors()
    {
        while (true)
        {
            //choosing a random meteor type to spawn
            int meteorTypes = Random.Range(0, 3);

            //selecting the correct meteor array based on the random type
            GameObject[] selectedMeteorArray = weakMeteors;     //default to weak meteors array

            switch (meteorTypes)
            {
                case 0:
                    selectedMeteorArray = weakMeteors;
                    break;
                case 1:
                    selectedMeteorArray = mediumMeteors;
                    break;
                case 2:
                    selectedMeteorArray = strongMeteors;
                    break;
            }

            //randomly select a meteor prefab from the selected array
            GameObject meteorPrefab = selectedMeteorArray[Random.Range(0, selectedMeteorArray.Length)];

            //determining a random spawn position at the top of the screen
            float cameraWidth = Camera.main.orthographicSize * Camera.main.aspect;
            Vector3 spawnPosition = new Vector3(Random.Range(-Camera.main.orthographicSize * Camera.main.aspect, Camera.main.orthographicSize * Camera.main.aspect), Camera.main.orthographicSize + 1, 0);
            
            //instantiating the meteors
            GameObject meteor = Instantiate (meteorPrefab, spawnPosition, Quaternion.identity);

            //assigning random speed and rotation to the meteors
            float speed = Random.Range(minSpeed, maxSpeed);
            float rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);

            //applying these new settings to the meteors
            MeteorScript meteorScript = meteor.GetComponent<MeteorScript>();
            if (meteorScript != null)
            {
                meteorScript.SetSpeed(speed);
                meteorScript.SetRotationSpeed(rotationSpeed);
            }

            //wait for the next spawn
            yield return new WaitForSeconds(spawnInterval);

        }
    }
}
