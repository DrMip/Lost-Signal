using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class RaycastShooting : MonoBehaviour
{
    //impact
    [SerializeField] GameObject ImpactPrefab; 
    GameObject impactObject;
    //shot
    RaycastHit2D ray;
    //hits
    float Yoffset = 0.15f;
    PlayerBehavior pb;
    //layermasks
    public List<string> Tags;
    //animations
 
    [NonSerialized] public bool pressedShoot;
    [NonSerialized] public bool canShoot = true;
    //counters
    float shotTimeCounter = 0;
    //line
    LineRenderer lineRenderer;

    private LayerMask environment;
    private LayerMask enemy;

    //know when shot
    //[NonSerialized] public bool shoot = false;
    // Start is called before the first frame update
    void Start()
    {
        pb = GetComponent<PlayerBehavior>();
        lineRenderer = transform.GetChild(1).GetComponent<LineRenderer>();
        //set layers
        environment = LayerMask.NameToLayer("Middleground");
        enemy = LayerMask.NameToLayer("Enemies");

        lineRenderer.enabled = false;
        

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            pressedShoot = true;
            //shoot = true;
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
                //shoot = false;
                
            }
        }
    }
    void Shoot()
    {
        //set hit to true
        //get location of initiation (if to the left or to the right) 
        float xLocation = transform.localScale.x * 0.1f;
        Vector2 direction = (transform.localScale.x > 0) ? Vector2.right : Vector2.left;
        string[] str = {LayerMask.LayerToName(enemy), LayerMask.LayerToName(environment)};
        //raycast check from almost to the player to distance and store hits in results 
        ray = Physics2D.Raycast(new Vector2(transform.position.x + xLocation, transform.position.y - Yoffset), direction, pb.shotDis, LayerMask.GetMask(str));
        //Debug.DrawLine(new Vector2(transform.position.x + xLocation, transform.position.y), new Vector2(xLocation + direction.x*pb.shotDis, transform.position.y), Color.white, 4f);

        if(ray.collider == null)
        {
            //if didnt hit anything, create shot object where it was supposed to be
            impactObject = Instantiate(ImpactPrefab , new Vector2(transform.position.x + xLocation + direction.x*pb.shotDis, transform.position.y - Yoffset), quaternion.identity);
            impactObject.GetComponent<ImpactDisappear>().type = "null";
            //make line
            lineRenderer.SetPosition(0, new Vector2(transform.position.x + xLocation, transform.position.y - Yoffset));
            lineRenderer.SetPosition(1, new Vector2(transform.position.x + xLocation + direction.x*pb.shotDis, transform.position.y - Yoffset));
        }
        else
        {
            impactObject = Instantiate(ImpactPrefab , ray.point, quaternion.identity);
            if(ray.transform.gameObject.layer == enemy)
            {
                //tell what animation
                impactObject.GetComponent<ImpactDisappear>().type = "enemy";
                //tell the enemy that it has been shot
                ray.transform.gameObject.GetComponent<EnemyHealth>().EnemyShot();
            }
            if(ray.transform.gameObject.layer == environment)
            {
                impactObject.GetComponent<ImpactDisappear>().type = "wall";
            }
            //make line
            lineRenderer.SetPosition(0, new Vector2(transform.position.x + xLocation, transform.position.y - Yoffset));
            lineRenderer.SetPosition(1, new Vector2(ray.point.x, ray.point.y));
        }
        //play sound
        AudioManager mana = FindObjectOfType<AudioManager>();
        mana.Play("Shooting", mana.sounds);
        //draw line for 0.02 seconds
        lineRenderer.enabled = true;
        Invoke("disableLineRenderer", 0.05f);
        
    }
    void disableLineRenderer()
    {
        lineRenderer.enabled = false;
    }
}
