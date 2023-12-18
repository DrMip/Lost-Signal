using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Security;
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
    SpecialAttacks sa;
    JetScript js;
    Dash dash;
    //variables
    //for animation
    private string currentState;


    //particles
    private enum Action
    {
        Running,
        Jeting
    }
    private bool[] ActionState = new bool[2];
    private bool[] LastActionState = new bool[2];
    private string[] sounds = { "Walk", "Jet" };
    private bool crRunning;
    [Tooltip("1 is Running, 2 is Jetting")]
    [SerializeField] List<ParticleSystem> Particles = new();
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
        js = GetComponent<JetScript>();
        sa = GetComponent<SpecialAttacks>();


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
        if (!crRunning) StartCoroutine(HandleDelayedComputation());


        if (sa.wrathHeal.isDoingSpecial)
        {
            StateChanger(sa.wrathHeal.Animation);
        }
        else if(sa.explode.isDoingSpecial)
        {
            StateChanger(sa.explode.Animation);
        }
        else if(dash.isDashing)
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
                    ActionState[(int)Action.Running] = true; //update the action state of running to true
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
                    ActionState[(int)Action.Running] = true; //update the action state of running to true
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
    //allows the computer to wait more then 1 frame in order to get information is the player stopped or continued moving 
    IEnumerator HandleDelayedComputation()
    {
        crRunning = true;
        //reset running
        ActionState[(int)Action.Running] = false;

        if (js.jetting)
            ActionState[(int)Action.Jeting] = true;
        else
            ActionState[(int)Action.Jeting] = false;

        //wait for 0.03 seconds to register if stoped running
        yield return new WaitForSecondsRealtime(0.04f);


        FXHandler((int)Action.Running);
        FXHandler((int)Action.Jeting);
        crRunning = false;
    }
    //particles
    void FXHandler(int indexToCheck)
    {
        //check if needs to be changed
        if (ActionState[indexToCheck] == LastActionState[indexToCheck]) return;
        //play if now wants to run, play
        if (ActionState[indexToCheck] == true)
        {
            //run particle
            Particles[indexToCheck].Play();
            //play sound
            AudioManager mana = FindObjectOfType<AudioManager>();
            mana.Play(sounds[indexToCheck], mana.sounds);
        }

        //else stop
        else
        {
            //stop particle
            Particles[indexToCheck].Stop();

            //stop sound
            AudioManager mana = FindObjectOfType<AudioManager>();
            mana.Stop(sounds[indexToCheck], mana.sounds);

        }
            
        //remember state
        LastActionState[indexToCheck] = ActionState[indexToCheck];
    }
}
