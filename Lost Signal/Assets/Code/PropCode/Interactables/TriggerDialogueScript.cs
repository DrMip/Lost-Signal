using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDialogueScript : MonoBehaviour
{
    [SerializeField] bool multipleConversations;
    DialogueFunc dialogueFunc;
    HaltMovement halt;
    Animator anim;
    bool entered = false;

    private void Awake() 
    {
        dialogueFunc = GetComponent<DialogueFunc>();
        halt = GameObject.Find("Player").GetComponent<HaltMovement>();
        anim = GameObject.Find("Player").GetComponent<Animator>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(!entered)
        {
            Debug.Log("dialogue");
            entered = true;
            
            StartCoroutine(dialogueFunc.DialogueLoop());

            anim.Play("Player_Idle");
            halt.HaltAll();

            //register as finished onlly if we have multiple conversations
            if(multipleConversations)
                entered = false;
        }
    }

}
