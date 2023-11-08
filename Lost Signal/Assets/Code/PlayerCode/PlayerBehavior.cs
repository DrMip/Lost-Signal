using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerBehavior : MonoBehaviour
{

    //Health
    [Header("Health")]
    public float MaxHealth;
    //float health;
    //bool dead = false;
    //wrath
    [Header("Wrath")]
    public float MaxWrath;
    //float wrath;
    [Header("X,Y movements")]
    public float MovementSpeed;

    [Header("Jumping")]
    public float JumpStrength;
    public float DelayJump = 0.3f;
    public float GroundedDelayAmount = 0.1f;

    [Header("Shooting")]
    public float ShotTime;
    public float ShotSpeed = 3;
    public float ShotDecayTime = 3;
    [Header("Jetpack")]
    public float JetPackStrength;
    public float JetTime;
    public float JetTimeCounter;
    public float JetRecoverRatio = 0.5f;






    // Start is called before the first frame update
    void Start()
    {

    }

    // for centeralize main variables and functions
    void Update()
    {
        //checks if grounded
        //isGrounded = Physics2D.CircleCast(new Vector2(groundSensor.position.x, groundSensor.position.y), 0.1f, Vector2.zero, 0, LayerMask.GetMask("Middleground"));
    }
}
