using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipPrevention : MonoBehaviour
{
    public GameObject ClipProjector;
    //public Transform ClipProjector2;
    public float checkDistance;
    public float checkSideDistance;
    public float reCheckDistance;
    public Vector3 newDr, newDr2;
    public Transform newGunPos;
    float lerpPos;
    RaycastHit hit;
    public LayerMask ground;
    public LayerMask NoClip;
    Vector3 initialGunPos;

    //[SerializeField] Water w;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        reCheckDistance = -checkDistance;
        Mathf.Clamp01(lerpPos);
        bool noClip = Physics.Raycast(ClipProjector.transform.position, -ClipProjector.transform.forward, out hit, checkDistance, NoClip);

        //get a % from 0 to max dstnc
        if (Physics.Raycast(ClipProjector.transform.position, -ClipProjector.transform.forward, out hit, checkDistance) && !(Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W)) && !noClip) 
        {
            lerpPos = 1 - (Mathf.Abs(hit.distance) / checkDistance);
            transform.localRotation = Quaternion.Slerp(Quaternion.Euler(Vector3.zero), Quaternion.Euler(newDr2), lerpPos);
        }

        //not hitting anthls
        else if (Physics.Raycast(ClipProjector.transform.position, ClipProjector.transform.forward, out hit, checkDistance) && Input.GetKey(KeyCode.LeftShift) & Input.GetKey(KeyCode.W))
        {
            lerpPos = 1 - (Mathf.Abs(hit.distance) / checkDistance);
            transform.localRotation = Quaternion.Slerp(Quaternion.Euler(Vector3.zero), Quaternion.Euler(newDr), lerpPos);
        }
        else { lerpPos = 0; }
    }   
}
