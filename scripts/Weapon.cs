using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapon : MonoBehaviour
{
    #region Variables
    public Guns[] LoadOut;
    private int Index;
    public Transform WeaponParent;
    public GameObject CurrentWeapon;
    public GameObject Gun;
    public Transform GroundAndWallDetector;
    public LayerMask ground;

    #endregion
    #region MonoBehaviour
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && CurrentWeapon == null)
            Equip(0);
        if(CurrentWeapon!=null)
        {
            aim(Input.GetMouseButton(1) && !Input.GetKey(KeyCode.LeftShift));
            Sprint(Input.GetKey(KeyCode.LeftShift));
        }          
    }
    #endregion
    #region Method
    void Equip(int p_ind)
    {
        if (CurrentWeapon != null)
            Destroy(CurrentWeapon);
        Index = p_ind;
        GameObject newWeapon = Instantiate(LoadOut[p_ind].prefabs, WeaponParent.position, WeaponParent.rotation, WeaponParent);
        newWeapon.transform.localPosition = Vector3.zero;
        newWeapon.transform.localEulerAngles = Vector3.zero;

        CurrentWeapon = newWeapon;
    }
    public void aim(bool p_aim)
    {
        Transform anchor = CurrentWeapon.transform.GetChild(0);
        Transform Status_hip = CurrentWeapon.transform.GetChild(1).GetChild(0);
        Transform Status_ads = CurrentWeapon.transform.GetChild(1).GetChild(1);

        if(p_aim)
        {
            anchor.position = Vector3.Lerp(anchor.position, Status_ads.position, Time.deltaTime * LoadOut[Index].AimSpeed); 
        }
        else
        {
            anchor.position = Vector3.Lerp(anchor.position, Status_hip.position, Time.deltaTime * LoadOut[Index].AimSpeed);
        }
    }
    void Sprint(bool p_sprint)
    {
        Transform anchor = CurrentWeapon.transform.GetChild(0);
        Transform Status_hip = CurrentWeapon.transform.GetChild(1).GetChild(0);
        Transform Status_hip_sprint = CurrentWeapon.transform.GetChild(1).GetChild(0).GetChild(1);
        if (p_sprint)
        {
            anchor.position = Vector3.Lerp(anchor.position, Status_hip_sprint.position, Time.deltaTime*0.2f);
        }
        else
        {
            anchor.position = Vector3.Lerp(anchor.position, Status_hip.position, Time.deltaTime * 0.2f);
        }
    }
    
    #endregion
}
