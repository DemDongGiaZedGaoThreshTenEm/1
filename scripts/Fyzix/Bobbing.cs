using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bobbing : MonoBehaviour
{
    public Transform GroundDetector;
    public LayerMask ground;
    public GameObject Gun;
    public Rigidbody rb;
    [Header("Sway")]
    public bool sway = true;
    public float step = 0.01f;
    public float maxStepDistance = 0.06f;
    Vector3 swayPos;

    [Header("Sway Rotation")]
    public float rotationStep = 4f;
    public float maxRotationStep = 5f;
    Vector3 swayEulerRot;

    public float smooth = 10f;
    float smoothRot = 12f;

    [Header("Bobbing")]
    public bool bobOffset = true;
    public bool bobSway = true;
    public float speedCurve;
    float curveSin { get => Mathf.Sin(speedCurve); }
    float curveCos { get => Mathf.Cos(speedCurve); }

    public Vector3 travelLimit = Vector3.one * 0.025f;
    public Vector3 bobLimit = Vector3.one * 0.01f;
    Vector3 bobPosition;

    public float bobExaggeration;

    [Header("Bob Rotation")]
    public Vector3 multiplier;
    Vector3 bobEulerRotation;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        rb = GetComponent<Rigidbody>();
            GetInput();
            Sway();
            SwayRotation();
            BobOffset();
            BobRotation();
            CompositePositionRotation();
        
    }
    Vector2 walkInput;
    Vector2 lookInput;

    void GetInput()
    {
        walkInput.x = Input.GetAxis("Horizontal");
        walkInput.y = Input.GetAxis("Vertical");
        walkInput = walkInput.normalized;

        lookInput.x = Input.GetAxis("Mouse X");
        lookInput.y = Input.GetAxis("Mouse Y");
    }


    public void Sway()
    {
        if(sway == false) { swayPos = Vector3.zero; return; }
        Vector3 invertLook = lookInput * -step;
        invertLook.x = Mathf.Clamp(invertLook.x, -maxStepDistance, maxStepDistance);
        invertLook.y = Mathf.Clamp(invertLook.y, -maxStepDistance, maxStepDistance);

        swayPos = invertLook;
    }

    public void SwayRotation()
    {
        Vector2 invertLook = lookInput * -rotationStep;
        invertLook.x = Mathf.Clamp(invertLook.x, -maxRotationStep, maxRotationStep);
        invertLook.y = Mathf.Clamp(invertLook.y, -maxRotationStep, maxRotationStep);
        swayEulerRot = new Vector3(invertLook.y, invertLook.x, invertLook.x);
    }

    public void CompositePositionRotation()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, swayPos + bobPosition, Time.deltaTime * smooth);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(swayEulerRot) * Quaternion.Euler(bobEulerRotation), Time.deltaTime * smoothRot);
    }

    public void BobOffset()
    {
        if(bobOffset == false) { bobPosition = Vector3.zero; return; }
        speedCurve += Time.deltaTime * (Physics.Raycast(GroundDetector.position, Vector3.down, 0.1f, ground) ? rb.velocity.magnitude : 1f) + 0.01f;

        bobPosition.x = (curveCos * bobLimit.x * (Physics.Raycast(GroundDetector.position, Vector3.down, 0.1f, ground) ? 1 : 0)) - (walkInput.x * travelLimit.x);
        bobPosition.y = (curveSin * bobLimit.y) - (Input.GetAxis("Vertical") * travelLimit.y);
        bobPosition.z = -(walkInput.y * travelLimit.z);
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            speedCurve += 2f*Time.deltaTime * (Physics.Raycast(GroundDetector.position, Vector3.down, 0.1f, ground) ? (Input.GetAxis("Horizontal") + Input.GetAxis("Vertical")) * bobExaggeration : 1f) + 0.01f;
        }    
        if (Input.GetMouseButton(1))
        {
            bobPosition.x = (curveCos * bobLimit.x * 0f* (Physics.Raycast(GroundDetector.position, Vector3.down, 0.1f, ground) ? 1 : 0)) - (walkInput.x * travelLimit.x*0.02f);
            bobPosition.y = (curveSin * bobLimit.y * 0f) - (Input.GetAxis("Vertical") * travelLimit.y*0f);
            bobPosition.z = -(walkInput.y * travelLimit.z * 0.01f);
        }    
    }

    public void BobRotation()
    {
        if(bobSway == false) { bobEulerRotation = Vector3.zero; return; } 
            
        bobEulerRotation.x = (walkInput != Vector2.zero ? multiplier.x * (Mathf.Sin(2 * speedCurve)) : multiplier.x * (Mathf.Sin(2 * speedCurve) / 2));
        bobEulerRotation.y = (walkInput != Vector2.zero ? multiplier.y * curveCos : 0);
        bobEulerRotation.z = (walkInput != Vector2.zero ? multiplier.z * curveCos * walkInput.x : 0);
        if (Input.GetMouseButton(1))
        {
            bobEulerRotation.x = (walkInput != Vector2.zero ? multiplier.x * (Mathf.Sin(0 * speedCurve)) : multiplier.x * (Mathf.Sin(0 * speedCurve) / 2));
            bobEulerRotation.y = (walkInput != Vector2.zero ? multiplier.y * curveCos : 0);
            bobEulerRotation.z = (walkInput != Vector2.zero ? multiplier.z * curveCos * walkInput.x : 0);

        }
    }
}
