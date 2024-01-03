using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprinting : MonoBehaviour
{
    public Camera normalCam;
    public bool isMoving = false;
    public float movementSpd = 125;
    private Rigidbody rb;
    float FOV;//flied of view
    float SprintFOVModifier = 1.5f;
    public Transform GroundDetector;
    public LayerMask ground;
    public GameObject Gun;
    // Start is called before the first frame update
    void Start()
    {
        FOV = normalCam.fieldOfView;
        //Camera.main.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        bool isGround = Physics.Raycast(GroundDetector.position, Vector3.down, 0.1f, ground);
        
        if (Input.GetKey(KeyCode.W))
        {
            isMoving = true;
        }

        if (Input.GetKeyUp("w") || (Input.GetKeyUp("w") && Input.GetKeyUp(KeyCode.LeftShift)))
        {
            isMoving=false;
            Gun.GetComponent<Animator>().Play("New State");          
        }

        if (Input.GetKey(KeyCode.LeftShift) & isMoving == true &  isGround && !Input.GetMouseButton(1) && !Input.GetKey(KeyCode.Space))
        {
            normalCam.fieldOfView = Mathf.Lerp(normalCam.fieldOfView, FOV * SprintFOVModifier, Time.deltaTime * 8f);
            transform.position += transform.forward * Time.deltaTime * movementSpd;
            Gun.GetComponent<Animator>().Play("Weapon - Running State");
            //ani.SetFloat("Speed", );
        }  
        else
        {
            normalCam.fieldOfView = Mathf.Lerp(normalCam.fieldOfView, FOV, Time.deltaTime * 8f);
        }
        
    }
}
