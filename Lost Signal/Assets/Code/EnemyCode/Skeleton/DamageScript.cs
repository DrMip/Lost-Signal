using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DamageScript : MonoBehaviour
{
    EnemyBehavior eb;
    GameObject player;

    //hits
    float hitTimer = 0;
    bool doHit = false;
    // Start is called before the first frame update
    void Start()
    {
        eb = GetComponent<EnemyBehavior>();
        player = GameObject.FindWithTag("Player");

    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.CompareTag("Player"))
        {
            doHit = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(doHit)
        {
            //Debug.Log(doHit);
            if(hitTimer < eb.HitInterval)
            {
                hitTimer += Time.deltaTime;
            }
            else
            {
                hitTimer = 0;
                player.GetComponent<PlayerBehavior>().health -= eb.DealDamage;
            }
        }
        else
        {
            hitTimer = 0;
        }
    }
    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.gameObject.CompareTag("Player"))
        {
            doHit = false;
        }
    }
}
