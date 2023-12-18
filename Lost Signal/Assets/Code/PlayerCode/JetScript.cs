using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetScript : MonoBehaviour
{
    //player behavior
    PlayerBehavior pb;
    Rigidbody2D rb;
    PlayerMovement plm;
    JumpScript jmp;
    [SerializeField] BarFuncs bar;
    //private float JetStrength;
    //Variables
    bool jetPressed;
    bool lastJetPressed;

    bool canJet = true;
    bool enableRunning;
    [NonSerialized] public bool jetting;

    // Start is called before the first frame update
    void Start()
    {
        pb = GetComponent<PlayerBehavior>();
        rb = GetComponent<Rigidbody2D>();
        plm = GetComponent<PlayerMovement>();
        jmp = GetComponent<JumpScript>();
        //JetStrength = pb.JetPackStrength;
        pb.JetTimeCounter = pb.JetTime;
        //jet max value
        bar.SetMaxJet(pb.JetTime);
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetButton("Jetpack") && !jmp.Jump && canJet)
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
        if (Input.GetButtonUp("Jetpack"))
        {
            canJet = false;
            //enable jet if not pressed
            if (!canJet && !enableRunning) StartCoroutine(EnableJet());
        }
        if (lastJetPressed == false && jetPressed == true)
        {
            rb.velocity = Vector2.up*2;
        }
        lastJetPressed = jetPressed;
        bar.SetJet(pb.JetTimeCounter);


    }

    void FixedUpdate()
    {
        if (jetPressed && pb.JetTimeCounter > 0)
        {
            transform.position += new Vector3(0, pb.JetPackStrength * 0.1f, 0);
            jetting = true;

        }
        else
            jetting = false;
    }

    IEnumerator EnableJet()
    {
        enableRunning = true;
        yield return new WaitForSeconds(pb.JetCooldown);
        canJet = true;
        enableRunning = false;
    }

}
