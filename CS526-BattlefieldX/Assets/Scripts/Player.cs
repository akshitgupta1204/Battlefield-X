using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public class Player : MonoBehaviour {
    

    
    public int fallBoundary = -20;

    public string deathSoundName = "DeathVoice";
    public string damageSoundName = "Grunt";

    private AudioManager audioManager;

    [SerializeField]
    private StatusIndicator statusIndicator;

    private PlayerStats playerStats;

    Rigidbody m_Rigidbody;
    public GameObject explosionEffect;

    void Start()
    {

        playerStats = PlayerStats.instance;

        playerStats.curHealth = playerStats.maxHealth;

        if(statusIndicator == null)
        {
            Debug.LogError("No status indicator referenced on Player");
        }
        else
        {
            statusIndicator.SetHealth(playerStats.curHealth, playerStats.maxHealth);
        }

        GameMaster.gm.onToggleUpgradeMenu += OnUpgradeMenuToggle;

        audioManager = AudioManager.instance;
        if(audioManager == null)
        {
            Debug.Log("Error");
        }

        InvokeRepeating("RegenHealth", 1f/playerStats.healthRegenRate, 1f/playerStats.healthRegenRate);
    }


    void RegenHealth()
    {
        playerStats.curHealth += 1;
        statusIndicator.SetHealth(playerStats.curHealth, playerStats.maxHealth);
    }

    void Update()
    {
        if(transform.position.y <= fallBoundary)
        {
            DamagePlayer(9999999);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Killbox")
        {
            Instantiate(explosionEffect, collision.transform.position, collision.transform.rotation);
            DamagePlayer(30);
            collision.gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if((col.tag == "EnemyBulletTag"))
        {
            DamagePlayer(30);
        }
    }

    void OnUpgradeMenuToggle(bool active)
    {

        GetComponent<PlayerController>().enabled = !active;
        if(active == true)
        GetComponent<Rigidbody2D>().Sleep(); 




        Weapon _weapon = GetComponentInChildren<Weapon>();
        if(_weapon != null)
        {
            _weapon.enabled = !active;
        }
    }

    void OnDestroy()
    {
        GameMaster.gm.onToggleUpgradeMenu -= OnUpgradeMenuToggle;    
    }


    public void DamagePlayer(int damage)
    {
        playerStats.curHealth -= damage;
        if(playerStats.curHealth <=0)
        {
            audioManager.PlaySound(deathSoundName);
            GameMaster.KillPlayer(this);
        }
        else
        {
            audioManager.PlaySound(damageSoundName);
        }
        statusIndicator.SetHealth(playerStats.curHealth, playerStats.maxHealth);
    }

}
