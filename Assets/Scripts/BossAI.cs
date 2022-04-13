// Tristan Caetano, Samuel Rouillard, Elijah Karpf
// Descend Project
// CIS 464 Project 1

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

// Script that controls the enemy AI
public class BossAI : MonoBehaviour {

    // Attack sound for enemy
    //public string AttackSound;

    // Player variables to change player health, identify, and track the player
    public PlayerAttributes playerAtt;
    public LayerMask playerLayers;
    public Transform target;
    GameObject mainPlayer;

    // Denotes the top speed and current speed of the enemy
    public float speed = 200f;
    float currSpeed;
    
    // Variables that allow the enemy to track the player
    Path path;
    int currentWaypoint = 0;
    public float nextWaypointDistance = 3f;
    Seeker seeker;

    // Standard attributes of the enemy so that it can move, animate, hit, and check health
    public BossAttributes enemy;
    Rigidbody2D rb;
    public Animator animator;
    public Collider2D collider;
    public Transform enemyAttackPoint;
    public float enemyAttackRange = 1f;
    
    // Gets the player info and does damage
    Collider2D hitInfoLocal = null;
    public int enemyAttack = 5;
    

    // Gets the rigidbody and seeker for tracking, starts tracking
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        
        InvokeRepeating("UpdatePath", 0f, .5f);
        
    }

    // Methods relating to pathing.
    void UpdatePath(){
        if (seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path p) {
        if(!p.error) {
            path = p;
            currentWaypoint = 0;
        }
    }

    public Vector2 getDirection(){
        return new Vector2(rb.velocity.x, rb.velocity.y);
    }

    // Method that is called regularly to update pathing waypoints, check enemy health, check for player collision, etc
    void FixedUpdate()
    {

        // Finds the hypotenuse to check the distance between the player and enemy
        float pythagDis = Mathf.Sqrt(Mathf.Pow(Mathf.Abs(target.position.x - rb.position.x) + Mathf.Abs(target.position.y - rb.position.y), 2f));

        // If the player is close enough and the enemy is alive
        if(enemy.health > 0 && pythagDis < 20){

            // Enabling the collider if the player is close enough and the enemy is alive
            collider.enabled = true;

            // Attacks player if they are close enough
            if (enemy.health > 0 && playerAtt.health > 0 && pythagDis < 1.5f){
            //    FindObjectOfType<AudioManager>().Play(AttackSound);
                animator.SetTrigger("isAttack");
                Attack(hitInfoLocal);
            }

            // Pathing code
            if(path == null){
                currSpeed = speed;
                return;
            }
            if(currentWaypoint >= path.vectorPath.Count){
                currSpeed = speed;
                return;
            }else{
                currSpeed = speed;
            }

            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            Vector2 force =  direction * speed * Time.deltaTime;
        
            rb.AddForce(force);

            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);    
            
            if(distance < nextWaypointDistance){
                currentWaypoint ++;
            }

            // Activating animation to show the enemy moving
            animator.SetFloat("Horizontal", rb.velocity.x);
            animator.SetFloat("Vertical", rb.velocity.y);
            animator.SetFloat("lastMoveHorizontal", rb.velocity.x);
            animator.SetFloat("lastMoveVertical", rb.velocity.y);
            animator.SetFloat("Speed", currSpeed);
        
        }else{

            // Disabling the collider if the enemy is dead or if the player is too far
            collider.enabled = false;
            return;
        }
    }

    // Initializing the player as a gameobject so that the enemy can recognize and track the player
    void Update(){

        // Keeps checking if the player value is still null
        if(mainPlayer == null){
            mainPlayer = GameObject.Find("Main_Player");
            target  = mainPlayer.transform;
            playerAtt = mainPlayer.GetComponent<PlayerAttributes>();
        }else{
            return;
        }
    }

    // Gets hit info if the collider is triggered
    void OnTriggerEnter2D(Collider2D hitInfo){hitInfoLocal = hitInfo;}

    // Sends collider info to this method if the enemy successfully hit the player
    void Attack(Collider2D hitInfo){

        // Makes sure the collider info contains the player
        if(hitInfo == null){return;}

        // Making sure only to attack if the enemy is alive
        if(enemy.health > 0){

                Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(enemyAttackPoint.position, enemyAttackRange, playerLayers);

                // Actually damaging the player if they are found in the collider
                foreach(Collider2D player in hitPlayer){
                    PlayerAttributes currPlayer = hitInfo.GetComponent<PlayerAttributes>();
                    if(currPlayer.health > 0){
                        currPlayer.TakeDamage(enemyAttack);
                    }
            }

        }else{

            // Turning off the collider if the enemy is dead
            collider.enabled = false;
            return;
        }
    }
}