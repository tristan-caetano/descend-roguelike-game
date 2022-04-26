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
    public float attackRange = 10f;
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
    public bool canMove = false;

    // Player dialogue
    public GameObject dialogueManager;
    public DialogueManager dlScript;

    bool boss1Dial = false;
    bool boss1DeadDial = false;
    GameObject boss1;
    Transform boss1Trans;
    EnemyAttributes boss1Att;

    bool boss2Dial = false;
    bool boss2DeadDial = false;
    GameObject boss2;
    Transform boss2Trans;
    EnemyAttributes boss2Att;

    bool boss3Dial = false;
    bool boss3DeadDial = false;
    GameObject boss3;
    Transform boss3Trans;
    EnemyAttributes boss3Att;

    void Start(){
        StartCoroutine(StartDialogue());
    }
    
    // Getting player input, making sure the player is alive, and animating the player
    void Update()
    {
        float pythagDis = 0f;

        // Boss 1 Dialogue
        if(boss1 == null && !boss1DeadDial){
            boss1 = GameObject.Find("The_Gatekeeper");
            boss1Att = boss1.GetComponent<EnemyAttributes>();
            boss1Trans = boss1.transform;
        }else{

            if(!boss1DeadDial){
                pythagDis = Mathf.Sqrt(Mathf.Pow(Mathf.Abs(boss1Trans.position.x - rb.position.x) + Mathf.Abs(boss1Trans.position.y - rb.position.y), 2f));
            }

            if(pythagDis < 14 && !boss1Dial){
                StartCoroutine(EnterBoss1());
                boss1Dial = true;
            }

            if(boss1Att.health <= 0 && !boss1DeadDial){
                StartCoroutine(Boss1Dead());
                boss1DeadDial = true;
            }
        }

        // Boss 2 Dialogue
        if(boss2 == null && !boss2DeadDial){
            boss2 = GameObject.Find("The_Corrupted");
            boss2Att = boss2.GetComponent<EnemyAttributes>();
            boss2Trans = boss2.transform;
        }else{

            if(!boss2DeadDial){
                pythagDis = Mathf.Sqrt(Mathf.Pow(Mathf.Abs(boss2Trans.position.x - rb.position.x) + Mathf.Abs(boss2Trans.position.y - rb.position.y), 2f));
            }

            if(pythagDis < 14 && !boss2Dial){
                StartCoroutine(EnterBoss2());
                boss2Dial = true;
            }

            if(boss2Att.health <= 0 && !boss2DeadDial){
                StartCoroutine(Boss2Dead());
                boss2DeadDial = true;
            }
        }

        // Boss 3 Dialogue
        if(boss3 == null && !boss3DeadDial){
            boss3 = GameObject.Find("The_Sister");
            boss3Att = boss3.GetComponent<EnemyAttributes>();
            boss3Trans = boss3.transform;
        }else{

            if(!boss3DeadDial){
                pythagDis = Mathf.Sqrt(Mathf.Pow(Mathf.Abs(boss3Trans.position.x - rb.position.x) + Mathf.Abs(boss3Trans.position.y - rb.position.y), 2f));
            }
            if(pythagDis < 14 && !boss3Dial){
                StartCoroutine(EnterBoss3());
                boss3Dial = true;
            }

            if(boss3Att.health <= 0 && !boss3DeadDial){
                StartCoroutine(Boss3Dead());
                boss3DeadDial = true;
            }

        }

        // if(canMove){

            if(winPoint == null){
                winPoint = GameObject.Find("WinPoint");
            }

            // Making sure the player is alive
            if(player.health > 0){   
            
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
                    isAvailable = false;
                    StartCoroutine(StartCooldown());
                    Attack();
                }
            } 
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

    // Cooldown timer
    public IEnumerator StartDialogue(){
        yield return new WaitForSeconds(1f);
        dlScript.PlayDialogue1();
        yield return new WaitForSeconds(4f);
        dlScript.PlayDialogue2();
        yield return new WaitForSeconds(3f);
        dlScript.PlayDialogue("A fire spell? I wonder what it does.");
        yield return new WaitForSeconds(3f);
        dlScript.RemoveDialogue();
    }

    // Cooldown timer
    public IEnumerator EnterBoss1(){
        yield return new WaitForSeconds(1f);
        dlScript.PlayDialogue("Man, you are HUGE!");
        yield return new WaitForSeconds(4f);
        dlScript.RemoveDialogue();
    }

    // Cooldown timer
    public IEnumerator Boss1Dead(){
        yield return new WaitForSeconds(1f);
        dlScript.PlayDialogue3();
        yield return new WaitForSeconds(4f);
        dlScript.PlayDialogue4();
        yield return new WaitForSeconds(3f);
        dlScript.RemoveDialogue();
    }

    // Cooldown timer
    public IEnumerator EnterBoss2(){
        yield return new WaitForSeconds(1f);
        dlScript.PlayDialogue("So you're huge AND weird, ugh...");
        yield return new WaitForSeconds(4f);
        dlScript.RemoveDialogue();
    }

    // Cooldown timer
    public IEnumerator Boss2Dead(){
        yield return new WaitForSeconds(1f);
        dlScript.PlayDialogue5();
        yield return new WaitForSeconds(4f);
        dlScript.PlayDialogue6();
        yield return new WaitForSeconds(3f);
        dlScript.RemoveDialogue();
    }

    // Cooldown timer
    public IEnumerator EnterBoss3(){
        yield return new WaitForSeconds(1f);
        dlScript.PlayDialogue7();
        yield return new WaitForSeconds(4f);
        dlScript.PlayDialogue8();
        yield return new WaitForSeconds(3f);
        dlScript.RemoveDialogue();
    }

    // Cooldown timer
    public IEnumerator Boss3Dead(){
        yield return new WaitForSeconds(1f);
        dlScript.PlayDialogue9();
        yield return new WaitForSeconds(3f);
        FindObjectOfType<AudioManager>().Play("Win");
        winMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }
}
