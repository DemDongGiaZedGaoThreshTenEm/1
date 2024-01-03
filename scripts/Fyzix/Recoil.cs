using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil : MonoBehaviour
{
    [Header("Recoil setting")]
    public float rotationSpeed = 4f;
    public float returnSpeed = 25f;

    [Header("Hip fire")]
    public Vector3 RecoilRotation = new Vector3(3f, 3f, 3f);

    [Header("ADS")]
    public Vector3 RecoilRotationADS = new Vector3(1.4f, 1.4f, 1.4f);

    [Header("Status")]
    public bool aiming;

    private Vector3 currentRotation;
    private Vector3 Rot;



    // Start is called before the first frame update
    void FixedUpdate()
    {
        currentRotation = Vector3.Lerp(currentRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        Rot = Vector3.Lerp(Rot, currentRotation, rotationSpeed * Time.fixedDeltaTime);
        transform.localRotation = Quaternion.Euler(Rot);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            fire();
        }
        if (Input.GetMouseButton(1))
        {
            aiming = true;
        }
        else { aiming = false; } 
            
    }

    public void fire()
    {
        if(aiming)
        {
            currentRotation += new Vector3(RecoilRotationADS.x, Random.Range(-RecoilRotationADS.y, RecoilRotationADS.y), Random.Range(-RecoilRotationADS.z, RecoilRotationADS.z));
        }  
        else
        {
            currentRotation += new Vector3(RecoilRotation.x, Random.Range(-RecoilRotation.y, RecoilRotation.y), Random.Range(-RecoilRotation.z, RecoilRotation.z));
        }
    }
}
