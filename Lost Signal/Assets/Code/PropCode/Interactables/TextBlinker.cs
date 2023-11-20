using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBlinker : MonoBehaviour
{
    Text text;
    [SerializeField] Color color_1;
    [SerializeField] Color color_2;
    [SerializeField] int blinkRate;
    float blinkCounter = 0;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        //run from 0 to rate over and over
        if(blinkCounter < blinkRate)
        {
            blinkCounter += Time.deltaTime;
        }
        else
        {
            blinkCounter = 0;
        }
        //change colors
        if(blinkCounter > blinkRate/2)
        {
            text.color = color_1;
        }
        else
        {
            text.color = color_2;
        }


    }
}
