using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttributes : MonoBehaviour
{
    public int health = 100;
    public Rigidbody2D rb;
    public Animator animator;
    float flashMime = .5f;
    public EnemyAI enemy;
    Color originalColor;
    public SpriteRenderer renderer;
    void Start()
    {
        originalColor = renderer.color;
    }
    void FlashRed()
    {
        renderer.color = Color.red;
        Invoke("ResetColor", flashMime);
    }
    void ResetColor()
    {
        renderer.color = originalColor;
    }
    public void TakeDamage(int damage){

        health -= damage;
        FlashRed();

        if(health <= 0){
            Die();
        }else{
            animator.SetTrigger("isHit");
        }
        
    }

    void Die(){
        animator.SetTrigger("isDead");
        
        Destroy(gameObject, 5f);
    }

    public int getHealth(){
        return health;
    }

}
