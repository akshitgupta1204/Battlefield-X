using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : MonoBehaviour {

    public GameObject EnemyBulletGo;
	// Use this for initialization
	void Start () {
        
            InvokeRepeating("FireEnemyBullet", 1f, 2f);
        
	}
	
	// Update is called once per frame
	void Update () {
        //Invoke("FireEnemyBullet", 1f);
    }

    void FireEnemyBullet()
    {
        GameObject player = GameObject.Find("Player");
        if(player != null)
        {
            GameObject bullet = (GameObject)Instantiate(EnemyBulletGo);
            bullet.transform.position = transform.position;

            Vector2 direction = player.transform.position - bullet.transform.position;
            bullet.GetComponent<EnemyBullet>().SetDirection(direction);
        }
    }
}
