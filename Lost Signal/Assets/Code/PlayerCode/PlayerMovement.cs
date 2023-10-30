using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private int movementSpeed;
    [SerializeField] private int jumpStrength;
    
    [SerializeField] private Transform groundSensor;

    [SerializeField] private Rigidbody2D rb;

    bool isGrounded;
    bool wasGrounded;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isGrounded = Physics2D.CircleCast(new Vector2(groundSensor.position.x, groundSensor.position.y), 0.1f, Vector2.zero, 0, LayerMask.GetMask("Middleground"));
        // movement on x axis
        float movement = Input.GetAxis("Horizontal");

        
        transform.position += new Vector3(movement*movementSpeed* 0.1f , 0, 0);


        
        if(isGrounded && Input.GetButton("Jump") && wasGrounded == true)
        {
            rb.AddForce(Vector2.up * jumpStrength * 100, ForceMode2D.Impulse);
        }
        wasGrounded = isGrounded;
        


    }
}
