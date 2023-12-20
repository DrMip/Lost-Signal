using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatAIAdditive : MonoBehaviour
{
    AIPath aiPath;
    AIDestinationSetter destinationSetter;

    [SerializeField] bool directionLookEnabled;
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
    // Start is called before the first frame update
    void Start()
    {
        aiPath = GetComponent<AIPath>();
        destinationSetter = GetComponent<AIDestinationSetter>();
        //Player is target
        destinationSetter.target = GameObject.FindGameObjectWithTag("Player").transform;
        //get what room you are in
        filter.NoFilter();
        Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.down, filter, rays);
        foreach (RaycastHit2D ray in rays)
        {
            //Debug.Log(ray.transform.gameObject.layer + " " + LayerMask.GetMask("Middleground"));
            if (ray.transform.gameObject.layer == LayerMask.NameToLayer("Room"))
            {
                room = ray.transform.gameObject;
                //Debug.Log(room);
            }
        }
        //now you know what room you are
        trigger = room.GetComponent<RoomTrigger>();
        aiPath.canMove = false;
        //get position
        startPos = transform.position;

    }
    private void Update()
    {
        if (DoRoomActivation)
        {
            if (trigger.PlayerInRoom == true && !entered)
            {
                entered = true;
                exited = false;
                aiPath.canMove = true;
            }
            if (trigger.PlayerInRoom == false && !exited)
            {
                entered = false;
                exited = true;
                aiPath.canMove = false;
                //return to your positions
                Invoke("ResetPosition", 1f);

            }

        }
        float directionX = transform.position.x - destinationSetter.target.transform.position.x;
        //Debug.Log(directionX);
        // Direction Graphics Handling
        if (directionLookEnabled)
        {
            if (directionX  > 0.05f)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (directionX < -0.05f)
            {
                transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }

    }

    void ResetPosition()
    {
        if (!trigger.PlayerInRoom) // if player hasn't returned to the room
            transform.position = startPos;
    }


}
