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
    public GameObject healSpell;
    public int healAmt = 30;
    bool canHeal = true;

    // Make sure the player can win
    public GameObject winPoint;
    public GameObject winMenuUI;

    // Make sure player can't move when game is paused
    bool canMove = false;
    
    // Getting player input, making sure the player is alive, and animating the player
    void Update()
    {

        if(canMove){

            if(winPoint == null){
                winPoint = GameObject.Find("WinPoint");
            }

            // Making sure the player is alive
            if(player.health > 0){

                // Finds the hypotenuse to check the distance between the player and enemy
                float winDis = Mathf.Sqrt(Mathf.Pow(Mathf.Abs(winPoint.transform.position.x - rb.position.x) + Mathf.Abs(winPoint.transform.position.y - rb.position.y), 2f));

                if(winDis < 1.5f){
                    FindObjectOfType<AudioManager>().Play("Win");
                    winMenuUI.SetActive(true);
                    Time.timeScale = 0f;
                }

                // Sprint when user holds down left shift
                // if (Input.GetKey(KeyCode.LeftShift))
                // {
                //     moveSpeed = 10f;
                // }else{
                //     moveSpeed = 5f;
                // }


                // Values for mana 
                // currTime = System.DateTime.Now.Second;
                // if(diffTime < 30){ diffTime = currTime - startTime; if(diffTime < 0){diffTime = diffTime + 60;}}
                // if(diffTime >= 30){diffTime = 30;}
                // manaBar.SetMana(diffTime);
                
                // Heal when user presses f
                if (Input.GetKey(KeyCode.F) && player.health < player.maxHealth && player.mana > 20 && canHeal)
                {
                    FindObjectOfType<AudioManager>().Play("Healing");
                    player.UseMana(20);
                    animator.SetFloat("Horizontal", movement.x);
                    animator.SetFloat("Vertical", movement.y);
                    animator.SetTrigger("Cast");
                    player.Heal(healAmt);
                    Destroy(healSpell, 2f);
                    Instantiate(healSpell, player.transform.position, Quaternion.Euler(0.0f, 0.0f, 0.0f));
                    StartCoroutine(HealCooldown());
                }

                // Player movement input
                movement.x = Input.GetAxisRaw("Horizontal");
                movement.y = Input.GetAxisRaw("Vertical");

                // Animation applied to movement
                animator.SetFloat("Horizontal", movement.x);
                animator.SetFloat("Vertical", movement.y);
                animator.SetFloat("Speed", movement.sqrMagnitude);
            }
        } else {
            StartCoroutine(StartGameCooldown());
        }
    }

    // Actually moving the player and attack cooldown
    void FixedUpdate()
    {
        // Making sure the player is alive
        if(player.health > 0){ 

            // Player movement using the rigidbody
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

            // If the attack is not in cooldown, pressing space will attack
            if(isAvailable){
                if(Input.GetKey(KeyCode.Space)){
                    Attack();
                    isAvailable = false;
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
            BossAttributes currBoss = enemy.GetComponent<BossAttributes>();

            // If the enemy hit, it takes damage   
            if(currEnemy.health > 0){
                currEnemy.TakeDamage(attackDamage);
                FindObjectOfType<AudioManager>().Play("Knife Hit");
            }
            if(currBoss.health > 0){
                currBoss.TakeDamage(attackDamage);
                FindObjectOfType<AudioManager>().Play("Knife Hit");
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

    // Cooldown timer
    public IEnumerator StartGameCooldown(){
        canMove = false;
        yield return new WaitForSeconds(.1f);
        canMove = true;
    }

    // Cooldown timer
    public IEnumerator HealCooldown(){
        canHeal = false;
        yield return new WaitForSeconds(2f);
        canHeal = true;
    }
    
}
