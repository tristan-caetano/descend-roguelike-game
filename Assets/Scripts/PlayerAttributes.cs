using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttributes : MonoBehaviour
{
    public int health = 100;
    public Rigidbody2D rb;
    public Animator animator;
    float flashTime = .5f;
    public PlayerMovement player;
    public Collider2D playerCollider;
    Color originalColor;
    public SpriteRenderer renderer;
    void Start()
    {
        originalColor = renderer.color;
    }
    void FlashRed()
    {
        renderer.color = Color.red;
        Invoke("ResetColor", flashTime);
    }
    void ResetColor()
    {
        renderer.color = originalColor;
    }
    public void TakeDamage(int damage){

        if(health > 0){
            StartCoroutine(StartCooldown());
            health -= damage;
            FlashRed();
            if(health <= 0){
                Die();
            }else{
                animator.SetTrigger("Hit");
            }
        }
    }

    void Die(){
        animator.SetTrigger("Dead");
        
        //Destroy(gameObject, 5f);
    }

    public int getHealth(){
        return health;
    }

    public IEnumerator StartCooldown()
     {
         playerCollider.enabled = false;
         yield return new WaitForSeconds(2f);
         playerCollider.enabled = true;
     }

}
