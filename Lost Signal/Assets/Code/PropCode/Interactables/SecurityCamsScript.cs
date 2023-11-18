using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCamsScript : MonoBehaviour
{
    //keypressing
    KeyPopUp key;
    //Halting
    HaltMovement halt;
    //Doing what is intended
    [SerializeField] GameObject securityCamsImage;
    //animation for being Idle when in cams
    Animator anim;
    //dialogue funcs
    DialogueFunc diaFunc;
    //vars
    bool keywaspressed;
    bool dialogueApearOnce = true;
    bool invoking;


    void Start()
    {
        securityCamsImage.SetActive(false);
        halt = GameObject.Find("Player").GetComponent<HaltMovement>();
        anim = GameObject.Find("Player").GetComponent<Animator>();
        key = GetComponent<KeyPopUp>();
        diaFunc = GetComponent<DialogueFunc>();
    }

    // Update is called once per frame
    void Update()
    {
        //dont register keypressing if in dialogue
        if(key.keyPressed && (diaFunc.dialogue_running || invoking))
        {
            key.keyPressed = false;
        }
        //get if key pressed
        if(key.keyPressed && !diaFunc.dialogue_running && !invoking)
        {
            securityCamsImage.SetActive(true);
            anim.Play("Player_Idle");
            halt.HaltAll();
        }
        else if(keywaspressed && !key.keyPressed)
        {
            if(dialogueApearOnce)
            {
                dialogueApearOnce = false;
                invoking = true;
                Invoke("dialogLoopShell", 1f);
            }
            else
            {
                halt.ResumeAll();
            }
            securityCamsImage.SetActive(false);

        }
        keywaspressed = key.keyPressed;
    }
    void dialogLoopShell()
    {
        StartCoroutine(diaFunc.DialogueLoop());

        invoking = false;
    }
    
}
