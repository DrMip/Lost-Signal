using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    EnemyBehavior eb;
    GameObject player;
    //handling attacks happen once
    bool hasDashAttacked = false;
    bool hasShotAttacked = false;

    //know if hurt
    [NonSerialized] public bool Hurt;
    int lastHealth;

    [SerializeField] BoxCollider2D standardCollider;
    // Start is called before the first frame update
    void Start()
    {
        eb = GetComponent<EnemyBehavior>();
        player = GameObject.FindWithTag("Player");

        eb.health = eb.MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(eb.health <= 0)
        {
            eb.health = 0;
            //kill(for now only destroy)
            Destroy(gameObject);
        }
        //handle knowing when hurt
        if(lastHealth > eb.health)
        {
            Hurt = true;
        }
        else
        {
            Hurt = false;
        }
        lastHealth = eb.health;
    }
    private void OnTriggerStay2D(Collider2D other) 
    {
        
        //if hits dashing player
        if(other.gameObject.CompareTag("Player") && player.GetComponent<Dash>().isDashing && !hasDashAttacked)
        {
            Debug.Log("hit");
            hasDashAttacked = true;
            standardCollider.enabled = false;
            eb.health -= 50;
        }
        else if(other.gameObject.CompareTag("Shot") && !hasShotAttacked)
        {
            hasShotAttacked = true;
            eb.health -= 10;
        }
    }
    private void OnTriggerExit2D(Collider2D other) 
    {
        Invoke("DelayedExit", 0.2f);
    }
    void DelayedExit()
    {
        //dash
        hasDashAttacked = false;
        standardCollider.enabled = true;
        //shot
        hasShotAttacked = false;

    }
}
