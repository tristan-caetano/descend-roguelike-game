// Tristan Caetano, Samuel Rouillard, Elijah Karpf
// Descend Project
// CIS 464 Project 1

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script that controls player cheats
public class Cheats : MonoBehaviour
{

    // Being able to change attributes from various game objects
    GameObject mainPlayer;
    GameObject camera;
    Collider2D playerCollider;
    PlayerAttributes playerAtt;
    PlayerMovement playerMove;
    Mouse_Pointer mousePointer;

    // Booleans for cheat toggles
    bool isFast;
    bool isNoclip;
    bool isCoolDown;
    bool isCheatReady;

    // Initializing booleans
    void Start(){
        isFast = false;
        isNoclip = false;
        isCoolDown = false;
        isCheatReady = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Getting main player info for cheats
        if(mainPlayer == null && camera == null){
            mainPlayer = GameObject.Find("Main_Player");
            camera = GameObject.Find("Main Camera");
            playerAtt = mainPlayer.GetComponent<PlayerAttributes>();
            playerMove = mainPlayer.GetComponent<PlayerMovement>();
            playerCollider = mainPlayer.GetComponent<Collider2D>();
            mousePointer = camera.GetComponent<Mouse_Pointer>();
        } 

        // Press home key to activate cheats
        if(Input.GetKeyDown(KeyCode.Home)){
            if(isCheatReady){
                Debug.Log("Cheats Disabled");
                isCheatReady = false;
            }else{
                Debug.Log("Cheats Enabled");
                isCheatReady = true;
            }
        }

        // F1, F2, F3, and F4 for adding healts, toggling noclip, speed, and cooldown respectively
        if(Input.GetKeyDown("f1") && isCheatReady){addHealth();}
        if(Input.GetKeyDown("f2") && isCheatReady){noClip();}
        if(Input.GetKeyDown("f3") && isCheatReady){fastSpeed();}
        if(Input.GetKeyDown("f4") && isCheatReady){noCoolDown();}
        if(Input.GetKeyDown("f5") && isCheatReady){addMana();}
        
    }

    // Adds 30 health to player
    void addHealth(){playerAtt.health += 30; Debug.Log("Player Health: " + playerAtt.health);}

    // Adds 30 mana to the player
    void addMana(){playerAtt.mana += 30; Debug.Log("Player Mana: " + playerAtt.mana);}

    // Removes and enables player collision
    void noClip(){

        if(isNoclip){
            playerCollider.isTrigger = false;
            isNoclip = false;
            Debug.Log("Player Clipping is On");
        }else{
            playerCollider.isTrigger = true;
            isNoclip = true;
            Debug.Log("Player Clipping is Off");
        }
    }

    // Removes and enables the player to move very fast
    void fastSpeed(){
        if(isFast){
            playerMove.moveSpeed = 5f;
            isFast = false;
            Debug.Log("Player Speed is Normal");
        }else{
            playerMove.moveSpeed = 20f;
            isFast = true;
            Debug.Log("Player Speed is Fast");
        }
    }

    // Removes and enables the players cooldown for melee and spells
    void noCoolDown(){
        if(isCoolDown){
            playerMove.cooldownDuration = 1.0f;
            mousePointer.cooldownDuration = 1.0f;
            isCoolDown = false;
            Debug.Log("Player has Cooldown");
        }else{
            playerMove.cooldownDuration = 0f;
            mousePointer.cooldownDuration = 0f;
            isCoolDown = true;
            Debug.Log("Player doesn't have Cooldown");
        }
    }
}
