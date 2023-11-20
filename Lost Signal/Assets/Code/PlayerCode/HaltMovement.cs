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
        dash.enabled = false;
        jet.enabled = false;
        movement.move = false;
        health.enabled = false;
        shoot.canShoot = false;
        jump.enabled = false;
    }
    public void ResumeAll()
    {
        dash.enabled = true;
        jet.enabled = true;
        movement.move = true;
        health.enabled = true;
        shoot.canShoot = true;
        jump.enabled = true;
    }
    public void HaltSpecific(Comps num)
    {
        Debug.Log((int)num);
        switch((int)num)
        {
            case 0:
                dash.enabled = false;
                break;
            case 1:
                jet.enabled = false;
                break;
            case 2:
                movement.move = false;
                break;
            case 3:
                health.enabled = false;
                break;
            case 4:
                shoot.canShoot = false;
                break;
            case 5:
                jump.enabled = false;
                break;
        }
    }
    public void ResumeSpecific(Comps num)
    {
        switch((int)num)
        {
            case 0:
                dash.enabled = true;
                break;
            case 1:
                jet.enabled = true;
                break;
            case 2:
                movement.move = true;
                break;
            case 3:
                health.enabled = true;
                break;
            case 4:
                shoot.canShoot = true;
                break;
            case 5:
                jump.enabled = true;
                break;
        }
    }
    
}
