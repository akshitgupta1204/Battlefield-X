using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour {

    public GameObject thePlatform;
    public Transform generationPoint;
    public float distanceBetween;
    private float platformWidth;

    public float distanceBetweenMin;
    public float distanceBetweenMax;
    public ObjectPooler[] theObjectPool;
    //public GameObject[] thePlatforms;
    private int platformSelector;
    private float[] platformWidths;

    public float randomFireThreshold;
    public ObjectPooler firePool;

    private CoinGenerator coinGenerator;
    public float randomCoinThreshold;

    private float minHeight;
    public Transform maxHeightPoint;
    private float maxHeight;
    public float maxHeightChange;
    private float heightChange;

    public float powerupHeight;
    public ObjectPooler powerupPool;
    public float powerupThreshold;

    public float randomMissileThreshold;
    public ObjectPooler missilePool;



	// Use this for initialization
	void Start () {
        //platformWidth = thePlatform.GetComponent<BoxCollider2D>().size.x;	
        platformWidths = new float[theObjectPool.Length];
        for(int i=0; i<theObjectPool.Length; i++)
        {
            platformWidths[i] = theObjectPool[i].pooledObject.GetComponent<BoxCollider2D>().size.x;
            Debug.Log("Platform width is: " + platformWidths[i]);
        }

        coinGenerator = FindObjectOfType<CoinGenerator>();

        minHeight = transform.position.y;
        maxHeight = maxHeightPoint.position.y;
	}
	
	// Update is called once per frame
	void Update () {

        if(transform.position.x < generationPoint.position.x)
        {
            distanceBetween = Random.Range(distanceBetweenMin, distanceBetweenMax);
            platformSelector = Random.Range(0, theObjectPool.Length);

            heightChange = transform.position.y + Random.Range(maxHeightChange, -maxHeightChange);

            if(heightChange > maxHeight)
            {
                heightChange = maxHeight;
            }

            else if(heightChange < minHeight)
            {
                heightChange = minHeight;
            }


            if(Random.Range(0f, 100f) < powerupThreshold)
            {
                GameObject newPowerup = powerupPool.GetPooledObject();
                newPowerup.transform.position = transform.position + new Vector3(distanceBetween / 2f, Random.Range(12f,powerupHeight), 0f);
                newPowerup.SetActive(true);
            }

            //transform.position = new Vector3(transform.position.x + (platformWidths[platformSelector]/2) + distanceBetween, heightChange, transform.position.z);
            transform.position = new Vector3(transform.position.x + 1290 + distanceBetween, transform.position.y, transform.position.z);
            //Instantiate(theObjectPool[platformSelector], transform.position, transform.rotation);
            GameObject newPlatform = theObjectPool[platformSelector].GetPooledObject();
            newPlatform.transform.position = transform.position;
            newPlatform.transform.rotation = transform.rotation;
            newPlatform.SetActive(true);
            if(Random.Range(0f, 100f) < randomFireThreshold)
            {
                GameObject newFire = firePool.GetPooledObject();
                float spikeXPosition = Random.Range(-platformWidths[platformSelector] / 2f + 1f, platformWidths[platformSelector] / 2f - 1f); 
                Vector3 firePosition = new Vector3(spikeXPosition, Random.Range(8f, 12f), 0f);

                newFire.transform.position = transform.position + firePosition;
                newFire.transform.rotation = transform.rotation;
                newFire.SetActive(true);
            }

            if (Random.Range(0f, 100f) < randomCoinThreshold)
            {
                coinGenerator.SpawnCoins(new Vector3(transform.position.x, transform.position.y + Random.Range(11f, 17f), transform.position.z));
            }

            if(Random.Range(0f, 100f) < randomMissileThreshold)
            {
                GameObject newMissile = missilePool.GetPooledObject();
                float missileXPosition = Random.Range(-platformWidths[platformSelector] / 2f + 1f, platformWidths[platformSelector] / 2f - 1f);

                Vector3 missilePosition = new Vector3(missileXPosition, Random.Range(10.5f, 15f), 0f);
                newMissile.transform.position = transform.position + missilePosition;
                newMissile.transform.rotation = transform.rotation;
                newMissile.SetActive(true);
            }

            transform.position = new Vector3(transform.position.x + (platformWidths[platformSelector] / 3), transform.position.y, transform.position.z);


        }

    }
}
