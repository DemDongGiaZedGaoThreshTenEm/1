using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTravel : MonoBehaviour
{
    public Transform AtkPoint;
    public GameObject Bullet;
    public float speed = 5f;
    private GameObject CurrentWeapon;
    public Guns[] LoadOut;
    private int Index;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            ShootBullets();
        }    
    }
    private void ShootBullets()
    {
        GameObject cB = Instantiate(Bullet, AtkPoint.position, Bullet.transform.rotation);
        Rigidbody rig = cB.GetComponent<Rigidbody>();
        rig.AddForce(AtkPoint.forward * speed, ForceMode.Impulse);

     
    }
}
