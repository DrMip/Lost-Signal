using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBarsScript : MonoBehaviour
{
    [SerializeField] BarFuncs bar;
    PlayerBehavior pb;
    // Start is called before the first frame update
    void Start()
    {
        pb = GetComponent<PlayerBehavior>();
        bar.SetMaxHealth(pb.MaxHealth);
        bar.SetMaxWrath(pb.MaxWrath);
        pb.health = pb.MaxHealth;
        pb.wrath = pb.MaxWrath;

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            pb.health -= 5;
            bar.SetHealth(pb.health);
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            pb.wrath -= 20;
            bar.SetWrath(pb.wrath);
        }
    }
}
