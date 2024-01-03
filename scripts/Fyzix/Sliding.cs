using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sliding : MonoBehaviour
{
    [Header("Reffrnc")]
    public Playermovement pm;
    public Transform Orientation;
    public Transform PlayerObj;
    public Rigidbody rb;

    [Header("Sliding")]
    public float maxSlideTime;
    public float slideForce;
    private float slideTimer;
    public bool sliding;
    [Header("Slope Handling")]
    public float MaxSlopeAngle;
    private RaycastHit slopeHit;
    // Start is called before the first frame update
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<Playermovement>();
    }

    // Update is called once per frame
    void Update()
    {
        float h_move = Input.GetAxisRaw("Horizontal");
        float v_move = Input.GetAxisRaw("Vertical");
    }
    void fixedUpdate()
    {
         SlidingMove();    
    }
    private void StartSlide()
    {
        sliding = true;
    }
    private void SlidingMove()
    {
        float h_move = Input.GetAxisRaw("Horizontal");
        float v_move = Input.GetAxisRaw("Vertical");

        Vector3 InputDr = Orientation.forward * v_move + Orientation.right * h_move;

        if (!pm.OnSlope() || rb.velocity.y > 0.1f)
        {
            rb.AddForce(InputDr.normalized * slideForce, ForceMode.Force);
        }
        else
        {
            rb.AddForce(pm.GetSlopeMoveDr(InputDr) * slideForce, ForceMode.Force);
        }
    }
    private void StopSliding()
    {
        sliding = false;
    }
}
