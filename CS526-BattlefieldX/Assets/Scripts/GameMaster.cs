using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

    public static GameMaster gm;
    public bool powerupReset;

    [SerializeField]
    private int maxLives = 1;

    private static int _remainingLives;
    public static int RemainingLives
    {
        get { return _remainingLives; }
    }

    [SerializeField]
    private int startingMoney;
    public static int Money;
   
    void Awake()
    {
      if(gm == null)
        {

            gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        }
    }

    public Transform playerPrefab;
    public Transform spawnPoint;
    public int spawnDelay = 2;
    public Transform spawnPrefab;
    public string respawnCountdownSoundName = "RespawnCountdown";
    public string spawnSoundName = "Spawn";
    public string gameOverSoundName = "GameOver";
    

    public CameraShake cameraShake;

    [SerializeField]
    private GameObject gameOverUI;

    [SerializeField]
    private GameObject upgradeMenu;

    [SerializeField]
    private WaveSpawner waveSpawner;

    public delegate void UpgradeMenuCallback(bool active);
    public UpgradeMenuCallback onToggleUpgradeMenu;

    private AudioManager audioManager;

    private ScoreManager scoreManager;

    void Start()
    {
        if(cameraShake == null)
        {
            Debug.LogError("No camera shake referenced in GameMaster");
        }

        _remainingLives = maxLives;
        Money = startingMoney;
        audioManager = AudioManager.instance;
        if(audioManager == null)
        {
            Debug.LogError("Freak out");
        }

        scoreManager = FindObjectOfType<ScoreManager>();
    }

    void Update()
    {
       if(Input.GetKeyDown(KeyCode.U))
        {
            ToggleUpgradeMenu();
        }
        
    }

    public void ToggleUpgradeMenu()
    {
        upgradeMenu.SetActive(!upgradeMenu.activeSelf);
        waveSpawner.enabled = !upgradeMenu.activeSelf;
        scoreManager.scoreIncreasing = !upgradeMenu.activeSelf;
        onToggleUpgradeMenu.Invoke(upgradeMenu.activeSelf);
    }


    public void EndGame()
    {

        audioManager.PlaySound(gameOverSoundName);
        Debug.Log("Game Over");
        gameOverUI.SetActive(true);
        scoreManager.scoreIncreasing = false;
        powerupReset = true;
    }
     
    public IEnumerator _RespawnPlayer()
    {

        audioManager.PlaySound(respawnCountdownSoundName);
        yield return new WaitForSeconds(spawnDelay);
        audioManager.PlaySound(spawnSoundName);
        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation);
        
    }

    public static void KillPlayer(Player player)
    {
        Destroy(player.gameObject);
        _remainingLives -= 1;
        if (_remainingLives <= 0)
        {
            gm.EndGame();
        }
        else
        {
            gm.StartCoroutine(gm._RespawnPlayer());
        }
    }

    public static void KillEnemy(Enemy enemy)
    {
        gm._KillEnemy(enemy);
    }

    public void _KillEnemy(Enemy _enemy)
    {
        audioManager.PlaySound(_enemy.deathSoundName);
        Money += _enemy.moneyDrop;
        audioManager.PlaySound("Money");
        Instantiate(_enemy.deathParticles, _enemy.transform.position, Quaternion.identity);
        cameraShake.Shake(_enemy.shakeAmt, _enemy.shakeLength);
        Destroy(_enemy.gameObject);
    }
}
