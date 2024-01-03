using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAnim : MonoBehaviour
{
    public Animator anim;
    public GunProjectile GP;
    private bool isBolting = false;
    public Guns gun;
    public GameObject Gun;
    private void Start()
    {
        // Get the Animator component attached to the same GameObject
        anim = GetComponent<Animator>();
        GP = this.gameObject.GetComponent<GunProjectile>();
    }

    private void Update()
    {
        // Check for user input to initiate the bolt action
        if (GP.readyToShoot = false && gun.Name == "HSR-LR19")
        {
            Gun.GetComponent<Bobbing>().enabled = false;
            // Trigger the BoltAction animation
            Gun.GetComponent<Animator>().Play("Handling");

            // Set a flag to prevent multiple bolt actions in quick succession
            isBolting = true;

        }
    }

    // Called by animation event when the bolt action animation finishes
    public void OnBoltActionComplete()
    {
        Gun.GetComponent<Bobbing>().enabled = true;

        // Reset the flag, allowing another bolt action
        isBolting = false;
    }
}
