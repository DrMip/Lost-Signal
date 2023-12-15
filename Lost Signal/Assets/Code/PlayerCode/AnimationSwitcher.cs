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
        //make the actionState for running array 0
        ActionState[(int)Action.Running] = false;
        //checks if needs to jet
        if(js.jetting)
            ActionState[(int)Action.Jeting] = true;
        else
            ActionState[(int)Action.Jeting] = false;
        


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
        Debug.Log(ActionState[(int)Action.Jeting]);
        //handling particles
        ParticleHandler((int)Action.Running);
        ParticleHandler((int)Action.Jeting);


    }

    //particles
    void ParticleHandler(int indexToCheck)
    {
        //check if needs to be changed
        if (ActionState[indexToCheck] == LastActionState[indexToCheck]) return;
        //play if now wants to run, play
        if (ActionState[indexToCheck] == true)
            Particles[indexToCheck].Play();
        //else stop
        else
            Particles[indexToCheck].Stop();
        //remember state
        LastActionState[indexToCheck] = ActionState[indexToCheck];
    }
}
