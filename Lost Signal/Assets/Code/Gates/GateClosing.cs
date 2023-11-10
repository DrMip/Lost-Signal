using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class GateClosing : MonoBehaviour
{
    //components
    Transform childtrans;
    //variables
    bool hasFallen;

    [SerializeField] float startLoc;
    [SerializeField] float MoveDis;
    [SerializeField] float fallSpeed;
    // Start is called before the first frame update
    void Start()
    {
        childtrans = transform.GetChild(0);
        childtrans.position = transform.position;
        childtrans.position += new Vector3(0,startLoc, 0);
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player") && !hasFallen)
        {
            StartCoroutine(Fall());
        }
    }
    // Update is called once per frame
    IEnumerator Fall()
    {
        hasFallen = true;
        Vector3 pos = transform.position;
        for(float dis = 0; dis < MoveDis; dis += fallSpeed)
        {
            Debug.Log(dis);
            
            childtrans.position = new Vector3(pos.x, pos.y - dis, pos.z);
            yield return null;
        }
        childtrans.position =  new Vector3(pos.x,pos.y - MoveDis, pos.z);
    }
}
