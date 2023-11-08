using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpScript : MonoBehaviour
{
    //script is in charge to know *when* to jump
    private PlayerMovement plm;
    private Rigidbody2D rb;
    private PlayerBehavior pb;

    public bool Jump;
    private float delayJump = 0.3f;
    private bool JumpWasPressed;

    float delayCounter;
    bool WasDelayGrounded;

    bool isInJump;

    float jumpBugCounter; //resets isInJump after couple of frames
    //handles delayed jumping
    float groundedDelayCounter;

    private float groundedDelayAmount = 0.1f;
    
    public bool delayedIsGrounded;






    // Start is called before the first frame update
    void Start()
    {
        plm = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
        pb = GetComponent<PlayerBehavior>();
        //initialize constants
        delayJump = pb.DelayJump;
        groundedDelayAmount = pb.GroundedDelayAmount;
    }

    // Update is called once per frame
    void Update()
    {
        //check if jump was pressed and keep it pressed until fixedUpdate notices
        if (Input.GetButtonDown("Jump"))
            JumpWasPressed = true;
                
        //handles grounded delay(delays grounded to make platform nicer to jump from)
        if(plm.isGrounded)
        {
            //Debug.Log("is on the ground");
            delayedIsGrounded = true;
            groundedDelayCounter = 0;
        }
        //instantiate delay jump if player is falling, he was grounded, and we havn't instatianted yet
        else if(rb.velocity.y <=0 && plm.wasGrounded && groundedDelayCounter == 0)
        {
            //Debug.Log("left the ground");
            groundedDelayCounter = 0.0001f;
        }
        //if falling and isnt grounded, start counting the delay
        else if(groundedDelayCounter > 0)
        {
            //Debug.Log("is delaying grounded " + delayedIsGrounded + plm.isGrounded);
            //Debug.Log(rb.velocity.y);
            if(groundedDelayCounter < groundedDelayAmount)
            {
                groundedDelayCounter += Time.deltaTime;
            }
            else
            {
                delayedIsGrounded = false;
            }

                 
        }
        else
        {
            //Debug.Log("no longer delayed jump");
            delayedIsGrounded = false;
        }
        //counter to jump delay
        if (!delayedIsGrounded)
        {
            delayCounter = 0;
        }
        if(delayCounter < delayJump)
        {
            delayCounter += Time.deltaTime;
        }

        //Debug.Log(delayedIsGrounded + " " + plm.isGrounded);

    }

    void FixedUpdate()
    {
        //nullifize jumping independently if is in the air
        if(JumpWasPressed && !delayedIsGrounded)
        {
            JumpWasPressed = false;
        }
        
        // if the player is on the ground, and was on the ground in the last frame
        if (delayedIsGrounded && WasDelayGrounded == true)
        {
            //if the player had kept jump down and waited the delay time or he released jump and jumped again, and he isn't already jumping, then jump
            if(((Input.GetButton("Jump") && delayCounter >= delayJump) || JumpWasPressed) && !isInJump)
            {
                    isInJump = true;
                    Jump = true;

            }
        }
        WasDelayGrounded = delayedIsGrounded;
        //make sure that is in Jump is false when hits the ground
        //have to delay a bit because it registers at the begining of the jump that it is grounded, even though jump was pressed
        if(isInJump)
        {
            if(jumpBugCounter < 4)
                jumpBugCounter++;
            else
            {
                jumpBugCounter = 0;
                isInJump = false;
            }
        }
    }
}
