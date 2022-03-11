using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    // Declaring variables
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;
    public GameObject camera;
    Vector2 movement;
    public Transform attackPoint;
    public float attackRange = .5f;
    public LayerMask enemyLayers;
    private float timeBtwAttack;
    public float startTimeBtwAttack;

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
        Vector3 target = camera.transform.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));

        // Weird rotation thing
        Vector3 difference = target - transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        Vector3 positionMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
 
        // make the Z position equal to the player for a fully 2D comparison
        positionMouse.z = transform.position.z;

        // Debug.Log("Player: " + transform.position);
        // Debug.Log("Mouse: " + positionMouse);
    
        // Vector3 towardsMouseFromPlayer = positionMouse - transform.position;
        // Vector3 vectorAttack = towardsMouseFromPlayer.normalized;
        // attackPoint.position = vectorAttack;
        //= vectorAttack;

        // Debug.Log("Hit: " + vectorAttack);
        // Debug.Log("ATTX: " + attackPoint.position);

        if(timeBtwAttack <= 0){
            if(Input.GetKey(KeyCode.Space)){
                animator.SetFloat("LastH", difference.x);
                animator.SetFloat("LastV", difference.y);
                rb.velocity = Vector3.zero;
                Attack();
        }
            timeBtwAttack = startTimeBtwAttack;
        }else{
            timeBtwAttack -= Time.deltaTime;
        }
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
