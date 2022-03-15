// Tristan Caetano, Samuel Rouillard, Elijah Karpf
// Descend Project
// CIS 464 Project 1

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Simple script for enemy health, hit, and death animations
public class EnemyAttributes : MonoBehaviour
{
    // Enemy sounds
    public string HitSound;
    public string DeathSound;

    // Enemy health
    public int health = 100;

    // Animating when the player is hit or dead
    public Animator animator;

    // Makes the enemy red for a short time after getting hit
    float flashMime = .5f;
    Color originalColor;
    public SpriteRenderer renderer;

    // Getting the original sprite color
    void Start(){originalColor = renderer.color;}

    // Making the sprite red
    void FlashRed(){renderer.color = Color.red; Invoke("ResetColor", flashMime);}

    // Resetting the sprite back to the original color
    void ResetColor(){renderer.color = originalColor;}

    // If the enemy is hit, they take damage
    public void TakeDamage(int damage){

        // Removes damage from health
        health -= damage;

        // Flashes the enemy red
        FlashRed();

        // If the enemy dies, call die method
        if(health <= 0){
            Die();
        
        // If they are alive, play the hit animation
        }
        else {
            FindObjectOfType<AudioManager>().Play(HitSound);
            animator.SetTrigger("isHit");
        }
    }

    // Enemy plays dead animation, and is removed from the scene
    void Die(){

        // Death animation is called
        animator.SetTrigger("isDead");

        // Death sound
        FindObjectOfType<AudioManager>().Play(DeathSound);
        
        // Enemy is removed from scene after 5 seconds
        Destroy(gameObject, 5f);
    }

    // Getter for enemy health
    public int getHealth(){return health;}
}
