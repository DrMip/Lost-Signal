using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScene : MonoBehaviour
{
    GameObject image;
    // Start is called before the first frame update
    void Start()
    {
        image = transform.GetChild(0).gameObject;
        image.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.name == "Player")
        {
            image.SetActive(true);
        }
    }
}
