using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class KeyPopUp : MonoBehaviour
{
    //Halt
    //HaltMovement halt;
    [SerializeField] GameObject keyToPopUp;
    [SerializeField] float height = 1.2f;
    //[SerializeField] GameObject securitycamsImage;
    GameObject KeyGameObject;
    bool entered;
    [NonSerialized] public bool keyPressed;
    // Start is called before the first frame update
    void Start()
    {
        //securitycamsImage.SetActive(false);
        //halt = GameObject.Find("Player").GetComponent<HaltMovement>();
    }

    private void OnTriggerEnter2D(Collider2D other)    
    {
        //create gameobject above interactable
        if(!entered)
        {
            Vector3 pos = transform.position + new Vector3(0, height, 0);
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
                //securitycamsImage.SetActive(true);
                //halt.HaltAll();
            }
            else
            {
                keyPressed = false;
                //securitycamsImage.SetActive(false);
                //halt.ResumeAll();
            }
            
        }
        
    }
    private void OnTriggerExit2D(Collider2D other) 
    {
        //destory the key gameobject when leaving the area
        if(entered)
        {
            Destroy(transform.GetChild(0).gameObject);
            entered = false;
        }

    }
}
