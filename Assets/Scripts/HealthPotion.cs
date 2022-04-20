// Tristan Caetano, Samuel Rouillard, Elijah Karpf
// Descend Project
// CIS 464 Project 1

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script for health potion attributes
public class HealthPotion : MonoBehaviour
{

    // Getting Rigidbody
    public Rigidbody2D rb;

    // Getting main player info
    GameObject mainPlayer;
    Transform target;
    PlayerAttributes playerAtt;

    // How much it heals the player
    public int heal = 30;

    // Boolean that makes sure that the potion is only used once
    bool used = false;

    // Update is called once per frame
    void Update()
    {

         // Keeps checking if the player value is still null
        if(mainPlayer == null){
            mainPlayer = GameObject.Find("Main_Player");
            target  = mainPlayer.transform;
            playerAtt = mainPlayer.GetComponent<PlayerAttributes>();

        // If the player is found, and the player is close enough, and if the potion 
        // hasn't already used, heal the player
        }else{

            // Pythagorean expression to determine distance to player
            float pythagDis = Mathf.Sqrt(Mathf.Pow(Mathf.Abs(target.position.x - rb.position.x) + Mathf.Abs(target.position.y - rb.position.y), 2f));

            if(pythagDis < 1 && playerAtt.health
             < 100 && !used && Input.GetKey(KeyCode.E)){
                playerAtt.Heal(heal);
                used = true;
                Destroy(gameObject, .1f);
            }
        }
    }

    // Healing the player
    void HealPlayer(){
        playerAtt.Heal(heal);
    }
}
