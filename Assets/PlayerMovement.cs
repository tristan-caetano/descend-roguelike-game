using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    // Declaring variables
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;
    Vector2 movement;
    public Transform attackPoint;
    public float attackRange = .5f;
    public LayerMask enemyLayers;

    

    // Update is called once per frame
    void Update()
    {
        
        // Sprint when user holds down left shift
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = 10f;
        }else{
            moveSpeed = 5f;
        }

        // Player movement input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        //Animation applied to movement (not implemented)
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

    }

    void FixedUpdate()
    {
        
        // Player movement
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

}
