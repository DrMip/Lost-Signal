using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerDialogueScript : MonoBehaviour
{

    [SerializeField] bool multipleConversations;
    DialogueFunc dialogueFunc;
    HaltMovement halt;
    Animator anim;
    bool doDialogue = true;
    bool triggered = false;
    [SerializeField] float delayBetweenDialogues = 1f;
    //makes restarting dialogue only happens once
    bool restartDialogue = true;
    //conversation counter
    private int convCounter;

    //event
    bool LastFrameDialogue_running;
    [SerializeField] AudioManager.ThemeSetter ThemeSetter = new AudioManager.ThemeSetter();

    private void Awake() 
    {
        dialogueFunc = GetComponent<DialogueFunc>();
        halt = GameObject.Find("Player").GetComponent<HaltMovement>();
        anim = GameObject.Find("Player").GetComponent<Animator>();
    }
    void Start()
    {
        convCounter = dialogueFunc.conversationNum;
    }
    void Update()
    {
        if(triggered)
        {
            if(!dialogueFunc.dialogue_running)
            {
                //do dialogue
                if(doDialogue)
                {
                    doDialogue = false;
                    StartCoroutine(dialogueFunc.DialogueLoop());

                    anim.Play("Player_Idle");
                    halt.HaltAll();
                }
                //register as finished only if we have multiple conversations
                else if(multipleConversations && restartDialogue && convCounter > 1)
                {
                    restartDialogue = false;
                    Debug.Log("restart conversation");
                    Invoke("DelayStart", delayBetweenDialogues);
                    halt.HaltAll();
                }
            }
            
        }
        if (LastFrameDialogue_running && !dialogueFunc.dialogue_running && convCounter <= 1)//if at the end of dialogue
        {
            ThemeSetter.Send();
        }

        LastFrameDialogue_running = dialogueFunc.dialogue_running;
        //Debug.Log(convCounter);
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.name == "Player")
        {
            triggered = true;
        }
    }
    void DelayStart()
    {
        doDialogue = true;
        restartDialogue = true;
        convCounter -= 1;
        //Debug.Log(convCounter);
    }
    

}
