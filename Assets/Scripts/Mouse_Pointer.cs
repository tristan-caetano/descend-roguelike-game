using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse_Pointer : MonoBehaviour
{

    private Vector3 target;
    public GameObject crosshairs;
    public GameObject player;
    public Rigidbody2D playerBody;
    public GameObject magicPrefab;
    public Animator animator;
    public Transform attackPoint;
    public float attackRange = .5f;
    public float spellSpeed = .5f;
    public LayerMask enemyLayers;
    public Vector3 mouseInput;
    private float timeBtwSpell;
    public float startTimeBtwSpell;
    public Transform attackPos;
    public float cooldownDuration = 1f;
    public bool isAvailable = true;
    public PlayerAttributes playerAtt;

    // Start is called before the first frame update
    void Start(){
        Cursor.visible = false;

    }

    // Update is called once per frame
    void Update()
    {

        target = transform.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
        crosshairs.transform.position = new Vector2(target.x, target.y);
        Vector3 attackDir = (target - player.transform.position).normalized;

        // Weird rotation thing
        Vector3 difference = target - player.transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        if(isAvailable){

            if(Input.GetMouseButtonDown(0) && playerAtt.getHealth() > 0){
                animator.SetFloat("LastH", difference.x);
                animator.SetFloat("LastV", difference.y);
                animator.SetTrigger("Cast");
                float distance = difference.magnitude;
                Vector2 direction = difference / distance;
                direction.Normalize();
                playerBody.velocity = Vector3.zero;
                fireBullet(direction, rotationZ);
                StartCoroutine(StartCooldown());
            }
        }else{
            return;
        }
    }

// Method that calculated and fires spell
    void fireBullet(Vector2 direction, float rotationZ){
        Instantiate(magicPrefab, player.transform.position, Quaternion.Euler(0.0f, 0.0f, rotationZ));
    }

    public IEnumerator StartCooldown(){
        isAvailable = false;
        yield return new WaitForSeconds(cooldownDuration);
        isAvailable = true;
    }
}
