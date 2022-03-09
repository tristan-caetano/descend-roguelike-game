using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse_Pointer : MonoBehaviour
{

    private Vector3 target;
    public GameObject crosshairs;
    public GameObject player;
    public GameObject magicPrefab;

    public float spellSpeed = 1f;

    // Start is called before the first frame update
    void Start(){
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {

        target = transform.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
        crosshairs.transform.position = new Vector2(target.x, target.y);

        // Weird rotation thing
        Vector3 difference = target - player.transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        // player.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);

        if(Input.GetMouseButtonDown(0)){
            float distance = difference.magnitude;
            Vector2 direction = difference / distance;
            direction.Normalize();
            fireBullet(direction, rotationZ);
        }
        
    }

    void fireBullet(Vector2 direction, float rotationZ){
        GameObject b = Instantiate(magicPrefab) as GameObject;
        b.transform.position = player.transform.position;
        b.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
        b.GetComponent<Rigidbody2D>().velocity = direction * spellSpeed;
    }
}