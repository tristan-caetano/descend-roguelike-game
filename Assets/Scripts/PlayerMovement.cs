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
    public bool isAvailable = true;
    public float cooldownDuration = 1.0f;

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

        if(isAvailable){
            if(Input.GetKey(KeyCode.Space)){
                Attack();
                StartCoroutine(StartCooldown());
            }
            
        }else{
            return;
        }
    }

    void Attack(){

        Vector3 target = camera.transform.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));

        // Weird rotation thing
        Vector3 difference = target - transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        Vector3 positionMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // make the Z position equal to the player for a fully 2D comparison
        positionMouse.z = transform.position.z;

        animator.SetFloat("LastH", difference.x);
        animator.SetFloat("LastV", difference.y);
        rb.velocity = Vector3.zero;
        animator.SetTrigger("Attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach(Collider2D enemy in hitEnemies){

             EnemyAttributes currEnemy = enemy.GetComponent<EnemyAttributes>();

            if(currEnemy.getHealth() != null){
                currEnemy.TakeDamage(5);
            }
        }
    }

    void OnDrawGizmosSelected(){

        if(attackPoint == null){
            return;
        }

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public IEnumerator StartCooldown(){
            isAvailable = false;
            yield return new WaitForSeconds(cooldownDuration);
            isAvailable = true;
        }
    
}
