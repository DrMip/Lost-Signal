using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStateChanger : MonoBehaviour
{
    //animator
    public Animator ani;

    bool entered;
    GameObject rooms;

    void Start()
    {
        rooms = GameObject.Find("Rooms Triggers");
    }

    // Start is called before the first frame update

    void OnTriggerStay2D(Collider2D other)
    {
        if(!entered)
        {
            entered = true;
            
            //Debug.Log("entered" + other.gameObject);
            //check all children of game object rooms
            for(int i = 1; i <= rooms.transform.childCount; i++)
            {
                
                if (other.gameObject.name == rooms.transform.GetChild(i-1).gameObject.name)
                {
                    //Debug.Log("Room " + i);
                    ani.Play("Room " + i);
                    break;
                }
                

            }


        }
    }
    private void OnTriggerExit2D(Collider2D other) 
    {
        entered = false;
    }


}
