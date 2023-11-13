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
    //vars
    bool keywaspressed;


    void Start()
    {
        securityCamsImage.SetActive(false);
        halt = GameObject.Find("Player").GetComponent<HaltMovement>();
        anim = GameObject.Find("Player").GetComponent<Animator>();
        key = GetComponent<KeyPopUp>();
    }

    // Update is called once per frame
    void Update()
    {
        //get if key pressed
        if(key.keyPressed)
        {
            securityCamsImage.SetActive(true);
            anim.Play("Player_Idle");
            halt.HaltAll();
        }
        else if(keywaspressed && !key.keyPressed)
        {
            securityCamsImage.SetActive(false);
            halt.ResumeAll();
        }
        keywaspressed = key.keyPressed;
    }
}
