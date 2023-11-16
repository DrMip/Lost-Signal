using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DialogueScript : MonoBehaviour
{
    //essential Components for PopUp
    [Header("Dialogue Chracteristics")]
    [SerializeField] GameObject dialogueDisplay;
    HaltMovement halt;
    Animator anim;
    KeyPopUp key;
    //children of popUp
    [SerializeField] Image avatarRenderer;
    [SerializeField] Text nameObject;
    [SerializeField] Text textObject;
    [SerializeField] GameObject enterObject;
    //variables for functionality
    bool dialogue_running;



    //dialogue properties
    [Header("Specifics of Dialogue")]
    [SerializeField] Color PlayerColor;
    [SerializeField] Color OtherColor;
    [SerializeField] string PlayerName;
    [SerializeField] string OtherName;
    [SerializeField] Sprite playerSprite;
    [SerializeField] Sprite OtherSprite;
    
    



    //--------------------
    //Dialogue
    //--------------------
    List<DialogueEntity> dialogueCharacteristicsList = new List<DialogueEntity>();
    
    [Tooltip("# is player, $ is other")]
    public string[] texts = {};




    //create dialogue list
    void Awake()
    {
        //initialize dialogues
        foreach(string text in texts)
        {
            if(text[0] == '#')
            {
                DialogueEntity tempDialogue = ScriptableObject.CreateInstance<DialogueEntity>();
                string trueText = text.Trim('#');
                //create
                tempDialogue.avatar = playerSprite;
                tempDialogue.speakerName = PlayerName + ':';
                tempDialogue.speakerColor = PlayerColor;
                tempDialogue.Speech = trueText;
                //append
                dialogueCharacteristicsList.Add(tempDialogue);
            }
            else if(text[0] == '$')
            {
                DialogueEntity tempDialogue = ScriptableObject.CreateInstance<DialogueEntity>();
                string trueText = text.Trim('$');
                //create
                tempDialogue.avatar = OtherSprite;
                tempDialogue.speakerName = OtherName + ':';
                tempDialogue.speakerColor = OtherColor;
                tempDialogue.Speech = trueText;
                //append
                dialogueCharacteristicsList.Add(tempDialogue);
            }
            else
            {
                Debug.Log("forgot # or $ in dialogue");
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        dialogueDisplay.SetActive(false);
        halt = GameObject.Find("Player").GetComponent<HaltMovement>();
        anim = GameObject.Find("Player").GetComponent<Animator>();
        key = GetComponent<KeyPopUp>();
    }

    // Update is called once per frame
    void Update()
    {
        //get if key pressed
        if(key.keyPressed && !dialogue_running)
        {
            key.keyPressed = false;
            StartCoroutine(DialogueLoop());
            anim.Play("Player_Idle");
            halt.HaltAll();
            
        }
    }
    IEnumerator DialogueLoop()
    {
        dialogue_running = true;
        dialogueDisplay.SetActive(true);
        //dialogue loop
        foreach(DialogueEntity line in dialogueCharacteristicsList)
        {
            //clear
            textObject.text = "";
            nameObject.text = "";
            enterObject.SetActive(false); 
            //Add Avatar
            avatarRenderer.sprite = line.avatar;
            //if speaker is other then make avater black
            if(line.speakerName == OtherName + ':')
            {
                avatarRenderer.color = Color.black;
            }
            else
            {
                avatarRenderer.color = Color.white;
            }
            //wait
            yield return new WaitForSeconds(0.5f);
            //add name
            nameObject.text = line.speakerName;
            nameObject.color = line.speakerColor;
            //wait
            yield return new WaitForSeconds(0.25f);
            //add text
            textObject.text = line.Speech;
            //wait for read
            yield return new WaitForSeconds(1f);  
            //Add enter option
            enterObject.SetActive(true); 

            //wair for enter
            while(!Input.GetKeyDown(KeyCode.Return))  
            {
                yield return null;
            }
            //when hit enter does loop again

        }
        
        //when finishes dialogue disappear and continue movement
        dialogueDisplay.SetActive(false);
        halt.ResumeAll();
        dialogue_running = false;
    }
}
