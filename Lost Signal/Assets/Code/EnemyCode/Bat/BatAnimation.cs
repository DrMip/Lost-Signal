using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyHurt;

public class BatAnimation : MonoBehaviour
{
    //components for state
    [SerializeField] EnemyHealth enemyHealth;

    EnemyState enemyState = new EnemyState();


    //strings
    //const string SKELETON_IDLE = "Skeleton_Idle";
    const string BAT_FLY = "Bat_Fly";
    const string BAT_HURT = "This String has no meaning lol";

    void Start()
    {


        //Debug.Log(defualtColor);

        //init all param for enemyState

        enemyState.rndr = GetComponent<SpriteRenderer>();
        enemyState.anim = GetComponent<Animator>();

        enemyState.EnemyHurtAnim = BAT_HURT;
        enemyState.defualtColor = enemyState.rndr.color;
        //
        enemyState.StateChanger(BAT_FLY);
        //
    }


    //Walking

    // Update is called once per frame
    void Update()
    {

        if (enemyHealth.Hurt)
        {
            enemyState.StateChanger(BAT_HURT);
            //StateChanger(SKELETON_HURT);
        }
        else
        {
            enemyState.StateChanger(BAT_FLY);
        }
    }

    
}
