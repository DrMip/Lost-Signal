using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DialogueFunc : MonoBehaviour
{
    //essential Components for PopUp
    [Header("Dialogue Chracteristics")]
    [SerializeField] GameObject dialogueDisplay;
    HaltMovement halt;
    //children of popUp
    [SerializeField] Image avatarRenderer;
    [SerializeField] Text nameObject;
    [SerializeField] Text textObject;
    [SerializeField] GameObject enterObject;
    //variables for functionality
    [NonSerialized] public bool dialogue_running = false;

    int conversationIndex = 1;

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
    //conversations list
    List<List<DialogEntity>> conversationsList = new List<List<DialogEntity>>();
    //dialogue line structure
    public struct DialogEntity
    {
        //
        public Sprite avatar;
        public string speakerName;
        public Color speakerColor;
        // All above are corrolated
        public string Speech;

    }






    //create conversation hierarchy, 
    [Serializable] public struct Conversation
    {
        public string name;
        public string[] lines;
    }
    [Tooltip("# is player, $ is other")]
    [SerializeField] Conversation[] conversations = new Conversation[1];




    //create dialogue list
    void Awake()
    {
        //disable dialog
        dialogueDisplay.SetActive(false);
        //initialize conversation structure
        foreach(Conversation conversation in conversations)
        {  
            //in each conversation there are multiple lines
            List<DialogEntity> tempCharaList = new List<DialogEntity>();
            foreach(string line in conversation.lines)
            {
                if(line[0] == '#')
                {
                    DialogEntity tempDialogue = new DialogEntity();
                    string trueText = line.Trim('#');
                    //create
                    tempDialogue.avatar = playerSprite;
                    tempDialogue.speakerName = PlayerName + ':';
                    tempDialogue.speakerColor = PlayerColor;
                    tempDialogue.Speech = trueText;
                    //append
                    tempCharaList.Add(tempDialogue);
                }
                else if(line[0] == '$')
                {
                    DialogEntity tempDialogue = new DialogEntity();
                    string trueText = line.Trim('$');
                    //create
                    tempDialogue.avatar = OtherSprite;
                    tempDialogue.speakerName = OtherName + ':';
                    tempDialogue.speakerColor = OtherColor;
                    tempDialogue.Speech = trueText;
                    //append
                    tempCharaList.Add(tempDialogue);
                }
                else
                {
                    Debug.Log("forgot # or $ in dialogue");
                }
            }
            conversationsList.Add(tempCharaList);
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        halt = GameObject.Find("Player").GetComponent<HaltMovement>();
    }
    public IEnumerator DialogueLoop()
    {
        dialogue_running = true;
        dialogueDisplay.SetActive(true);
        //dialogue loop
        foreach(DialogEntity line in conversationsList[conversationIndex-1])
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
        //Continue to next conversation
        if(conversationIndex + 1 <= conversationsList.Count)
        {
            conversationIndex ++;
        }
        //when finishes dialogue disappear and continue movement
        dialogueDisplay.SetActive(false);
        halt.ResumeAll();
        dialogue_running = false;
    }
}
