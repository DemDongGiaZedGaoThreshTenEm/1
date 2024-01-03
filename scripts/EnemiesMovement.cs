using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemiesMovement : MonoBehaviour
{
    public Transform Player, Allies;
    public GameObject Target;
    UnityEngine.AI.NavMeshAgent navm;
    public LayerMask whatIsGround, whatisPlayer, whatisAlly;

    public float detectTime, reinforceDistance;
    
    //patroling
    public Vector3 walkpoint;
    bool walkPointset;
    public float walkPointRange;

    //Attacking
    public float TimeBetweenAtk;
    public float AtkRange, SightRange, MvmntSenseRange, SensingTime;
    bool alreadyAtkd;

    public bool PlayerInSenseRange, PlayerInAtkRange, PlayerInSightRange , PlayerInDetectRange, ReinforceLoudCanHeard;

    public Camera cam;
    public Collider PlayerColl;
    private Plane[] planes;

    void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        navm = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }
    // Start is called before the first frame update
    void Start()
    {
        PlayerColl = Player.GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {

        planes = GeometryUtility.CalculateFrustumPlanes(cam);
        if (GeometryUtility.TestPlanesAABB(planes, PlayerColl.bounds));
        {
            navm.SetDestination(Player.position);
            Debug.Log("Player sighted");
            Check4Player();               
        }
        if(!GeometryUtility.TestPlanesAABB(planes, PlayerColl.bounds))
        {
            Patroling();
        }

        PlayerInAtkRange = Physics.CheckSphere(transform.position, AtkRange, whatisPlayer);
        PlayerInSenseRange = Physics.CheckSphere(transform.position, MvmntSenseRange, whatisPlayer);
        PlayerInSightRange = Physics.CheckSphere(transform.position, SightRange, whatisPlayer);
        var targetRender = Player.GetComponent<Renderer>();
        
        if (PlayerInAtkRange && GeometryUtility.TestPlanesAABB(planes, PlayerColl.bounds)) Attacking();
    }
    void Check4Player()
    { 
        RaycastHit hit;
        Debug.DrawRay(cam.transform.position, transform.forward * 100, Color.green);
        if(Physics.Raycast(cam.transform.position, transform.forward, out hit, 100))
        {
            Chasing();
            Target = hit.collider.gameObject;
        }          
    }

    /*private void IsAlly(Transform a)
    {
        Allies == a.gameObject.tag =="Enemy";
        Allies != this.gameObject;
    }*/
    private void Patroling()
    {
        if (!walkPointset) SearchWalkPoint();
        if (walkPointset)
            navm.SetDestination(walkpoint);

        Vector3 distance2Walkpoint = transform.position - walkpoint;

        //walkpoint reached
        if(distance2Walkpoint.magnitude <1f)
            walkPointset = false;
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkpoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
 
        if (Physics.Raycast(walkpoint, -transform.up, 0.8f, whatIsGround))
            walkPointset = true;
    }

    public void Chasing()
    {
        navm.SetDestination(Player.position);
    }

    private void Attacking()
    {
        navm.SetDestination(Player.position);
        transform.LookAt(Player);
        if(!alreadyAtkd)
        {
            alreadyAtkd = true;
            Invoke(nameof(ResetAttack), TimeBetweenAtk);
        }    

    }

    /*private void Reinforcing()
    {
        ReinforceLoudCanHeard = Physics.CheckSphere(transform.position, reinforceDistance, whatisAlly);
        if (ReinforceLoudCanHeard && Allies.gameObject && !this.gameObject)
        {
            FollowingAlly();
        }
    }
    //Check 4 rein4cement from allies
    private bool Rein4cementFromAllies()
    {
        if(Physics.CheckSphere(Allies.transform.position, reinforceDistance, whatisAlly) && IsAlly(Allies))
        {
            return true;
            FollowingAlly();
        }
        else
        {
            return false;
        }    
    }
    private void FollowingAlly()
    {
        this.gameObject.transform.position = Vector3.Lerp(this.gameObject.transform.position, Allies.transform.position, Time.deltaTime * 0.2f);
        
    }*/

    private void ResetAttack()
    {
        alreadyAtkd = false;
    }

    /*public void TakeDmg(int dmg)
    {
        health -= dmg;
        if (health < 0)
            Invoke(nameof(DestroyEnemy), 5f);

    }*/

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AtkRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, SightRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, reinforceDistance );

    }
}
