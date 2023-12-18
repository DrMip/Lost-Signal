using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    //player Behavior
    PlayerBehavior pb;
    //rigid body
    Rigidbody2D rb;
    //halting
    HaltMovement halt;
    //variables
    float xdirect;
    bool pressedDash;
    [NonSerialized] public bool isDashing;
    [NonSerialized] public bool canDash = true;
    // Start is called before the first frame update
    void Start()
    {
        pb = GetComponent<PlayerBehavior>();
        rb = GetComponent<Rigidbody2D>();
        halt = GetComponent<HaltMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        //if pressed dash, and not already dashing, say is dashing and get dash direction
        if(Input.GetButtonDown("Dash") && canDash)
        {
            pressedDash = true;
            xdirect = (transform.localScale.x > 0) ? 1: (-1);
            
        }

    }
    void FixedUpdate()
    {
        //if pressed Start DoDash coroutine (freeze y movement)
        if(pressedDash)
        {

            StartCoroutine(DoDash());
            pressedDash = false;
        }
        if(isDashing)
        {
            transform.position += new Vector3(xdirect*pb.DashStrength*0.1f, 0, 0);
        }
    }
    IEnumerator DoDash()
    {
        canDash = false;
        isDashing = true;
        //Play Sound
        AudioManager mana = FindObjectOfType<AudioManager>();
        mana.Play("Dash", mana.sounds);
        //can't use jet or shot or jump
        halt.HaltSpecific(HaltMovement.Comps.Jet);
        halt.HaltSpecific(HaltMovement.Comps.Shooting);
        halt.HaltSpecific(HaltMovement.Comps.Jump);
        //freeze ROtation AND y postion
        rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
        //wait for duration
        yield return new WaitForSeconds(pb.DashDuration);
        //unfreeze freeze position
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        //rb.velocity = new Vector2(xdirect* 0.01f ,0);
        isDashing = false;
        //Unfreeze Jet and shot
        halt.ResumeSpecific(HaltMovement.Comps.Jet);
        halt.ResumeSpecific(HaltMovement.Comps.Shooting);
        halt.ResumeSpecific(HaltMovement.Comps.Jump);
        //wait before enabling dash again
        yield return new WaitForSeconds(pb.DashCooldown);
        canDash = true;

    }
}
