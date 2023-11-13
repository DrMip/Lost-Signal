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
    [SerializeField] bool startOpen = true;
    public bool OpenGate;//if open is true than open, can change with outside script
    [SerializeField] float startLoc;
    [SerializeField] float MoveDis;
    [SerializeField] float fallSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        childtrans = transform.GetChild(0);
        if(startOpen)
        {
            childtrans.position = transform.position;
            childtrans.position += new Vector3(0,startLoc, 0);   
        }

    }

    void Update()
    {
        if(OpenGate)
        {
            OpenGate = false;
            hasFallen = false;
            childtrans.position = transform.position;
            childtrans.position += new Vector3(0,startLoc, 0); 
        }
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
        Vector3 pos = childtrans.position;
        for(float dis = 0; dis < MoveDis; dis += fallSpeed * Time.deltaTime)
        {
            //Debug.Log(childtrans.position);
            
            childtrans.position = new Vector3(pos.x, pos.y - dis, pos.z);
            yield return null;
        }
        childtrans.position =  new Vector3(pos.x,pos.y - MoveDis, pos.z);
    }
}
