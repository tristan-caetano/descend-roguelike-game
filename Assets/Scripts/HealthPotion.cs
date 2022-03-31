using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : MonoBehaviour
{

    public Rigidbody2D rb;
    GameObject mainPlayer;
    Transform target;
    PlayerAttributes playerAtt;
    int heal = 30;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

         // Keeps checking if the player value is still null
        if(mainPlayer == null){
            mainPlayer = GameObject.Find("Main_Player");
            target  = mainPlayer.transform;
            playerAtt = mainPlayer.GetComponent<PlayerAttributes>();
        }else{
            float pythagDis = Mathf.Sqrt(Mathf.Pow(Mathf.Abs(target.position.x - rb.position.x) + Mathf.Abs(target.position.y - rb.position.y), 2f));

            if(pythagDis < 1 && playerAtt.getHealth() < 100){
                playerAtt.Heal(heal);
                Destroy(gameObject, .1f);
                
            }
        }

        
        
    }

    void HealPlayer(){
        playerAtt.Heal(heal);
    }
}
