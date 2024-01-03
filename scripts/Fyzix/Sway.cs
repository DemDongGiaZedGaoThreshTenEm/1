using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sway : MonoBehaviour
{
    #region Variables
    public float Intensity;
    public float Smooth;
    private Quaternion origi_rotation;
    #endregion

    #region Monobehaviour
    private void Start()
    {
        origi_rotation = transform.localRotation;
    }
    private void Update()
    {
        UpdateSway();
    }
    #endregion

    #region Methods
    private void UpdateSway()
    {
        float x_Mouse = Input.GetAxis("Mouse X");
        float y_Mouse = Input.GetAxis("Mouse Y");
        Quaternion x_adj = Quaternion.AngleAxis(-Intensity * x_Mouse, Vector3.up);
        Quaternion y_adj = Quaternion.AngleAxis(-Intensity * y_Mouse, Vector3.right);

        Quaternion target_rotation = origi_rotation * x_adj * y_adj;
        transform.localRotation = Quaternion.Lerp(transform.localRotation, target_rotation, Smooth*Time.deltaTime);
    }
    #endregion
}
