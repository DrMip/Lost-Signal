using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenGateCollider : MonoBehaviour
{
    [SerializeField] GateClosing gate;
    bool open = false;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(!open)
        {
            gate.OpenGate = true;
            open = true;
        }
        
    }
}
