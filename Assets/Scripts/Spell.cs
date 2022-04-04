// Tristan Caetano, Samuel Rouillard, Elijah Karpf
// Descend Project
// CIS 464 Project 1

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script that deals with spell interactibility
public class Spell : MonoBehaviour
{

// Explosion effect after the spell hits something
public GameObject impactEffect;

// Speed of the spell
public float speed = 20f;

// How much mana the spell uses
public int manaUsage;

// How much damage the spell uses
public int damage;

// Determines how long the spell lasts for
public bool collateral;

// Used to make sure if the spell hits a wall it doesnt go through it
float initSpeed;

// Spell's rigidbody
public Rigidbody2D rb;

    // Making sure the spell flies in the right direction, and then destroys it if it hasnt hit anything in 10 seconds
    void Start(){
        rb.velocity = transform.right * speed;
        initSpeed = rb.velocity.magnitude;
        Destroy(gameObject, 5f);
    }

    void Update(){
        if(rb.velocity.magnitude < initSpeed){
            // When the player is hit, the explosion effect plays on the impact location
            GameObject impact = Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(impact, 5f);

            if(!collateral){
                // Destroys the spell
                Destroy(gameObject);
            }
        }
    }

    // If the spell hits something
    void OnTriggerEnter2D(Collider2D hitInfo){


        // Getting the enemy info
        EnemyAttributes enemy = hitInfo.GetComponent<EnemyAttributes>();

        // Ignoring the players hitbox
        if(enemy.tag != "Player"){

            // If an enemy is found, they take damage
            if(enemy != null){
                enemy.TakeDamage(damage);
                FindObjectOfType<AudioManager>().Play("Spell Hit");
            }

            // When the player is hit, the explosion effect plays on the impact location
            GameObject impact = Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(impact, 5f);

            if(!collateral){
                // Destroys the spell
                Destroy(gameObject, .1f);
            }
            
        } else{
            return;
        }
    }
}
