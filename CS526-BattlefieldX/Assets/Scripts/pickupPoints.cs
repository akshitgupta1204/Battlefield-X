using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickupPoints : MonoBehaviour {

    public int scoreToGive;
    private AudioManager audioManager;

    // Use this for initialization
    void Start () {
        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogError("Freak out");
        }
    }
	
	// Update is called once per frame
	void Update () {
        
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.name == "Player")
        {
            audioManager.PlaySound("Money");

            GameMaster.Money += scoreToGive;
            gameObject.SetActive(false);
        }
    }
}
