using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Tilemaps;
using UnityEngine;

public class ShootingPlayer : MonoBehaviour
{
    //other components and refrences
    PlayerBehavior pb;
    Animator ani;
    //shot prefab
    public GameObject shotPrefab;
    //shot object
    private GameObject shot;
    //layercast
    private LayerMask environment;
    private LayerMask enemy;
    //animations
    [SerializeField] string ExplodeAnimTimesUp = "";
    [SerializeField] string ExplodeAnimWall = "";
    [SerializeField] string ExplodeAnimEnemy = "";
    //shooting variables
    private float shotTime;
    private float shotSpeed;
    private float shotDecayTime;
    bool isShotRight;
    bool pressedShoot;
    bool hit;
    float Xdirection;
    //temp variables
    [SerializeField] float ShotDistance;
    [SerializeField] float XDisfromCenter = 0.1f;
    //counters
    float shotTimeCounter = 0;
    float shotDecayTimeCounter = 0;

    //other variables
    float Xscale;
    // Start is called before the first frame update
    void Start()
    {
        pb = GetComponent<PlayerBehavior>();

        shotTime = pb.ShotTime;
        shotSpeed = pb.ShotSpeed;
        shotDecayTime = pb.ShotDecayTime;

        //set layers
        environment = LayerMask.GetMask("Middleground");
        enemy = LayerMask.GetMask("Enemies");
        

    }

    // Update is called once per frame
    void Update()
    {
        //checking for button click
        if(Input.GetButtonDown("Fire1"))
        {
            pressedShoot = true;
        }
        //if didnt shot right then fix direction of shoot 
        if(!isShotRight && shot)
        {
            shot.transform.localScale = new Vector3(-Xscale,shot.transform.localScale.y,shot.transform.localScale.z);
        }
        //when shot hits or finishes
        if(shot)
        {
  
            CheckandEndShot();
        }
        
        

    }
    void FixedUpdate()
    {
        if(shotTimeCounter < shotTime && pressedShoot)
        {
            shotTimeCounter += Time.deltaTime;
        }
        else if(pressedShoot)
        {
            shotTimeCounter = 0;
            Shoot();
            pressedShoot = false;
        }

        if(shot && !hit)
        {
            //make it go
            shot.transform.position += new Vector3(Xdirection*shotSpeed* 0.1f , 0, 0);
        }
    }

    void Shoot()
    {
        //create location to fire
        float xLocation = transform.localScale.x * ShotDistance;
        Vector3 shotPosition =new Vector3 (transform.position.x + xLocation, transform.position.y, transform.position.z);
        //saves direction of shot
        if(xLocation > 0)
            isShotRight = true;
        else
            isShotRight = false;

        //create the shot
        shot = Instantiate(shotPrefab , shotPosition, quaternion.identity);
        //get scale
        Xscale = shot.transform.localScale.x;
        Xdirection = (transform.localScale.x > 0) ? 1:(-1);
        //gets component
        ani = shot.GetComponent<Animator>();
    


        
        
    }
    void CheckandEndShot()
    {
        //destroy shot if animation ended (animation would be the exploded because of hit)
        if(ani.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !ani.IsInTransition(0) && hit)
        {
            hit = false;
            shotDecayTimeCounter = 0;
            Destroy(shot);
            //Debug.Log(hit + " 1");
        }
        //if timer hasnt finished
        else if(shotDecayTimeCounter < shotDecayTime && !hit)
        {
            //Debug.Log(hit + " 2");
            //continue running timer
            shotDecayTimeCounter += Time.deltaTime;
            //check for enemies
            if(ShotSearchFor(enemy))
            {
                hit = true;
                ani.Play(ExplodeAnimEnemy);
                //Debug.Log(hit + " 3");
            }
            else if(ShotSearchFor(environment))
            {
                hit = true;
                ani.Play(ExplodeAnimWall);
                //Debug.Log(hit + " 4");
            }
        }
        else if(!hit)
        {
            shotDecayTimeCounter = 0;
            hit = true;
            ani.Play(ExplodeAnimTimesUp);
            //Debug.Log(hit + " 5");

        }
        //Debug.Log(hit + " 6");
        
        //get what shot collided with
    }

    bool ShotSearchFor(LayerMask layer)
    {
        float newxdis = (Xdirection > 0) ? XDisfromCenter:(-XDisfromCenter);
    
        //Debug.DrawLine(new Vector2(shot.transform.position.x + newxdis, shot.transform.position.y), new Vector2(shot.transform.position.x + newxdis, shot.transform.position.y + 0.1f), Color.green);
        return Physics2D.CircleCast(new Vector2(shot.transform.position.x + newxdis, shot.transform.position.y), 0.1f, Vector2.right, 0f, layer);
    }


}
