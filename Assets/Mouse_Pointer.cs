using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse_Pointer : MonoBehaviour
{

    private Vector3 target;
    public GameObject crosshairs;
    public GameObject player;
    public GameObject magicPrefab;
    public Animator animator;
    public Transform attackPoint;
    public float attackRange = .5f;
    public float spellSpeed = 1f;
    float lastH, lastV;
    public LayerMask enemyLayers;
    public Vector3 mouseInput;

    // Start is called before the first frame update
    void Start(){
        Cursor.visible = false;
        lastH = 0;
        lastV = 0;
    
    }

    // Update is called once per frame
    void Update()
    {

        target = transform.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
        crosshairs.transform.position = new Vector2(target.x, target.y);

        // Weird rotation thing
        Vector3 difference = target - player.transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        if(Input.GetMouseButtonDown(0)){
            animator.SetFloat("LastH", difference.x);
            animator.SetFloat("LastV", difference.y);
            animator.SetTrigger("Cast");
            float distance = difference.magnitude;
            Vector2 direction = difference / distance;
            direction.Normalize();
            fireBullet(direction, rotationZ);
        }

    }

    void FixedUpdate(){

        target = transform.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
        crosshairs.transform.position = new Vector2(target.x, target.y);

        // Weird rotation thing
        Vector3 difference = target - player.transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        if(Input.GetKey(KeyCode.Space)){
            animator.SetFloat("LastH", difference.x);
            animator.SetFloat("LastV", difference.y);
            Attack();
        }
    }

// Method that calculated and fires spell
    void fireBullet(Vector2 direction, float rotationZ){
        GameObject b = Instantiate(magicPrefab) as GameObject;
        b.transform.position = player.transform.position;
        b.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
        b.GetComponent<Rigidbody2D>().velocity = direction * spellSpeed;
    }

// Method that calculates melee attack
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
