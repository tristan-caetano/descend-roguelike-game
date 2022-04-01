// Tristan Caetano, Samuel Rouillard, Elijah Karpf
// Descend Project
// CIS 464 Project 1

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Simple script for player health, hit, and death animations
public class PlayerAttributes : MonoBehaviour
{
    // Death screen object
    public GameObject deathMenuUI;

    // Player health and mana
    public int health = 100;
    public int maxHealth = 100;
    bool canBeDamaged = true;
    public int maxMana = 100;
    public int mana = 100;

    // Health and Mana Bar initialize
    public HealthBar healthBar;
    public ManaBar manabar;


    // Animating when the player is hit or dead
    public Animator animator;

    // Makes the player red for a short time after being hit
    float flashTime = .5f;
    Color originalColor;
    public SpriteRenderer renderer;

    // Player collider that can be turned off to get iframes
    public Collider2D playerCollider;
    
    // Getting the original sprite color
    void Start(){
        originalColor = renderer.color;
        healthBar.SetMaxHealth(maxHealth);
        manabar.SetMana(maxMana);
    }

    // Making the sprite red
    void FlashColor(Color color){renderer.color = color; Invoke("ResetColor", flashTime);}

    // Resetting the sprite back to the original color
    void ResetColor(){renderer.color = originalColor;}

    // If the player is hit, they take damage
    public void TakeDamage(int damage){

        // If the player is alive
        if(health > 0 && canBeDamaged){

            // Cooldown begins for iframes
            StartCoroutine(StartCooldown());

            // Damage is subtracted from health
            health -= damage;

            // Player flashes red
            FlashColor(Color.red);

            // If the player dies, call the die method
            if(health <= 0){
                Die();
            // If they are alive, play the hit animation
            }
            else{animator.SetTrigger("Hit");}
            FindObjectOfType<AudioManager>().Play("Player Get Hit");
        }
    }

    // If the player casts a spell, it uses mana
    public void UseMana(int usedMana){

        // Make sure the player is alive
        if(health > 0){
            mana -= usedMana;

        }
    }

    // If the player heals, they gain health and flash white
    public void Heal(int healAmt){

        // If the player is alive
        if(health > 0){

            // Damage is subtracted from health
            health += healAmt;

            // Keeping health at max
            if(health > maxHealth){
                health = maxHealth;
            }

            Color healColor = Color.cyan;
            healColor.a = .9f;

            // Player flashes red
            FlashColor(healColor);

            animator.SetTrigger("Cast");
        }
    }

    // If the player picks up mana potion
    public void RegenerateMana(int manaRegenAmt){

        // If the player is alive
        if(mana < maxMana){

            // Damage is subtracted from health
            mana += manaRegenAmt;

            // Keeping health at max
            if(mana > maxMana){
                mana = maxMana;
            }

            // Setting color for visual feedback
            Color regenManaColor = Color.green;
            regenManaColor.a = .9f;

            // Player flashes red
            FlashColor(regenManaColor);
        }

    }

    void FixedUpdate(){
        healthBar.SetHealth(health);
        manabar.SetMana(mana);
    }

    // Player plays death animation and is removed from the scene
    void Die(){
        animator.SetTrigger("Dead");
        FindObjectOfType<AudioManager>().Play("Death");
        deathMenuUI.SetActive(true);
        //Destroy(gameObject, 5f);
}

    // Cooldown for player iframes
    public IEnumerator StartCooldown()
     {
        canBeDamaged = false;
        yield return new WaitForSeconds(2f);
        canBeDamaged = true;
     }

}
