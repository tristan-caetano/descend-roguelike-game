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
    public string BossWinSound;

    // Enemy health
    public int health = 100;

    public int maxHealth = 100;

    // Animating when the player is hit or dead
    public Animator animator;

    // Health Bar initialize
    public HealthBar healthBar;

    // Makes the enemy red for a short time after getting hit
    float flashMime = .5f;
    Color originalColor;
    public SpriteRenderer renderer;

    // Potions
    public GameObject healthPotion;
    public GameObject manaPotion;

    // AI Script
    public EnemyAI enemyAI;
    public byte boss;

    // Getting the original sprite color
    void Start(){
        originalColor = renderer.color;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Making the sprite red
    void FlashRed(){renderer.color = Color.red; Invoke("ResetColor", flashMime);}

    // Resetting the sprite back to the original color
    void ResetColor(){renderer.color = originalColor;}

    // If the enemy is hit, they take damage
    public void TakeDamage(int damage){

        // Getting boss val
        boss = enemyAI.boss;

        // Removes damage from health
        health -= damage;

        // Flashes the enemy red
        FlashRed();

        // If the enemy dies, call die method
        if(health <= 0){
            Die();
            FindObjectOfType<AudioManager>().Play(BossWinSound);
        
        // If they are alive, play the hit animation
        }
        else {
            FindObjectOfType<AudioManager>().Play(HitSound);
            animator.SetTrigger("isHit");
        }

        if(boss > 0){
            // Determines if an enemy drops an item or not
            var rand = new System.Random();
            int num = rand.Next(1,21);
            Debug.Log("CHANCE: " + num);

            if(num < 2){
                num = rand.Next(1,3);

                if(num > 1){
                    // Potion is placed where the enemy dies
                    Instantiate(healthPotion, transform.position, transform.rotation);
                }else{
                    // Potion is placed where the enemy dies
                    Instantiate(manaPotion, transform.position, transform.rotation);
                }
            }
        }
    }

    // void FixedUpdate(){
    //     healthBar.SetHealth(health);
    // }

    // Enemy plays dead animation, and is removed from the scene
    void Die(){

        // Death animation is called
        animator.SetTrigger("isDead");

        // Death sound
        FindObjectOfType<AudioManager>().Play(DeathSound);
        
        // Enemy is removed from scene after 5 seconds
        Destroy(gameObject, 5f);

        // Determines if an enemy drops an item or not
        var rand = new System.Random();
        int num = rand.Next(1,11);

        if(num > 5){
            num = rand.Next(1,3);

            if(num > 1){
                // Potion is placed where the enemy dies
                Instantiate(healthPotion, transform.position, transform.rotation);
            }else{
                // Potion is placed where the enemy dies
                Instantiate(manaPotion, transform.position, transform.rotation);
            }
        }
    }
}
