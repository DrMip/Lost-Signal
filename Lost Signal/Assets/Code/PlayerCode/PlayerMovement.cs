using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using UnityEditor.Callbacks;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //components of the player
    private Rigidbody2D rb;
    private Animator ani;
    private JumpScript jmp;
    private PlayerBehavior pb;
    
    //other components
    [SerializeField] private Transform groundSensor;
    //variables for checking surroundings
    public bool isGrounded;
    public bool wasGrounded;
    bool isStuckOnLeftWall;
    bool isStuckOnRightWall;
    //variables for movement x,y 
    //private float movementSpeed;
    float movement;
    bool lookingRight = true;
    //variables for jumping
    //private float jumpStrength;
    bool jetting;
    


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        jmp = GetComponent<JumpScript>();
        pb = GetComponent<PlayerBehavior>();
        transform.position = new Vector3 (-4,0);

        //initialize constants
        //movementSpeed = pb.MovementSpeed;
        //jumpStrength = pb.JumpStrength;
    }
    void Update()
    {
        CheckSurroundings();
        //gets input
        movement = Input.GetAxis("Horizontal");

        //checks if needs to flip direction
        if(movement > 0 && !lookingRight)
        {
            Flip();
        }
        if(movement < 0 && lookingRight)
        {
            Flip();
        }
        //if is pressing jet, and is registed as grounded and has jetpower say jetting is true
        if(Input.GetButton("Jetpack") && jmp.delayedIsGrounded && pb.JetTimeCounter != 0)
        {
            jetting = true;
        }
        else
            jetting = false;
        
        HandleAnimations();

        
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        

        // if stuck on wall dont try to move in its direction
        if((!isStuckOnLeftWall || movement >= 0) && (!isStuckOnRightWall || movement <= 0))
        {
            transform.position += new Vector3(movement*pb.MovementSpeed* 0.1f , 0, 0);
        }
        if(jmp.Jump|| jetting)
            Debug.Log(jmp.Jump + " " + jetting);
        //if jump and not jetting
        if(jmp.Jump && !jetting)
        {
            rb.AddForce(Vector2.up * pb.JumpStrength * 100, ForceMode2D.Impulse);
            jmp.Jump = false;
        }
        else
            jmp.Jump = false;
        
        
        wasGrounded = isGrounded;
    }

    void CheckSurroundings()
    {
        //checks if grounded
        isGrounded = Physics2D.CircleCast(new Vector2(groundSensor.position.x, groundSensor.position.y), 0.1f, Vector2.zero, 0, LayerMask.GetMask("Middleground"));
        //checks if next to a wall
        isStuckOnLeftWall = Physics2D.BoxCast(new Vector2(transform.position.x-0.3f,transform.position.y), new Vector2(0.7f,1.2f), 0.0f, Vector2.right, 0, LayerMask.GetMask("Middleground"));
        isStuckOnRightWall = Physics2D.BoxCast(new Vector2(transform.position.x+0.4f,transform.position.y), new Vector2(0.7f,1.2f), 0.0f, Vector2.right, 0, LayerMask.GetMask("Middleground"));
        //Debug.Log(isStuckOnRightWall);
    }

    void HandleMovements()
    {

    }

    void Flip()
    {
        lookingRight = !lookingRight;
        //takes the local scale and flips x axis
        Vector3 Scale = transform.localScale;
        Scale.x *= -1;
        transform.localScale = Scale;
    }

    void HandleAnimations()
    {   
        //set Speed for running animations
        ani.SetFloat("Speed", Mathf.Abs(movement));
        //get check if jumping up or falling
        if(!isGrounded)
        {   
            //if going up then jumping if going down than falling
            if(rb.velocity.y >= 0)
            {
                ani.SetBool("IsJumping", true);
                ani.SetBool("IsFalling", false);
            }
            else if(rb.velocity.y < 0)
            {
                ani.SetBool("IsFalling", true);
                ani.SetBool("IsJumping", false);
            }
        }
        else
        {
            //resets jumping animation
            ani.SetBool("IsFalling", false);
            ani.SetBool("IsJumping", false);
        }


    }


}
