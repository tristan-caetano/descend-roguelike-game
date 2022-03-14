// Tristan Caetano, Samuel Rouillard, Elijah Karpf
// Descend Project
// CIS 464 Project 1

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Script for player movement and melee attack
public class PlayerMovement : MonoBehaviour
{

    // Making sure the player can move and is animated
     Vector2 movement;
    public float moveSpeed = 3f;
    public Rigidbody2D rb;
    public Animator animator;

    // Getting health of the player
    public PlayerAttributes player;

    // The camera allows us to get the mouse position so we know what direction to attack in
    public GameObject camera;
   
    // Making sure the player can attack the enemy and attack cooldown
    public Transform attackPoint;
    public float attackRange = .5f;
    public LayerMask enemyLayers;
    public bool isAvailable = true;
    public float cooldownDuration = 1.0f;
    public int attackDamage = 5;

    // Heal prefab and cooldown
    public bool canHeal = true;
    public GameObject healSpell;
    public float healCoolDuration = 30f; 
    public int healAmt = 30;
    int startTime;
    int currTime;
    int diffTime = 30;
    

    // Getting player input, making sure the player is alive, and animating the player
    void Update()
    {

        // Making sure the player is alive
        if(player.getHealth() > 0){

            // Sprint when user holds down left shift
            // if (Input.GetKey(KeyCode.LeftShift))
            // {
            //     moveSpeed = 10f;
            // }else{
            //     moveSpeed = 5f;
            // }

            // Values for mana 
            currTime = System.DateTime.Now.Second;
            if(diffTime < 30){ diffTime = currTime - startTime; if(diffTime < 0){diffTime = diffTime + 60;}}
            if(diffTime >= 30){diffTime = 30;}
            Debug.Log("Diff: " + diffTime + " Curr: " + currTime + " Start: " + startTime);
            
            // Heal when user presses f
            if (Input.GetKey(KeyCode.F) && player.getHealth() < player.maxHealth && canHeal)
            {
                startTime = System.DateTime.Now.Second;
                StartCoroutine(HealCooldown());
                animator.SetFloat("Horizontal", movement.x);
                animator.SetFloat("Vertical", movement.y);
                animator.SetTrigger("Cast");
                player.Heal(healAmt);
                Destroy(healSpell, 2f);
                Instantiate(healSpell, player.transform.position, Quaternion.Euler(0.0f, 0.0f, 0.0f));
            }

            // Player movement input
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            // Animation applied to movement
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Speed", movement.sqrMagnitude);
        }
    }

    // Actually moving the player and attack cooldown
    void FixedUpdate()
    {
        // Making sure the player is alive
        if(player.getHealth() > 0){ 

            // Player movement using the rigidbody
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

            // If the attack is not in cooldown, pressing space will attack
            if(isAvailable){
                if(Input.GetKey(KeyCode.Space)){
                    Attack();
                    StartCoroutine(StartCooldown());
                }
            } 
            else {return;}
        }
    }

    // Attack method
    void Attack(){

        // Getting mouse location so the player will hit in the direction of the mouse
        Vector3 target = camera.transform.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
        Vector3 difference = target - transform.position;
        Vector3 positionMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        positionMouse.z = transform.position.z;

        // Temporarily stopping the player
        rb.velocity = Vector3.zero;

        // Animating the attack
        animator.SetFloat("LastH", difference.x);
        animator.SetFloat("LastV", difference.y);
        animator.SetTrigger("Attack");

        // Checking if the player hit an enemy
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach(Collider2D enemy in hitEnemies){

            EnemyAttributes currEnemy = enemy.GetComponent<EnemyAttributes>();

            // If the enemy hit, it takes damage   
            if(currEnemy.getHealth() != null){
                currEnemy.TakeDamage(attackDamage);
            }
        }
    }

    // Drawing the attack radius
    void OnDrawGizmosSelected(){
        if(attackPoint == null){return;}
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    // Timer for cooldown
    public IEnumerator StartCooldown(){
            isAvailable = false;
            yield return new WaitForSeconds(cooldownDuration);
            isAvailable = true;
        }

    // Timer for heal cooldown
    public IEnumerator HealCooldown(){
            canHeal = false;
            diffTime = 0;
            yield return new WaitForSeconds(healCoolDuration);
            canHeal = true;
        }
    
}
