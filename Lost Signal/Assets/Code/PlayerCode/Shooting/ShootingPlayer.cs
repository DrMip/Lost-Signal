using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEditor.Tilemaps;
using UnityEngine;


public class ShootingPlayer : MonoBehaviour
{
    public class Shot
    {
        public GameObject shotObject;
        public bool isShotRight;
        public bool hit;
        public float Xdirection;
        public float shotDecayTimeCounter = 0;
    }
    //shot list
    List<Shot> shots = new List<Shot>();
    //other components and refrences
    PlayerBehavior pb;
    Animator animShot;
    //shot prefab
    public GameObject shotPrefab;
    //layercast
    private LayerMask environment;
    private LayerMask enemy;
    //animations
    [SerializeField] string ExplodeAnimTimesUp = "";
    [SerializeField] string ExplodeAnimWall = "";
    [SerializeField] string ExplodeAnimEnemy = "";
    //shooting variables
    [NonSerialized] public bool pressedShoot;
    [NonSerialized] public bool canShoot = true;
    //temp variables
    [SerializeField] float ShotDistance;
    [SerializeField] float XDisfromCenter = 0.1f;
    //counters
    float shotTimeCounter = 0;
    //float shotDecayTimeCounter = 0;
    public bool temp;
    //other variables
    float Xscale;
    // Start is called before the first frame update
    void Start()
    {
        pb = GetComponent<PlayerBehavior>();
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
        for(int i = 0; i < shots.Count; i++)
        {
            Transform trans = shots[i].shotObject.transform;
            if(!shots[i].isShotRight)
            {
                trans.localScale = new Vector3(-Xscale,trans.localScale.y,trans.localScale.z);
            }
            //when shot hits or finishes and returns if needs to be deleted
            if(shots[i].shotObject)
                CheckandEndShot(shots[i]);
                
        }
        
        
        

    }
    void FixedUpdate()
    {
        if(pressedShoot)
        {
            //run shot timer
            if(shotTimeCounter < pb.ShotTime)
            {
                shotTimeCounter += Time.deltaTime;
            }
            //if cant shoot than cancel shoot
            else if(!canShoot)
            {
                pressedShoot = false;
                shotTimeCounter = 0;
            }
            //if waited to shoot and can shoot, shoot
            else
            {
                pressedShoot = false;
                shotTimeCounter = 0;
                Shoot();
                
            }
        }

        for(int i = 0; i < shots.Count; i++)
        {
            if(!shots[i].hit)
            {
                //make it go
                shots[i].shotObject.transform.position += new Vector3(shots[i].Xdirection*pb.ShotSpeed* 0.1f , 0, 0);
            }
        }
        if(temp)
        {
            Shoot();
            temp = false;
        }

    }

    void Shoot()
    {
        //create location to fire
        float xLocation = transform.localScale.x * ShotDistance;
        Vector3 shotPosition =new Vector3 (transform.position.x + xLocation, transform.position.y, transform.position.z);
        //create object
        Shot shot = new Shot();
        //create the shot
        shot.shotObject = Instantiate(shotPrefab , shotPosition, quaternion.identity);
        shot.shotObject.name = "shot " + (shots.Count + 1);
        //saves direction of shot
        if(xLocation > 0)
            shot.isShotRight = true;
        else
            shot.isShotRight = false;

        //get scale
        Xscale = shot.shotObject.transform.localScale.x;
        shot.Xdirection = (transform.localScale.x > 0) ? 1:(-1);
        //sets counter
        shot.shotDecayTimeCounter = 0;
        shots.Add(shot);

    }
    void CheckandEndShot(Shot shot)
    {
        animShot = shot.shotObject.GetComponent<Animator>();
        //destroy shot if animation ended (animation would be the exploded because of hit)
        if(animShot.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95f && !animShot.IsInTransition(0) && shot.hit)
        {
            shot.hit = false;
            shot.shotDecayTimeCounter = 0;
            Destroy(shot.shotObject);
            shots.Remove(shot);
            //Debug.Log(hit + " 1");
        }
        //if timer hasnt finished
        else if(shot.shotDecayTimeCounter < pb.ShotDecayTime && !shot.hit)
        {
            //Debug.Log(hit + " 2");
            //continue running timer
            shot.shotDecayTimeCounter += Time.deltaTime;
            //check for enemies
            if(ShotSearchFor(enemy,shot))
            {
                shot.hit = true;
                animShot.Play(ExplodeAnimEnemy);
                //Debug.Log("enemy");
            }
            else if(ShotSearchFor(environment,shot))
            {
                shot.hit = true;
                animShot.Play(ExplodeAnimWall);
                //Debug.Log("walls");
            }
        }
        else if(!shot.hit)
        {
            shot.shotDecayTimeCounter = 0;
            shot.hit = true;
            animShot.Play(ExplodeAnimTimesUp);
            //Debug.Log("disipated");

        }
        //Debug.Log(hit + " 6");
        
        //get what shot collided with
    }

    bool ShotSearchFor(LayerMask layer, Shot shot)
    {
        float newxdis = (shot.Xdirection > 0) ? XDisfromCenter:(-XDisfromCenter);
    
        //Debug.DrawLine(new Vector2(shot.transform.position.x + newxdis, shot.transform.position.y), new Vector2(shot.transform.position.x + newxdis, shot.transform.position.y + 0.1f), Color.green);
        return Physics2D.CircleCast(new Vector2(shot.shotObject.transform.position.x + newxdis, shot.shotObject.transform.position.y), 0.1f, Vector2.right, 0f, layer);
    }


}
