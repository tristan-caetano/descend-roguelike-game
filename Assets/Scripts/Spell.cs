using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{

public GameObject impactEffect;
public float speed = 20f;
public Rigidbody2D rb;

void Start(){
    rb.velocity = transform.right * speed;

    Destroy(gameObject, 10f);
}

    void OnTriggerEnter2D(Collider2D hitInfo){

        EnemyAttributes enemy = hitInfo.GetComponent<EnemyAttributes>();

        if(enemy.tag != "Player"){

            if(enemy != null){
                enemy.TakeDamage(10);
            }

            Debug.Log("Hit");

            Instantiate(impactEffect, transform.position, transform.rotation);

            Debug.Log(hitInfo.name);

            Destroy(gameObject, .1f);
        } else{
            return;
        }
    }
}
