using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(EnemyAI))]
public class Enemy : MonoBehaviour {

    public GameObject explosionEffect;


    [System.Serializable]
	public class EnemyStats
    {
        public int maxHealth=100;
        private int _curHealth;
        public int curHealth
        {
            get { return _curHealth; }
            set { _curHealth = Mathf.Clamp(value, 0, maxHealth); }
        }

        public int damage = 40;

        
        public void Init()
        {
            curHealth = maxHealth;
        }
    }

    public EnemyStats playerStats = new EnemyStats();

    public Transform deathParticles;
    public float shakeAmt = 0.1f;
    public float shakeLength = 0.1f;

    public string deathSoundName = "Explosion";
    public int moneyDrop = 10;

    [Header("Optional: ")]
    [SerializeField]
    private StatusIndicator statusIndicator;

    void Start()
    {
        playerStats.Init();

        if(statusIndicator != null)
        {
            statusIndicator.SetHealth(playerStats.curHealth, playerStats.maxHealth);
        }

        GameMaster.gm.onToggleUpgradeMenu += OnUpgradeMenuToggle;

        if(deathParticles == null)
        {
            Debug.LogError("No death particles referenced on Enemy");
        }
    }


    void OnUpgradeMenuToggle(bool active)
    {
        
        GetComponent<EnemyAI>().enabled = !active;
        
    }

    public void DamageEnemy(int damage)
    {
        playerStats.curHealth -= damage;
        if(playerStats.curHealth <=0)
        {
            GameMaster.KillEnemy(this);
        }

        if (statusIndicator != null)
        {
            statusIndicator.SetHealth(playerStats.curHealth, playerStats.maxHealth);
        }
    }

    void OnCollisionEnter2D(Collision2D _colInfo)
    {
        Player _player = _colInfo.collider.GetComponent<Player>();
        if(_player != null)
        {
            _player.DamagePlayer(playerStats.damage);
            DamageEnemy(999999);
        }

      

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Missile")
        {
            Instantiate(explosionEffect, col.transform.position, col.transform.rotation);
            DamageEnemy(999999);
            col.gameObject.SetActive(false);

        }
    }





    void OnDestroy()
    {
        GameMaster.gm.onToggleUpgradeMenu -= OnUpgradeMenuToggle;
    }
}
