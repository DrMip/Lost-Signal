using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerBehavior : MonoBehaviour
{

    //Health
    [Header("Health")]
    [SerializeField] public float MaxHealth;
    //float health;
    //bool dead = false;
    //wrath
    [Header("Wrath")]
    [SerializeField] public float MaxWrath;
    //float wrath;
    [Header("X,Y movements")]
    [SerializeField] public float MovementSpeed;

    [Header("Jumping")]
    [SerializeField] public float JumpStrength;
    [SerializeField] public float DelayJump = 0.3f;
    [SerializeField] public float GroundedDelayAmount = 0.1f;

    [Header("Shooting")]
    [SerializeField] public float shotTime;
    [SerializeField] public float shotSpeed;






    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
