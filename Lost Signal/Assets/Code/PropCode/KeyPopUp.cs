using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class KeyPopUp : MonoBehaviour
{
    [SerializeField] GameObject keyToPopUp;
    [SerializeField] GameObject securitycamsImage;
    GameObject KeyGameObject;
    bool entered;
    bool keyPressed;
    // Start is called before the first frame update
    void Start()
    {
        securitycamsImage.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)    
    {
        if(!entered)
        {
            Vector3 pos = transform.position + new Vector3(0, +1.2f, 0);
            KeyGameObject = Instantiate(keyToPopUp , pos, quaternion.identity);
            KeyGameObject.name = "F Key";
            KeyGameObject.transform.parent = transform; 
            entered = true;
        }
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F) && entered)
        {
            if(keyPressed == false)
            {
                keyPressed = true;
                securitycamsImage.SetActive(true);
            }
            else
            {
                keyPressed = false;
                securitycamsImage.SetActive(false);
            }
            
        }
        
    }
    private void OnTriggerExit2D(Collider2D other) 
    {
        if(entered)
        {
            Destroy(transform.GetChild(0).gameObject);
            entered = false;
        }

    }
}
