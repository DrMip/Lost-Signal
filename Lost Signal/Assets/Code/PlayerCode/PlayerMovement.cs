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
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.CircleCast(new Vector2(groundSensor.position.x, groundSensor.position.y), 0.1f, Vector2.zero);
        // movement on x axis
        float movement = Input.GetAxis("Horizontal");
        Debug.Log(isGrounded);
        rb.AddForce(new Vector2(movement*100*movementSpeed, 0), ForceMode2D.Force);
        
        if(isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector2.up * jumpStrength, ForceMode2D.Impulse);
        }
        


    }
}
