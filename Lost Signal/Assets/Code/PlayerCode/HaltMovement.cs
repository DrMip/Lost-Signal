using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaltMovement : MonoBehaviour
{
    //AllmovementComponents
    Dash dash;
    JetScript jet;
    PlayerMovement movement;
    PlayerBarsScript health;
    ShootingPlayer shoot;
    JumpScript jump;
    public enum Comps
    {
        Dash,
        Jet,
        PlayerMovement,
        Health,
        Shooting,
        Jump

    }
    private void Awake() 
    {
        dash = GetComponent<Dash>();
        jet = GetComponent<JetScript>();
        movement = GetComponent<PlayerMovement>();
        health = GetComponent<PlayerBarsScript>();
        shoot = GetComponent<ShootingPlayer>();
        jump = GetComponent<JumpScript>();
    }
    //this method halts all
    public void HaltAll()
    {
        //Debug.Log("halt");
        dash.enabled = false;
        jet.enabled = false;
        movement.move = false;
        health.enabled = false;
        shoot.enabled = false;
        jump.enabled = false;
    }
    public void ResumeAll()
    {
        dash.enabled = true;
        jet.enabled = true;
        movement.move = true;
        health.enabled = true;
        shoot.enabled = true;
        jump.enabled = true;
    }
    public void HaltSpecific(int num)
    {
        switch(num)
        {
            case 1:
                dash.enabled = false;
                break;
            case 2:
                jet.enabled = false;
                break;
            case 3:
                movement.move = false;
                break;
            case 4:
                health.enabled = false;
                break;
            case 5:
                shoot.enabled = false;
                break;
            case 6:
                jump.enabled = false;
                break;
        }
    }
    public void ResumeSpecific(int num)
    {
        switch(num)
        {
            case 1:
                dash.enabled = true;
                break;
            case 2:
                jet.enabled = true;
                break;
            case 3:
                movement.move = true;
                break;
            case 4:
                health.enabled = true;
                break;
            case 5:
                shoot.enabled = true;
                break;
            case 6:
                jump.enabled = false;
                break;
        }
    }
    
}
