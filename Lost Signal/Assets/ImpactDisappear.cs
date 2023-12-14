using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactDisappear : MonoBehaviour
{
    //what animation to play
    [Tooltip("null,enemy,wall")]
    public string type;
    //animation
    Animator anim;
    [SerializeField] string ExplodeAnimTimesUp = "";
    [SerializeField] string ExplodeAnimWall = "";
    [SerializeField] string ExplodeAnimEnemy = "";
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        if(type == "null")
            anim.Play(ExplodeAnimTimesUp);
        if(type == "enemy")
            anim.Play(ExplodeAnimEnemy);
        if(type == "wall")
            anim.Play(ExplodeAnimWall);
        Invoke("EndAnim", 0.8f);
    }

    void EndAnim()
    {
        Destroy(gameObject);
    }
}
