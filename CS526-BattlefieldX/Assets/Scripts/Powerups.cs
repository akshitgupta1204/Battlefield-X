using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerups : MonoBehaviour {

    public bool doublePoints;
    public bool safeMode;
    public float powerupLength;
    private PowerupManager thePowerupManager;
    public Sprite[] powerupSprites;
    private AudioManager audioManager;
    // Use this for initialization
    void Start () {
        thePowerupManager = FindObjectOfType<PowerupManager>();
        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogError("Freak out");
        }
    }


    void Awake()
    {
        int powerupSelector = Random.Range(0, 2);
        
        switch(powerupSelector)
        {
            case 0: doublePoints = true;
                break;
            case 1: safeMode = true;
                break;
        }

        GetComponent<SpriteRenderer>().sprite = powerupSprites[powerupSelector];
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.name == "Player")
        {
            thePowerupManager.ActivatePowerup(doublePoints, safeMode, powerupLength);
            audioManager.PlaySound("Money");
        }
        gameObject.SetActive(false);
    }
}
