using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour {

    private bool doublePoints;
    private bool safeMode;
    private bool powerupActive;
    private float powerupLengthCounter;
    private ScoreManager theScoreManager;
    private GameMaster theGameMaster;
    private PlatformGenerator thePlatformGenerator;
    private float normalPointsPerSecond;
    private float fireRate;
    private PlatformDestroyer[] fireList;
    private PlatformDestroyer[] fireArray;


    [SerializeField]
    private GameObject scoreMultiplier;

    [SerializeField]
    private GameObject eliminateFire;


    // Use this for initialization
    void Start () {
        theScoreManager = FindObjectOfType<ScoreManager>();
        thePlatformGenerator = FindObjectOfType<PlatformGenerator>();
        theGameMaster = FindObjectOfType<GameMaster>();
        fireArray = FindObjectsOfType<PlatformDestroyer>();
    }
	
	// Update is called once per frame
	void Update () {
		if(powerupActive)
        {
            powerupLengthCounter -= Time.deltaTime;

            if(theGameMaster.powerupReset)
            {
                powerupLengthCounter = 0;
                theGameMaster.powerupReset = false;
            }


            if(doublePoints)
            {
                theScoreManager.pointsPerSecond = normalPointsPerSecond * 2.75f;
                theScoreManager.shouldDouble = true;
                scoreMultiplier.SetActive(true);
            }

            if(safeMode)
            {
                thePlatformGenerator.randomFireThreshold = 0f;
                eliminateFire.SetActive(true);
            }

            if(powerupLengthCounter <= 0)
            {
                theScoreManager.pointsPerSecond = normalPointsPerSecond;
                theScoreManager.shouldDouble = false;
                thePlatformGenerator.randomFireThreshold = fireRate;
                powerupActive = false;
                scoreMultiplier.SetActive(false);
                eliminateFire.SetActive(false);

                for (int i = 0; i < fireArray.Length; i++)
                {
                    if (fireArray[i].gameObject.name.Contains("Fire"))
                    {

                        fireArray[i].gameObject.SetActive(true);
                    }
                }
            }
        }
	}

    public void ActivatePowerup(bool points, bool safe, float time)
    {
        doublePoints = points;
        safeMode = safe;
        powerupLengthCounter = time;

        normalPointsPerSecond = theScoreManager.pointsPerSecond;
        fireRate = thePlatformGenerator.randomFireThreshold;

        if(safeMode)
        {
            fireList = FindObjectsOfType<PlatformDestroyer>();
            for(int i=0; i<fireList.Length; i++)
            {
                if (fireList[i].gameObject.name.Contains("Fire"))
                {
                    fireList[i].gameObject.SetActive(false);
                }
            }

        }

        powerupActive = true;
    }
}
