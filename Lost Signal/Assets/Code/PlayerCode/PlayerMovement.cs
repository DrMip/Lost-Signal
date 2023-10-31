using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private int movementSpeed;
    [SerializeField] private int jumpStrength;
    
    [SerializeField] private Transform groundSensor;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float delayJump  = 0.5f;
    private float delayCounter;
    bool isGrounded;
    bool wasGrounded;
    bool isStuckOnLeftWall;
    bool isStuckOnRightWall;
    bool isOnStairLeft;
    bool isOnStairRight;



    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //checking surroundings
        isGrounded = Physics2D.CircleCast(new Vector2(groundSensor.position.x, groundSensor.position.y), 0.1f, Vector2.zero, 0, LayerMask.GetMask("Middleground"));
        isStuckOnLeftWall = Physics2D.BoxCast(new Vector2(transform.position.x-0.3f,transform.position.y), new Vector2(0.55f,0.6f), 0.0f, Vector2.right, 0, LayerMask.GetMask("Middleground"));
        isStuckOnRightWall = Physics2D.BoxCast(new Vector2(transform.position.x+0.4f,transform.position.y), new Vector2(0.65f,0.6f), 0.0f, Vector2.right, 0, LayerMask.GetMask("Middleground"));
        //isOnStairLeft;
        //isOnStairRight;
        // movement on x axis
        float movement = Input.GetAxis("Horizontal");

        Debug.Log(isStuckOnLeftWall + " " + isStuckOnRightWall);
        // if stuck on wall dont try to move in its direction
        if((!isStuckOnLeftWall || movement >= 0) && (!isStuckOnRightWall || movement <= 0))
        {
            transform.position += new Vector3(movement*movementSpeed* 0.1f , 0, 0);
        }
        


        // if the player is on the ground, and was on the ground in the last frame and the player pressed jump and waited the delay, then jump
        if(isGrounded && Input.GetButton("Jump") && wasGrounded == true && delayCounter >= delayJump)
        {
            rb.AddForce(Vector2.up * jumpStrength * 100, ForceMode2D.Impulse);
        }
        wasGrounded = isGrounded;
        


    }

    void Update()
    {
        //use jump delay
        if(!isGrounded)
        {
            delayCounter = 0;
        }
        if(delayCounter < delayJump)
        {
            delayCounter += Time.deltaTime;
        }

    }
}
