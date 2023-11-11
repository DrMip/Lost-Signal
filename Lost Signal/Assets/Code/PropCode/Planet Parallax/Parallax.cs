using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private GameObject player;
    Transform childtrans;
    //vars
    private float startpos;
    private int hasEntered;
    bool doParallax;
    //serialized
    [SerializeField] float parallaxEffect;
    [SerializeField] float pullback;
    float pullbackValue;


    // Start is called before the first frame update
    void Start()
    {
        childtrans = transform.GetChild(0);
        player = GameObject.Find("Player");
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        //Debug.Log("entered");
        startpos = player.transform.position.x;
        doParallax = true;
        pullbackValue = (player.transform.localScale.x > 0) ? pullback : 2*pullback;

    }
    // Update is called once per frame
    void Update()
    {
        if(doParallax)
        {
            
            float dist = player.transform.position.x * parallaxEffect;
            //Debug.Log(dist);
            childtrans.position = new Vector3(startpos + dist - pullbackValue,childtrans.position.y , childtrans.position.z);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        //Debug.Log("exited");
        doParallax = false;
    }
}
