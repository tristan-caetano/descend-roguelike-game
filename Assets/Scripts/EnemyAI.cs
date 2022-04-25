// Tristan Caetano, Samuel Rouillard, Elijah Karpf
// Descend Project
// CIS 464 Project 1

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

// Script that controls the enemy AI
public class EnemyAI : MonoBehaviour {

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
    float enemyDis = 1.5f;

    // Standard attributes of the enemy so that it can move, animate, hit, and check health
    public EnemyAttributes enemy;
    Rigidbody2D rb;
    public Animator animator;
    public Collider2D collider;
    public Transform enemyAttackPoint;
    public float enemyAttackRange = 1f;
    public byte boss = 0;
    
    // Gets the player info and does damage
    Collider2D hitInfoLocal = null;
    public int enemyAttack = 5;

    // Boss magic info
    public GameObject magicSpell;
    public GameObject magicSpell2;
    public GameObject magicSpell3;
    public GameObject magicSpell4;
    int x = 0;
    bool readyToFire = true;
    public byte castAmt = 5;
    byte currCast = 0;
    

    // Gets the rigidbody and seeker for tracking, starts tracking
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        
        InvokeRepeating("UpdatePath", 0f, .5f);

        if(boss > 0){
            enemyDis *= 2f;
        }
        
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
            if (enemy.health > 0 && playerAtt.health > 0 && pythagDis < enemyDis){
            //    FindObjectOfType<AudioManager>().Play(AttackSound);
                animator.SetTrigger("isAttack");
                Attack(hitInfoLocal);
            }

            // Attacks player if they are close enough
            if (enemy.health > 0 && playerAtt.health > 0 && pythagDis > 5 && boss == 1){
            //    FindObjectOfType<AudioManager>().Play(AttackSound);
                
                if(readyToFire){
                    animator.SetTrigger("isAttack");
                    for(x = 0; x < castAmt; x ++){

                        if(magicSpell){shootAOE(magicSpell);}
                        if(magicSpell2){shootAOE(magicSpell2);}
                            
                   }
                    readyToFire = false;
                    StartCoroutine(StartCooldown());
                }
            // Second Boss spell
            }else if (enemy.health > 0 && playerAtt.health > 0 && pythagDis > 1 && boss == 2){
            //    FindObjectOfType<AudioManager>().Play(AttackSound);
                
                if(readyToFire){
                    for(x = 0; x < castAmt; x ++){

                        if(magicSpell){shootAOE(magicSpell);}
                        if(magicSpell2){shootAOE(magicSpell2);}
                            
                   }
                    readyToFire = false;
                    StartCoroutine(StartCooldown2());
                }

            // Final Boss spell
            }else if(enemy.health > 0 && playerAtt.health > 0 && pythagDis > 5 && boss == 3){
                
                if(readyToFire){
                    animator.SetTrigger("isCast");

                        if(currCast == 0){shootAOE(magicSpell); currCast ++; readyToFire = false;}
                        else if(currCast == 1){shootAOE(magicSpell2); currCast ++; readyToFire = false;}
                        else if(currCast == 2){shootAOE(magicSpell3); currCast ++; readyToFire = false;}
                        else if(currCast == 3){shootAOE(magicSpell4); currCast ++; readyToFire = false;}
                        else{currCast = 0;}
                            
                    readyToFire = false;
                    StartCoroutine(StartCooldown3());
                }
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

    // Enemy shooting spell
    void shootAOE(GameObject currSpell){

        // Getting the direction of the mouseclick for firing the spell and playing the correct animation
        Vector3 difference = target.transform.position - gameObject.transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

            // If the mouse is clicked and the player is alive
            if(enemy.health > 0){

                // Animate the casting
                
                FindObjectOfType<AudioManager>().Play("Shoot Spell");

                // Get the direction and distance from the mouse click
                float distance = difference.magnitude;
                Vector2 direction = difference / distance;
                direction.Normalize();
                rb.velocity = Vector3.zero;

                // Cast the spell
                Instantiate(currSpell, gameObject.transform.position, Quaternion.Euler(0.0f, 0.0f, rotationZ));
            }
    }

    // Cooldown timer
    public IEnumerator StartCooldown(){
        readyToFire = false;
        yield return new WaitForSeconds(2.5f);
        readyToFire = true;
    }

    // Cooldown timer 2nd boss
    public IEnumerator StartCooldown2(){
        readyToFire = false;
        yield return new WaitForSeconds(1f);
        readyToFire = true;
    }

    // Cooldown timer final boss
    public IEnumerator StartCooldown3(){
        readyToFire = false;
        yield return new WaitForSeconds(1.5f);
        readyToFire = true;
    }
}