using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttributes : MonoBehaviour
{
    public int health = 100;

    public Animator animator;

    public void TakeDamage(int damage){

        health -= damage;

        if(health < 0){
            Die();
        }

    }

    void Die(){
        animator.SetTrigger("isDead");
        Destroy(gameObject, 5f);
    }

}
