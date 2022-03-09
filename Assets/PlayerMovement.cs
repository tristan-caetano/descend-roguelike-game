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
    float lastH, lastV;
    public Transform attackPoint;
    public float attackRange = .5f;
    public LayerMask enemyLayers;

    void Start()
    {
       lastH = 0;
       lastV = 0;
    }
    

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

        if(movement.sqrMagnitude > 0.01){
            lastH = movement.x;
            lastV = movement.y;
        }

        

        //animator.SetBool("isAttack", false);

    }

    void FixedUpdate()
    {
        
        // Player movement
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

        if(Input.GetKey(KeyCode.Space)){
            animator.SetFloat("LastH", lastH);
            animator.SetFloat("LastV", lastV);
            Attack();
        }

        animator.SetBool("isAttack", false);

    }

    void Attack(){
        animator.SetTrigger("Attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach(Collider2D enemy in hitEnemies){
            Debug.Log("Hit!");
        }
    }

    void OnDrawGizmosSelected(){

        if(attackPoint == null){
            return;
        }

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
