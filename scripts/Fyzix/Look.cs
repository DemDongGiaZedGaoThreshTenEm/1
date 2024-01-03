using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Look : MonoBehaviour
{
    public float speed = 5f;
    public Transform target;
    public float smooth = 100;


    // Update is called once per frame
    void Update()
    {
        // Di chuyển Object theo phím mũi tên
        transform.LookAt(target);
    }
}
