using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyAIFeature;

public class BatAIAdditive : MonoBehaviour
{
    AIPath aiPath;
    AIDestinationSetter destinationSetter;

    [SerializeField] bool directionLookEnabled;
    //handle room activation    
    public RoomActivation roomActivation;
    //handle sight
    public Sight sight;
    //sight drawing interval
    private float IntervalForDrawing;

    // Start is called before the first frame update
    void Start()
    {
        aiPath = GetComponent<AIPath>();
        destinationSetter = GetComponent<AIDestinationSetter>();
        //Player is target
        destinationSetter.target = GameObject.FindGameObjectWithTag("Player").transform;
        //get what room you are in
        roomActivation.mono = this;
        roomActivation._init_(this);
        //init sight
        sight._init_(this, 0.001f);


    }
    private void Update()
    {
        aiPath.canMove = sight.See() && roomActivation.Check();

        
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
}
