using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Weapon : MonoBehaviour {

    public float fireRate = 0;
    public int Damage = 10;
    public LayerMask whatToHit;
    public Transform BulletTrailPrefab;
    public Transform HitPrefab;
    public Transform MuzzleFlashPrefab;
    float timeToSpawnEffect = 0;
    public float effectSpawnRate = 10;
    public float camShakeAmount = 0.05f;
    public float camShakeLength = 0.1f;
    CameraShake camShake;

    public string weaponShotSound = "DefaultShot";

    float timeToFire = 0;
    Transform firePoint;

    AudioManager audioManager;

    public int maxAmmo = 10;
    private int currentAmmo;
    public float reloadTime = 1f;
    private bool isReloading = false;
    float h;
    float v;

    // Use this for initialization
    void Awake () {
        firePoint = transform.Find("FirePoint");
        if(firePoint == null)
        {
            Debug.LogError("No Ammo");
        }




	}

    void Start()
    {
        camShake = GameMaster.gm.GetComponent<CameraShake>();
        if(camShake == null)
        {
            Debug.LogError("No CameraShake script found on GM object");
        }

        audioManager = AudioManager.instance;
        if(audioManager == null)
        {
            Debug.LogError("Freak");
        }

        if(currentAmmo == -1)
        {
            currentAmmo = maxAmmo;
        }
    }


    void OnEnable()
    {
        isReloading = false;
    }

    // Update is called once per frame
    void Update () {
        //Effect();
        h = CrossPlatformInputManager.GetAxis("Horizontal");
        v = CrossPlatformInputManager.GetAxis("Vertical");
        if (isReloading)
            return;
        if(currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }



        if (fireRate == 0)
        {
            /*if(Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }*/

            if(!(h==0 && v==0))
            {
                Shoot();
            }
        }

        else
        {
            /*if(Input.GetButton("Fire1") && Time.time > timeToFire)
            {
                timeToFire = Time.time + 1 / fireRate;
                Shoot();
            }*/

            if (!(h == 0 && v == 0) && Time.time > timeToFire)
            {
                timeToFire = Time.time + 1 / fireRate;
                Shoot();
            }
        }

	}

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");
        audioManager.PlaySound("Reloading");
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
    }

    void Shoot()
    {

        currentAmmo--;
        //Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);
        //RaycastHit2D hit = Physics2D.Raycast(firePointPosition, mousePosition - firePointPosition, 100, whatToHit);
        RaycastHit2D hit = Physics2D.Raycast(firePointPosition, new Vector2(h, v), 100, whatToHit);

        //Debug.DrawLine(firePointPosition, (mousePosition - firePointPosition)*100, Color.cyan);

        if(hit.collider!=null)
        {
            Debug.DrawLine(firePointPosition, hit.point, Color.red);
           
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if(enemy != null)
            {
                
                enemy.DamageEnemy(Damage);
                Debug.Log("We hit " + hit.collider.name + " and did " + Damage + "damage");
            }

            EnemyBullet em = hit.collider.GetComponent<EnemyBullet>();
            if (em != null)
            {
                
                Destroy(em.gameObject);
            }

            
        }

        if (Time.time >= timeToSpawnEffect)
        {

            Vector3 hitPos;
            Vector3 hitNormal;
            if (hit.collider == null)
            {

                hitPos = (new Vector2(h, v)) * 30;
                hitNormal = new Vector3(9999, 9999, 9999);
            }
            else
            {
                hitPos = hit.point;
                hitNormal = hit.normal;
            }
            Effect(hitPos, hitNormal);
            timeToSpawnEffect = Time.time + 1 / effectSpawnRate;
        }
    }

    void Effect(Vector3 hitPos, Vector3 hitNormal)
    {
        Transform trail = Instantiate(BulletTrailPrefab, firePoint.position, firePoint.rotation) as Transform;
        LineRenderer lr = trail.GetComponent<LineRenderer>();

        if (lr != null)
        {
            lr.SetPosition(0, firePoint.position);
            lr.SetPosition(1, hitPos);
        }

        Destroy(trail.gameObject, 0.04f);

        if(hitNormal != new Vector3(9999, 9999, 9999))
        {
            Transform hitParticle = Instantiate(HitPrefab, hitPos, Quaternion.FromToRotation(Vector3.right, hitNormal)) as Transform;
            Destroy(hitParticle.gameObject, 1f);
        }

        Transform clone = Instantiate(MuzzleFlashPrefab, firePoint.position, firePoint.rotation) as Transform;
        clone.parent = firePoint;
        float size = Random.Range(0.6f, 0.9f);
        clone.localScale = new Vector3(size, size, size);
        Destroy(clone.gameObject, 0.02f);


        camShake.Shake(camShakeAmount, camShakeLength);

        audioManager.PlaySound(weaponShotSound);
    }
}
