using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour {

    public Transform target;
    public float speed = 200f;
    float currSpeed;
    public float nextWaypointDistance = 3f;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;
    public Animator animator;
    public EnemyAttributes enemy;
    public Collider2D collider;
    public Transform enemyAttackPoint;
    public float enemyAttackRange = .5f;
    public LayerMask playerLayers;
    public bool isReady = true;
    Collider2D hitInfoLocal = null;
    public PlayerAttributes playerAtt;



    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        
        InvokeRepeating("UpdatePath", 0f, .5f);
        
    }

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

    // Update is called once per frame
    void FixedUpdate()
    {
        
        Debug.Log(enemy.getHealth());
        
        if(enemy.getHealth() > 0){

            float pythagDis = Mathf.Sqrt(Mathf.Pow(Mathf.Abs(target.position.x - rb.position.x) + Mathf.Abs(target.position.y - rb.position.y), 2f));

            if(enemy.getHealth() > 0 && playerAtt.getHealth() > 0){
                if (pythagDis < 1.5f){
                    animator.SetTrigger("isAttack");
                    Attack(hitInfoLocal);
                }
            }
            collider.enabled = true;
            
            if(path == null){
                return;
            }
            if(currentWaypoint >= path.vectorPath.Count){
                reachedEndOfPath = true;
                currSpeed = 0;
                return;
            }else{
                reachedEndOfPath = false;
                currSpeed = speed;
            }

            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            Vector2 force =  direction * speed * Time.deltaTime;
        
            rb.AddForce(force);

            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);    
            
            if(distance < nextWaypointDistance){
                currentWaypoint ++;
            }

            animator.SetFloat("Horizontal", rb.velocity.x);
            animator.SetFloat("Vertical", rb.velocity.y);
            animator.SetFloat("lastMoveHorizontal", rb.velocity.x);
            animator.SetFloat("lastMoveVertical", rb.velocity.y);
            animator.SetFloat("Speed", currSpeed);
        
        }else{
            collider.enabled = false;
            return;
        }
        
    }

    void OnTriggerEnter2D(Collider2D hitInfo){hitInfoLocal = hitInfo;}

void Attack(Collider2D hitInfo){

    if(hitInfo == null){return;}

    if(enemy.getHealth() > 0){

            Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(enemyAttackPoint.position, enemyAttackRange, playerLayers);

            foreach(Collider2D player in hitPlayer){
                PlayerAttributes currPlayer = hitInfo.GetComponent<PlayerAttributes>();
                if(currPlayer.getHealth() != null){
                    currPlayer.TakeDamage(50);
                }
        }

    }else{
        collider.enabled = false;
        return;
    }
}

}

// ⢀⣠⣾⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⠀⠀⠀⠀⣠⣤⣶⣶
// ⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⠀⠀⠀⢰⣿⣿⣿⣿
// ⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣧⣀⣀⣾⣿⣿⣿⣿
// ⣿⣿⣿⣿⣿⡏⠉⠛⢿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡿⣿
// ⣿⣿⣿⣿⣿⣿⠀⠀⠀⠈⠛⢿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⠿⠛⠉⠁⠀⣿
// ⣿⣿⣿⣿⣿⣿⣧⡀⠀⠀⠀⠀⠙⠿⠿⠿⠻⠿⠿⠟⠿⠛⠉⠀⠀⠀⠀⠀⣸⣿
// ⣿⣿⣿⣿⣿⣿⣿⣷⣄⠀⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣴⣿⣿
// ⣿⣿⣿⣿⣿⣿⣿⣿⣿⠏⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠠⣴⣿⣿⣿⣿
// ⣿⣿⣿⣿⣿⣿⣿⣿⡟⠀⠀⢰⣹⡆⠀⠀⠀⠀⠀⠀⣭⣷⠀⠀⠀⠸⣿⣿⣿⣿
// ⣿⣿⣿⣿⣿⣿⣿⣿⠃⠀⠀⠈⠉⠀⠀⠤⠄⠀⠀⠀⠉⠁⠀⠀⠀⠀⢿⣿⣿⣿
// ⣿⣿⣿⣿⣿⣿⣿⣿⢾⣿⣷⠀⠀⠀⠀⡠⠤⢄⠀⠀⠀⠠⣿⣿⣷⠀⢸⣿⣿⣿
// ⣿⣿⣿⣿⣿⣿⣿⣿⡀⠉⠀⠀⠀⠀⠀⢄⠀⢀⠀⠀⠀⠀⠉⠉⠁⠀⠀⣿⣿⣿
// ⣿⣿⣿⣿⣿⣿⣿⣿⣧⠀⠀⠀⠀⠀⠀⠀⠈⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢹⣿⣿
// ⣿⣿⣿⣿⣿⣿⣿⣿⣿⠃⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⣿⣿