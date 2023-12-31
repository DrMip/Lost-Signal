using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyHurt;

public class AnimationSkeleton : MonoBehaviour
{
    //components for state
    // pm;
    EnemyHealth skeletonHealth;

    EnemyState enemyState = new EnemyState();


    //strings
    //const string SKELETON_IDLE = "Skeleton_Idle";
    const string SKELETON_WALK = "Skeleton_Walk";
    const string SKELETON_HURT = "Player_Shoot";

    void Start()
    {
        
        skeletonHealth = GetComponent<EnemyHealth>();

        //Debug.Log(defualtColor);

        //init all param for enemyState

        enemyState.rndr = GetComponent<SpriteRenderer>();
        enemyState.anim = GetComponent<Animator>();

        enemyState.EnemyHurtAnim = SKELETON_HURT;
        enemyState.defualtColor = enemyState.rndr.color;


    }


    //Walking

    // Update is called once per frame
    void Update()
    {
        
        if(skeletonHealth.Hurt)
        {
            enemyState.StateChanger(SKELETON_HURT);
            //StateChanger(SKELETON_HURT);
        }
        else
        {
            enemyState.StateChanger(SKELETON_WALK);
        }
    }

}
