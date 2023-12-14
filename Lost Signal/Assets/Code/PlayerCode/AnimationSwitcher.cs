using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class AnimationSwitcher : MonoBehaviour
{
    // neccesary components
    Animator anim;
    //components for state
    PlayerMovement pm;
    ShootingPlayer sp;
    Rigidbody2D rb;
    RaycastShooting rs;

    Dash dash;
    //variables
    private string currentState;
    //strings
    const string PLAYER_IDLE = "Player_Idle";
    const string PLAYER_RUN = "Player_Run";
    const string PLAYER_SHOOT = "Player_Shoot";
    const string RUN_AND_GUN = "Run_and_Gun";
    const string PLAYER_JUMP_UP = "Player_Jump_Up";
    const string PLAYER_FALLING_DOWN = "Player_Falling_Down";
    const string PLAYER_DASH = "Player_Dash";
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        
        pm = GetComponent<PlayerMovement>();
        sp = GetComponent<ShootingPlayer>();
        rs = GetComponent<RaycastShooting>();
        dash = GetComponent<Dash>();


    }

    void StateChanger(string newState)
    {
        //check if needs to be changed
        if(newState == currentState) return;
        //play animation
        anim.Play(newState);
        //remember state
        currentState = newState;
    }
    //IsGrounded
    //Shooting
    //Y velocity
    //x velocity

    // Update is called once per frame
    void Update()
    {
        if(dash.isDashing)
        {
            StateChanger(PLAYER_DASH);
        }
        else if(pm.isGrounded)
        {
            if(rs.pressedShoot || sp.pressedShoot)
            {
                if(Mathf.Abs(pm.movement) > 0.1f)
                {
                    StateChanger(RUN_AND_GUN);
                }
                else
                {
                    StateChanger(PLAYER_SHOOT);
                }
            }
            else
            {
                if(Mathf.Abs(pm.movement) > 0.1f)
                {
                    //Debug.Log("run");
                    StateChanger(PLAYER_RUN);
                }
                else
                {
                    //Debug.Log("idle");
                    StateChanger(PLAYER_IDLE);
                }
            }
        }
        else if(rb.velocity.y != 0)
        {
            if(rb.velocity.y > 0)
            {
                StateChanger(PLAYER_JUMP_UP);
            }
            else
            {
                StateChanger(PLAYER_FALLING_DOWN);
            }
        }


    }
}
