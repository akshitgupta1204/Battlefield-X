using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class HomingMissile : MonoBehaviour {

    
    public float speed = 0f;
    public float rotateSpeed = 0f;
    private Rigidbody2D rb;
    bool touch = false;
    public Transform target;
    //Enemy enemy;

    // Use this for initialization
    void Start () {
        
        rb = GetComponent<Rigidbody2D>();
        //target = GameObject.FindGameObjectWithTag("Enemy").transform;
        //Debug.Log("Check it: "+target);
        //enemy = FindObjectOfType<Enemy>();
	}


    void Update()
    {
        
        target = GameObject.FindGameObjectWithTag("Enemy").transform;
        
        
    }


    // Update is called once per frame
    void FixedUpdate () {
        if (touch)
        {
            Vector2 direction = (Vector2)target.position - rb.position;

            direction.Normalize();

            float rotateAmount = Vector3.Cross(direction, transform.up).z;

            rb.angularVelocity = -rotateAmount * rotateSpeed;
            rb.velocity = transform.up * speed;
        }
	}


    


    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            touch = true;
            
        }

        
    }

    


}
