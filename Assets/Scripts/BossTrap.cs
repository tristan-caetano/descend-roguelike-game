using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrap : MonoBehaviour
{
    
    public Collider2D boxCollider2D;
    public EnemyAttributes enemy;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (enemy.health == 0){
            boxCollider2D.isTrigger = true;
        }
    }
}
