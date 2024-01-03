using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : MonoBehaviour
{
    public GameObject Gun;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Gun != null)
        {
            if (Input.GetMouseButton(1))
            {
                Gun.GetComponent<Animator>().Play("Aim");
            }
        }    
        
    }
}
