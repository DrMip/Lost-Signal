using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    //player Behavior
    PlayerBehavior pb;
    //rigid body
    Rigidbody2D rb;
    //variables
    float xdirect;
    //bool dash;
    public bool isDashing;
    // Start is called before the first frame update
    void Start()
    {
        pb = GetComponent<PlayerBehavior>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Dash") && !isDashing)
        {
            xdirect = (transform.localScale.x > 0) ? 1: (-1);
            isDashing = true;
        }

    }
    void FixedUpdate()
    {
        if(isDashing)
        {
            StartCoroutine(DoDash());
            transform.position += new Vector3(xdirect*pb.DashStrength*0.1f, 0, 0);
        }
    }
    IEnumerator DoDash()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
        yield return new WaitForSeconds(pb.DashDuration);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        //rb.velocity = new Vector2(xdirect* 0.01f ,0);
        isDashing = false;

    }
}
