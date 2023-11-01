using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStateChanger : MonoBehaviour
{
    //animator
    public Animator ani;
    //rooms
    public Collider2D Room1;
    public Collider2D Room2;
    public Collider2D Room3;
    // Start is called before the first frame update

    void OnTriggerStay2D(Collider2D other)
    {
        //Debug.Log("entered" + other.gameObject);
        if (other == Room1)
            ani.Play("Room 1");
        else if (other == Room2)
            ani.Play("Room 2");
        else if (other == Room3)
            ani.Play("Room 3");
    }


}
