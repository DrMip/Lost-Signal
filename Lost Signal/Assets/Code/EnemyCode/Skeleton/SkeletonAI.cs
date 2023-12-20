using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Pathfinding")]
    private Transform target;
    public float activateDistance = 50f;
    public float pathUpdateSeconds = 0.5f;

    [Header("Physics")]
    public float speed = 200f, jumpForce = 100f;
    public float nextWaypointDistance = 3f;
    public float jumpNodeHeightRequirement = 0.8f;
    public float jumpModifier = 0.3f;
    public float jumpCheckOffset = 0.1f;

    [Header("Custom Behavior")]
    public bool followEnabled = true;
    public bool jumpEnabled = true, isJumping, isInAir;
    public bool directionLookEnabled = true;

    [SerializeField] Vector3 startOffset;

    private Path path;
    private int currentWaypoint = 0;
    [SerializeField] public RaycastHit2D isGrounded;
    Seeker seeker;
    Rigidbody2D rb;
    private bool isOnCoolDown;

    //handle room activation
    GameObject room;
    RoomTrigger trigger;
    ContactFilter2D filter;

    bool entered = false;
    bool exited = false;

    List<RaycastHit2D> rays = new List<RaycastHit2D>();
    public bool DoRoomActivation = true;

    //return to poistion
    Vector3 startPos;


    public void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindWithTag("Player").transform;

        isJumping = false;
        isInAir = false;
        isOnCoolDown = false; 

        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);

        //get what room you are in
        filter.NoFilter();
        Physics2D.Raycast(new Vector2(transform.position.x,transform.position.y), Vector2.down, filter, rays);
        foreach(RaycastHit2D ray in rays)
        {
            //Debug.Log(ray.transform.gameObject.layer + " " + LayerMask.GetMask("Middleground"));
            if(ray.transform.gameObject.layer == LayerMask.NameToLayer("Room"))
            {
                room = ray.transform.gameObject;
                //Debug.Log(room);
            }
        }
        // now room is the room
        
        trigger = room.GetComponent<RoomTrigger>();
        followEnabled = false;
        //get position
        startPos = transform.position;

    }
    private void Update()
    {
        if(DoRoomActivation)
        {
            if(trigger.PlayerInRoom == true && !entered)
            {
                entered = true;
                exited = false;
                followEnabled = true;
            }
            if(trigger.PlayerInRoom == false && !exited)
            {
                entered = false;
                exited = true;
                followEnabled = false;
                //return to your positions
                Invoke("ResetPosition", 1f);
                
            }
        
        }

    }

    void ResetPosition()
    {
        if(!trigger.PlayerInRoom) // if player hasn't returned to the room
            transform.position = startPos;
    }
    private void FixedUpdate()
    {
        if (TargetInDistance() && followEnabled)
        {
            PathFollow();
        }
    }

    private void UpdatePath()
    {
        if (followEnabled && TargetInDistance() && seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    private void PathFollow()
    {
        if (path == null)
        {
            return;
        }

        // Reached end of path
        if (currentWaypoint >= path.vectorPath.Count)
        {
            return;
        }

        // See if colliding with anything
        startOffset = transform.position - new Vector3(0f, GetComponent<Collider2D>().bounds.extents.y + jumpCheckOffset, transform.position.z);
        isGrounded = Physics2D.Raycast(startOffset, -Vector3.up, 0.05f);

        // Direction Calculation
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        //Vector2 force = direction * speed;

        // Jump
        if (jumpEnabled && isGrounded && !isInAir && !isOnCoolDown)
        {
            if (direction.y > jumpNodeHeightRequirement)
            {
                if (isInAir) return; 
                isJumping = true;
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                StartCoroutine(JumpCoolDown());

            }
        }
        if (isGrounded)
        {
            isJumping = false;
            isInAir = false; 
        }
        else
        {
            isInAir = true;
        }

        // Movement
        //rb.velocity = new Vector2(force.x, rb.velocity.y);
        transform.position += new Vector3(direction.x*speed* 0.01f , 0, 0);

        // Next Waypoint
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        // Direction Graphics Handling
        if (directionLookEnabled)
        {
            if (direction.x > 0.05f)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (direction.x < -0.05f)
            {
                transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
    }

    private bool TargetInDistance()
    {
        return Vector2.Distance(transform.position, target.transform.position) < activateDistance;
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    IEnumerator JumpCoolDown()
    {
        isOnCoolDown = true; 
        yield return new WaitForSeconds(1f);
        isOnCoolDown = false;
    }
}