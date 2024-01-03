using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DmgManagement : MonoBehaviour
{
    private int i;
    public float dmg;
    [Header("Area Dmg")]
    public float Radius;
    private List<float> distances;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        //dmg = wp.LoadOut[0].Damage;
        
    }
    private void OnCollisionEnter(Collision c)
    {
        //Direct hit
        if(c.gameObject.TryGetComponent<AttributesManager>(out AttributesManager target))
        {
            target.TakeDmg(dmg);
        }

        //Explode
        Collider[] colliders = Physics.OverlapSphere(transform.position, Radius);

        foreach (Collider col in colliders)
        {
            // Check if the nearby object has a Damageable component
            if (col.gameObject.TryGetComponent<AttributesManager>(out AttributesManager nearby))
            {
                float distance = Vector3.Distance(transform.position, col.transform.position);
                float ActualDmg = (1 - distance / Radius) * dmg;
                if (nearby != null)
                {
                    // Apply area damage to the nearby object
                    dmg = ActualDmg;
                    nearby.TakeDmg(dmg);
                }
            }
        }

        //Affected
        /*if (!c.gameObject.CompareTag("Enemy"))
        {
            
            Collider[] colliders = Physics.OverlapSphere(transform.position, Radius);

            foreach (Collider col in colliders)
            {
                // Check if the nearby object has a Damageable component
                if (col.CompareTag("Enemy"))
                {
                    float distance = Vector3.Distance(transform.position, col.transform.position);
                    float ActualDmg = Mathf.RoundToInt((1 - distance / Radius) * dmg);

                    AttributesManager nearbyDamageable = col.GetComponent<AttributesManager>();
                    if (nearbyDamageable != null && nearbyDamageable.HP != null)
                    {
                        // Apply area damage to the nearby object
                        nearbyDamageable.TakeDmg(ActualDmg);
                    }

                }
            }
        }*/    
        
        Destroy(gameObject);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Radius);
    }

}
