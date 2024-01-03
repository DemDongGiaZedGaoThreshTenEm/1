using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GunProjectile : MonoBehaviour
{
    //bulet
    public GameObject bullet;
    public Transform cam;
    //bullet force
    public float shootForce, upwardForce;

    //Gun stats
    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots, flashtime;
    public int magazineSize, bulletsPerTap;
    public int maxAmmo;
    public bool allowButtonHold;
    public float speed = 5f;
    private float Loud;

    int bulletsleft, bulletsShot;

    //bools
    public bool shooting, readyToShoot, reloading;

    //Reference
    public Transform attackPoint;
    public Transform flashPoint;
    public LayerMask Enemy;
    private EnemiesMovement EM;
    public Guns gun;

    //Graphic
    public GameObject muzzleflash;
    public TextMeshProUGUI ammunitionDisplay;
    //bug fixing
    public bool allowInvoke = true;

    //Recoil
    Vector3 initialGunPos;
    Vector3 initialGunRot;

    [Header("Recoil setting")]
    public float rotationSpeed = 4f;
    public float returnSpeed = 25f;
    public float backSpeed = 25f;


    [Header("Hip fire")]
    public Vector3 RecoilRotation = new Vector3(3f, 3f, 3f);
    public float kickBack = 5f;
    public float snappiness = 5f;


    [Header("ADS")]
    public Vector3 RecoilRotationADS = new Vector3(1.4f, 1.4f, 1.4f);
    public float kickBackADS = 0f;
    [Header("Status")]
    public bool aiming;

    private Vector3 currentRotation;
    private Vector3 currentPos;
    private Vector3 targetPos;

    private Vector3 Rot;

    public Rotation SetY;
    // Start is called before the first frame update\
    void Start()
    {
        
    }
    private void Awake()
    {

        //make sure mag is full
        bulletsleft = magazineSize;
        readyToShoot = true;

    }
    //Fixed Updt (uses 4 recoil)
    void FixedUpdate() 
    {
        
    }

    // Update is called once per frame
    
    private void Update()
    {
        //Update Recoil
        cam = Camera.main.transform;

        initialGunPos = transform.localPosition;
        currentRotation = Vector3.Lerp(currentRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        Rot = Vector3.Slerp(Rot, currentRotation, rotationSpeed * Time.fixedDeltaTime);
        transform.localRotation = Quaternion.Euler(Rot);
        cam.localRotation = Quaternion.Euler(-Rot);
        

        back();
        /**/
        if (Input.GetMouseButton(1))
        {
            aiming = true;
        }
        else { aiming = false; }
        MyInput();
        //Set ammunition display if it exists
        if (ammunitionDisplay != null)
            ammunitionDisplay.SetText(bulletsleft / bulletsPerTap + "/" + magazineSize / bulletsPerTap);
        
    }
    private void MyInput()
    {
        if (allowButtonHold) shooting = Input.GetMouseButton(0);

        else shooting = Input.GetKeyDown(KeyCode.Mouse0);
        //Reloading 
        if (Input.GetKeyDown(KeyCode.R) && bulletsleft < magazineSize && !reloading) Reload();

        if (readyToShoot && shooting && !reloading && bulletsleft <= 0 && maxAmmo >0) Reload();
            //Shooting
            if (readyToShoot && shooting && !reloading && bulletsleft >0 && maxAmmo >0)
        {
            bulletsShot = 0;
            Shoot();
            fire();
            back();
        }    
    }
    public void Shoot()
    {
        readyToShoot = false;
        //Add forces 2 bullet
        GameObject cB = Instantiate(bullet, attackPoint.position, bullet.transform.rotation);
        Rigidbody rig = cB.GetComponent<Rigidbody>();
        rig.AddForce(attackPoint.forward * speed, ForceMode.Impulse);
        cB.GetComponent<Rigidbody>().AddForce(attackPoint.forward * shootForce, ForceMode.Impulse);
        
        //Instantiate muzzle flash if has 1
        if (muzzleflash != null)
            Instantiate(muzzleflash, flashPoint.position, Quaternion.identity);

        bulletsleft--;
        bulletsShot++;
        maxAmmo--;

        //Instantiate loud after 1 shot
        Loud = gun.MuzzleBlastLoud;
        Collider[] enemyColliders = GetEnemyCollidersInRadius(attackPoint.transform.position, Loud);
        foreach (Collider enemyCollider in enemyColliders)
            {
                EM = enemyCollider.GetComponent<EnemiesMovement>();
                EM.Chasing();
            }


        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }
        if (bulletsShot < bulletsPerTap && bulletsleft > 0)
        {
            Invoke("Shoot", timeBetweenShots);
        }

    }

    Collider[] GetEnemyCollidersInRadius(Vector3 position, float rad)
    {
        
        // Use OverlapSphere to find colliders within the specified radius
        Collider[] allColliders = Physics.OverlapSphere(position, rad);

        // Filter colliders to get only those with the "Enemy" tag
        List<Collider> enemyColliders = new List<Collider>();
        foreach (Collider collider in allColliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                enemyColliders.Add(collider);
            }
        }

        return enemyColliders.ToArray();
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }
    private void Reload()
    {
        Debug.Log("Reloading");
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }
    private void ReloadFinished()
    {
        bulletsleft = magazineSize;
        reloading = false;
    }
    public void fire()
    {

        currentPos = Vector3.Lerp(currentPos, targetPos, Time.deltaTime * snappiness);
        transform.localPosition = currentPos;
        

        if (aiming)
        {
            currentRotation += new Vector3(RecoilRotationADS.x, Random.Range(-RecoilRotationADS.y, RecoilRotationADS.y), Random.Range(-RecoilRotationADS.z, RecoilRotationADS.z));
            currentPos += new Vector3(0, 0, kickBackADS);
        }
        else
        {
            currentRotation += new Vector3(RecoilRotation.x, Random.Range(-RecoilRotation.y, RecoilRotation.y), Random.Range(-RecoilRotation.z, RecoilRotation.z));
            currentPos += new Vector3(0, 0, kickBack);

        }
    }
   
    public void back()
    {
        targetPos = Vector3.Lerp(targetPos, initialGunPos, Time.deltaTime * backSpeed);
        currentPos = Vector3.Lerp(currentPos, targetPos, Time.deltaTime * snappiness);
        transform.localPosition = currentPos;
        cam.localPosition = currentPos;
    }
    public void OnTriggerEnter(Collider bullet)
    {
        Destroy(this.bullet);
    }
}
