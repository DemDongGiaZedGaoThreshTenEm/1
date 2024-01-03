using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField] GameObject waterfx;
    public bool isUnderWater = false;
    // Start is called before the first frame update
    [SerializeField] Playermovement Pmvmnt;
    [SerializeField] ClipPrevention CP;


    void OnTriggerEnter(Collider c)
    {
        
        isUnderWater = true;
        if(c.gameObject.CompareTag("MainCamera"))
        {
            waterfx.gameObject.SetActive(true);
            RenderSettings.fog = true;
        }
        if(c.gameObject.CompareTag("Player") && c.GetComponent<Playermovement>() != null)
        {
            Pmvmnt = c.GetComponent<Playermovement>();
            Pmvmnt.isSwimming = true;
        } 
        
        if(c.gameObject.CompareTag("ChestLvl"))
        {
            c.GetComponentInParent<Rigidbody>().useGravity = false;
            if(Pmvmnt != null)   
            Pmvmnt.ResetVelocity();
            //Pmvmnt.JumpForce = 0f;
        }
            
    }   
    void OnTriggerExit(Collider c)
    {
        isUnderWater = false;
        if (c.gameObject.CompareTag("MainCamera"))
        {
            waterfx.gameObject.SetActive(false);
            RenderSettings.fog = false;
        }

        if (c.gameObject.CompareTag("Player") && c.GetComponent<Playermovement>() != null)
        {
            Pmvmnt = c.GetComponent<Playermovement>();
            Pmvmnt.isSwimming = false;
        }

    }
}
