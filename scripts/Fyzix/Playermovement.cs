using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playermovement : MonoBehaviour
    {
    #region Variables
    Vector3 moveVector = Vector3.zero;
    public Vector3 velocity;
    CharacterController characterController;

    [SerializeField] public float speed;

    public float JumpForce;
    public float jumpspd;
    bool ready2jump;
    public float gravity;
    public Rigidbody rb;
    public Transform GroundDetector;
    public LayerMask ground;
    public GameObject Gun;
    public Transform target;
    public float drag;

    [Header("Reffrnc")]
    public Transform Orientation;
    public Transform PlayerObj;

    [Header("Sliding")]
    public float maxSlideTime;
    public float slideForce;
    private float slideTimer;

    [Header("Slope Handling")]
    public float MaxSlopeAngle;
    private RaycastHit slopeHit;
    //Swimming
    [SerializeField] Transform chestLvl;
    [SerializeField] float swimSpeed;
    [SerializeField] Transform DivingPoint;
    [SerializeField] public bool isSwimming;

    #endregion
    #region MonoBehaviour
    void Start()
    {
        //Camera.main.enabled = false;
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
         
            
    }
    private void FixedUpdate()
    {
        if(isSwimming != true)
        {

            float h_move = Input.GetAxisRaw("Horizontal");
            float v_move = Input.GetAxisRaw("Vertical");

            bool isGround = Physics.Raycast(GroundDetector.position, Vector3.down, 0.1f, ground);
            bool jump = Input.GetKeyDown(KeyCode.Space) && isGround;
            bool isjumping = jump;
            

            /*float adjustedspeed = speed;
            Physics.gravity = new Vector3(0f, -9.8f, 0f);
            Vector3 dir = new Vector3(h_move, 0f, v_move);
            dir.Normalize();
            Vector3 TargetVelocity = transform.TransformDirection(dir) * adjustedspeed * Time.deltaTime ;
            TargetVelocity.y -= Physics.gravity.y;          
            rb.velocity = TargetVelocity;*/
              
            if (Input.GetKey(KeyCode.Space))
            {
                Gun.GetComponent<Animator>().Play("Weapon Jumping");
            }

            if (Input.GetKeyDown(KeyCode.W) && isGround && !Input.GetKey(KeyCode.LeftShift))
                Gun.GetComponent<Animator>().Play("Weapon - Walking State");


            if (isjumping )
            {
                ready2jump = false;
                
                rb.AddForce(Vector3.up * JumpForce * 5f , ForceMode.Impulse);
            }

            if (isGround)
            {
                rb.drag = drag;
            }
            else 
            { 
                rb.drag = 0;

            }

            if (rb.useGravity != true)
            {
                rb.useGravity = true;
            }    
            
            
            SpeedControl();
            Move();
        }
        else
        {
            if (Input.GetAxisRaw("Vertical") > 0)
                transform.position += target.forward * swimSpeed * Time.deltaTime;

            if (Input.GetAxisRaw("Vertical") < 0)
                transform.position -= target.forward * swimSpeed * Time.deltaTime;

            if (Input.GetAxisRaw("Horizontal") > 0)
                transform.position += target.right * swimSpeed * Time.deltaTime;

            if (Input.GetAxisRaw("Horizontal") < 0)
                transform.position -= target.right * swimSpeed * Time.deltaTime;
        }
    }

    
    #endregion
    public void Move()
    {
        moveVector = Input.GetAxisRaw("Horizontal") * Orientation.right + Input.GetAxisRaw("Vertical") * Orientation.forward;
        bool isGround = Physics.Raycast(GroundDetector.position, Vector3.down, 0.1f, ground);
        
        
            rb.AddForce(moveVector.normalized * speed, ForceMode.Force);
        
            rb.transform.position += moveVector.normalized * speed * Time.deltaTime;
        


        if (OnSlope())
        {
            
            rb.AddForce(GetSlopeMoveDr(moveVector)* speed * 1f, ForceMode.Force);
            if(rb.velocity.y >0)
            {
                rb.AddForce(Vector3.down *80f, ForceMode.Force);
            }
               
        }
    }
    public bool OnSlope()
    {
        if (Physics.Raycast(GroundDetector.position, Vector3.down, out slopeHit, 0.8f))
        {
            if (slopeHit.normal != Vector3.up)
            {
                // Apply slope force
                Vector3 slopeForceDirection = Vector3.Cross(Vector3.Cross(Vector3.up, slopeHit.normal), slopeHit.normal).normalized;
                rb.AddForce(slopeForceDirection * slideForce, ForceMode.Force);
            }
            float Angle = Vector3.Angle(Vector3.up, slopeHit.normal);

            float adjstSpd = Mathf.Clamp01(1f - Mathf.Abs(Vector3.Dot(rb.velocity.normalized, transform.up)));

            return Angle <= MaxSlopeAngle && Angle != 0;
        }
        return false;
    }
    public Vector3 GetSlopeMoveDr(Vector3 Dir)
    {
        return Vector3.ProjectOnPlane(moveVector, slopeHit.normal).normalized;
    }

    private void SpeedControl()
    {
        if(OnSlope())
        {
            if (rb.velocity.magnitude > speed)
            {
                rb.velocity = rb.velocity.normalized * speed;
            }
        }
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            flatVel.Normalize();
            if (flatVel.magnitude > speed)
            {
                Vector3 limVel = transform.TransformDirection(flatVel) * speed * Time.deltaTime;
                rb.velocity = new Vector3(limVel.x, rb.velocity.y, limVel.z);
            }

        }
    }
    public void ResetVelocity()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero; 
    }
}
