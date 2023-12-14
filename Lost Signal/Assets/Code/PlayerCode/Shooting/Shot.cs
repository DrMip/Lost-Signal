using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : ScriptableObject
{
    public GameObject shotObject;
    public bool isShotRight;
    public bool hit;
    public float Xdirection;
    public float shotDecayTimeCounter = 0;
}
