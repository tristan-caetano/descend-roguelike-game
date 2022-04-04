using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellPickup : MonoBehaviour
{

    public Rigidbody2D rb;
    public GameObject spell;
    GameObject mainPlayer;
    Transform target;
    PlayerAttributes playerAtt;
    GameObject camera;
    Mouse_Pointer mousePoint;
    public string magicName;

    // Update is called once per frame
    void Update()
    {
        // Keeps checking if the player value is still null
        if(mainPlayer == null && camera == null){
            mainPlayer = GameObject.Find("Main_Player");
            camera = GameObject.Find("Main Camera");
            target = mainPlayer.transform;
            playerAtt = mainPlayer.GetComponent<PlayerAttributes>();
            mousePoint = camera.GetComponent<Mouse_Pointer>();

        // If the player is found, and the player is close enough, and if the potion 
        // hasn't already used, heal the player
        }else{

            // Pythagorean expression to determine distance to player
            float pythagDis = Mathf.Sqrt(Mathf.Pow(Mathf.Abs(target.position.x - rb.position.x) + Mathf.Abs(target.position.y - rb.position.y), 2f));

            if(pythagDis < 1 && mousePoint.magicName != magicName && Input.GetKey(KeyCode.E)){

                mousePoint.replaceSpell(gameObject);
            }
        }
    }
}
