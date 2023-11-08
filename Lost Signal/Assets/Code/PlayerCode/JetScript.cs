using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetScript : MonoBehaviour
{
    //player behavior
    PlayerBehavior pb;
    Rigidbody2D rb;
    PlayerMovement plm;
    //private float JetStrength;
    //Variables
    bool jetPressed;
    bool lastJetPressed;

    // Start is called before the first frame update
    void Start()
    {
        pb = GetComponent<PlayerBehavior>();
        rb = GetComponent<Rigidbody2D>();
        plm = GetComponent<PlayerMovement>();
        //JetStrength = pb.JetPackStrength;
        pb.JetTimeCounter = pb.JetTime;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetButton("Jetpack"))
        {
            jetPressed = true;
            if(pb.JetTimeCounter < 0)
                pb.JetTimeCounter = 0;
            if(pb.JetTimeCounter > 0)
            {
                pb.JetTimeCounter -= Time.deltaTime;
            }
        }
        else
        {
            jetPressed = false;
            if(pb.JetTimeCounter > pb.JetTime)
                pb.JetTimeCounter = pb.JetTime;
            if(pb.JetTimeCounter < pb.JetTime  && plm.isGrounded)
            {
                pb.JetTimeCounter += pb.JetRecoverRatio*Time.deltaTime;
            }
        }
        if(lastJetPressed == false && jetPressed == true)
        {
            rb.velocity = Vector2.up*2;
        }
        lastJetPressed = jetPressed;


    }

    void FixedUpdate()
    {
        if(jetPressed && pb.JetTimeCounter > 0)
        {
            transform.position += new Vector3(0 , pb.JetPackStrength * 0.1f, 0);
        }
    }

}
