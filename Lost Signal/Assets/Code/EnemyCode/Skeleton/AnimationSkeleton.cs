using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSkeleton : MonoBehaviour
{
    // neccesary components
    Animator anim;
    //components for state
    // pm;
    EnemyHealth skeletonHealth;
    Rigidbody2D rb;
    SpriteRenderer rndr;
    //variables
    private string currentState;
    private Color defualtColor;
    Vector3 lastPos;
    [SerializeField] float temp;

    [SerializeField] private Color newColor;
    [SerializeField] private float ColorDisplayTime;

    //strings
    const string SKELETON_IDLE = "Skeleton_Idle";
    const string SKELETON_WALK = "Skeleton_Walk";
    const string SKELETON_HURT = "Player_Shoot";
    //const string RUN_AND_GUN = "Run_and_Gun";
    //const string PLAYER_JUMP_UP = "Player_Jump_Up";
    //const string PLAYER_FALLING_DOWN = "Player_Falling_Down";
    //const string PLAYER_DASH = "Player_Dash";
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rndr = GetComponent<SpriteRenderer>();
        
        skeletonHealth = GetComponent<EnemyHealth>();

        //save defualt color
        defualtColor = rndr.color;
        //Debug.Log(defualtColor);
        lastPos = transform.position;


    }

    void StateChanger(string newState)
    {
        //check if needs to be changed
        if(newState == currentState) return;
        //if we are taking about hurt
        if(newState == SKELETON_HURT)
        {
            StartCoroutine(HurtColor());
            return;
        }
        //play animation
        anim.Play(newState);
        //remember state
        currentState = newState;
    }
    IEnumerator HurtColor()
    {
        //change color
        rndr.color = newColor;
        yield return new WaitForSeconds(ColorDisplayTime);
        //return to color
        rndr.color = defualtColor;
    }
    //Walking

    // Update is called once per frame
    void Update()
    {
        
        if(skeletonHealth.Hurt)
        {
            StateChanger(SKELETON_HURT);
            //StateChanger(SKELETON_HURT);
        }
        else
        {
            StateChanger(SKELETON_WALK);
        }
        lastPos = transform.position;
    }

}
