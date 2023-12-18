using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerBehavior : MonoBehaviour
{

    //Health
    [Header("Health")]
    public int MaxHealth;
    public int health;
    //bool dead = false;
    //wrath
    [Header("Wrath")]
    public int MaxWrath;
    public int wrath;
    public int AddedWrathOnDeath;
    [Header("X,Y movements")]
    public float MovementSpeed;

    [Header("Jumping")]
    public float JumpStrength;
    public float DelayJump = 0.3f;
    public float GroundedDelayAmount = 0.1f;

    [Header("Shooting")]
    public float ShotTime;
    public float ShotSpeed = 3;
    [Tooltip("Used in Animated shot")]
    public float ShotDecayTime = 3;
    [Tooltip("Used in Raycast shot")]
    public float shotDis = 10f;
    [Header("Jetpack")]
    public float JetPackStrength;
    public float JetTime;
    public float JetTimeCounter;
    public float JetRecoverRatio = 0.5f;
    public float JetCooldown;
    [Header("Dash")]
    public float DashStrength;
    public float DashDuration;
    public float DashCooldown;

    [Header("AttacksDamage")]
    public int DashDamage;

    public int ShotDamage;
}
