using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBackground : MonoBehaviour
{
    public GameObject[] backgrounds;    //array of background GameObjects for continued use of scrolling effect
    public float scrollSpeed = 8f;      //Speed at which the background scrolls
    private float backgroundHeight;     //Height of the background sprite
    private float backgroundWidth;      //weidth of the background sprite

    private Vector2 startPosition;      //initial position of the background

    // Start is called before the first frame update
    void Start()
    {
        //calculating the backgrounds sprite width and height
        backgroundHeight = backgrounds[0].GetComponent<SpriteRenderer>().bounds.size.y;
        backgroundWidth = backgrounds[0].GetComponent<SpriteRenderer>().bounds.size.x;

        //resizing the backgrounds to fit the screen width and maintain aspect ratio
        ResizeBackgrounds();

        //position the backgrounds in sequence vertically
        for (int i = 0; i < backgrounds.Length; i++)
        {
            backgrounds[i].transform.position = new Vector2(0, i * backgroundHeight);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //moving each background downwards
        for (int i = 0; i < backgrounds.Length; i++)
        {
            backgrounds[i].transform.Translate(Vector2.down * scrollSpeed * Time.deltaTime);

            //if the background is out of view, reposition it to the top
            if (backgrounds[i].transform.position.y <= -backgroundHeight)
            {
                RepositionBackground(backgrounds[i]);       //function for repositioning backgrounds
            }
        }
    }

    void RepositionBackground(GameObject background)
    {
        //finding the highest background
        float highestYPosition = backgrounds[0].transform.position.y;

        foreach (GameObject bg in backgrounds)
        {
            if (bg.transform.position.y > highestYPosition)
            {
                highestYPosition = bg.transform.position.y;
            }
        }

        //positioning the background above the highest background
        background.transform.position = new Vector2(0, highestYPosition + backgroundHeight);
    }

    void ResizeBackgrounds()
    {
        //getting the screen size in world units
        float screenHeight = Camera.main.orthographicSize * 2.0f;
        float screenWidth = screenHeight * Camera.main.aspect;

        //resizing each background to match the screen width and maintain aspect ration
        foreach (GameObject background in backgrounds)
        {
            Vector3 newScale = background.transform.localScale;
            newScale.x *= screenWidth / backgroundWidth;    //adjusting width to match screen
            newScale.y *= screenHeight / backgroundHeight;  //adjusting height to match screen
            background.transform.localScale = newScale;
        }
    }
}

