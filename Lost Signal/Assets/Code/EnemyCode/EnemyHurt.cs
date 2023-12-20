using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class EnemyHurt : MonoBehaviour
{
    public class EnemyState
    {
        public string currentState;
        public string EnemyHurtAnim;
        public Color defualtColor;

        public Animator anim;
        public SpriteRenderer rndr;

        public static MonoBehaviour mono;

        public static Color EnemyHurtColor = Color.red;
        public static float ColorDisplayTime = 0.2f;
        public void StateChanger(string newState)
        {
            //check if needs to be changed
            if (newState == currentState) return;
            //if we are taking about hurt
            if (newState == EnemyHurtAnim)
            {
                mono.StartCoroutine(HurtColor());
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
            rndr.color = EnemyHurtColor;
            yield return new WaitForSeconds(ColorDisplayTime);
            //return to color
            rndr.color = defualtColor;
        }
    }
    void Awake()
    {
        EnemyState.mono = this;
    }

}
