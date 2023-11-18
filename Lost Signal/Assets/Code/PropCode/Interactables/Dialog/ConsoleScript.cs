using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleScript : MonoBehaviour
{
    //components for script
    HaltMovement halt;
    Animator anim;
    KeyPopUp key;
    DialogueFunc diaFunc;
    void Start()
    {
        //initialize components 
        halt = GameObject.Find("Player").GetComponent<HaltMovement>();
        anim = GameObject.Find("Player").GetComponent<Animator>();
        key = GetComponent<KeyPopUp>();
        diaFunc = GetComponent<DialogueFunc>();
    }

    // Update is called once per frame
    void Update()
    {
        //dont register keypressing if in dialogue
        if(key.keyPressed && diaFunc.dialogue_running)
        {
            key.keyPressed = false;
        }
        //get if key pressed
        if(key.keyPressed && !diaFunc.dialogue_running)
        {
            key.keyPressed = false;
            StartCoroutine(diaFunc.DialogueLoop());
            anim.Play("Player_Idle");
            halt.HaltAll();
            
        }
    }
}
