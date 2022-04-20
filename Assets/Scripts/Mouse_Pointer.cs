// Tristan Caetano, Samuel Rouillard, Elijah Karpf
// Descend Project
// CIS 464 Project 1

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class for having the mouse cursor on screen as a crosshair, and shooting spells
public class Mouse_Pointer : MonoBehaviour
{
    // Displaying a crosshair on the screen
    private Vector3 target;
    public GameObject crosshairs;

    // Getting the position of the player, animating the attack, and getting the health of the player
    public GameObject player;
    public Rigidbody2D playerBody;
    public Animator animator;
    public PlayerAttributes playerAtt;

    // What the spell looks like
    public GameObject currMagicDrop;
    SpellPickup spellPickup;
    public string magicName;
    bool canPickup = true;
    
    // Cooldown between spells
    public float cooldownDuration = 1f;
    public bool isAvailable = true;

    // Make sure player can't move when game is paused
    bool canMove = false;

    // Update is called once per frame
    void Update()
    {
        // Making the cursor invisible
        Cursor.visible = false;

        // Getting the location of the mouse on screen and placing the crosshair there
        target = transform.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
        crosshairs.transform.position = new Vector2(target.x, target.y);

        // Getting the direction of the mouseclick for firing the spell and playing the correct animation
        Vector3 difference = target - player.transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        if(canMove){

            // If the cooldown isnt active
            if(isAvailable){

                // Getting spell info
                spellPickup = currMagicDrop.GetComponent<SpellPickup>();
                magicName = spellPickup.magicName;

                // If the mouse is clicked and the player is alive
                if(Input.GetMouseButtonDown(0) && playerAtt.health > 0 && playerAtt.mana > 5 && spellPickup != null){

                    // Animate the casting
                    animator.SetFloat("LastH", difference.x);
                    animator.SetFloat("LastV", difference.y);
                    animator.SetTrigger("Cast");
                    FindObjectOfType<AudioManager>().Play("Shoot Spell");

                    // Get the direction and distance from the mouse click
                    float distance = difference.magnitude;
                    Vector2 direction = difference / distance;
                    direction.Normalize();
                    playerBody.velocity = Vector3.zero;

                    // Reduces mana
                    playerAtt.UseMana(spellPickup.spell.GetComponent<Spell>().manaUsage);

                    // Cast the spell
                    fireBullet(direction, rotationZ);

                    // Start cooldown
                    StartCoroutine(StartCooldown());
                }
            }
            else{return;}
    } else {
        StartCoroutine(StartGameCooldown());
    }
    }

// Method that creates and casts the spell
    void fireBullet(Vector2 direction, float rotationZ){Instantiate(spellPickup.spell, player.transform.position, Quaternion.Euler(0.0f, 0.0f, rotationZ));}

    public void replaceSpell(GameObject newSpell){  
        if(canPickup && currMagicDrop != null){
            currMagicDrop.SetActive(true);
            currMagicDrop.transform.position = player.transform.position;
            currMagicDrop = newSpell;
            currMagicDrop.SetActive(false);

            // Start cooldown
            StartCoroutine(StartCooldown());
        }else if(canPickup && currMagicDrop == null){
            currMagicDrop = newSpell;
            currMagicDrop.SetActive(false);
        }
    }

    // Cooldown timer
    public IEnumerator StartCooldown(){
        isAvailable = false;
        yield return new WaitForSeconds(cooldownDuration);
        isAvailable = true;
    }

     // Cooldown timer
    public IEnumerator PickupCooldown(){
        canPickup = false;
        yield return new WaitForSeconds(cooldownDuration);
        canPickup = true;
    }

    // Cooldown timer
    public IEnumerator StartGameCooldown(){
        canMove = false;
        yield return new WaitForSeconds(.1f);
        canMove = true;
    }
}
