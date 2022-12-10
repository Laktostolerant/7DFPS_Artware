using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{

    //MAJOR WIP
    //WILL BE REWORKED
    //TO ADD CHASING BEHAVIOUR INSTEAD.

    private NavMeshAgent agent;
    Vector3 originPosition;
    RaycastHit hit;

    bool isOccupied;

    [SerializeField] LayerMask chaseRayMask;
    Vector3 chaseLocation;

    // Start is called before the first frame update
    void Start()
    {
        originPosition = transform.position;
        agent = GetComponent<NavMeshAgent>();
        //agent.autoBraking = false;
        Wander();
    }

    // Update is called once per frame
    void Update()
    {

        //Todo: make it choose between behaviours; idle, wandering, chasing & fighting.
        //Make it an enum?
        //Kill coroutines when starting new behaviours.

        if(isOccupied && agent.hasPath)
        {
            
            return;
        }
           

        if(!agent.hasPath)
        {
            var distance = Vector3.Distance(transform.position, originPosition);

            if(distance > 10)
            {
                agent.destination = originPosition;
            }
            else
            {
                Wander();
                StartCoroutine(ActionDelay(Random.Range(5, 10)));
            }
        }
    }

    void Wander()
    {
        agent.destination = RandomNavSphere(transform.position, 10, 7);
    }

    Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;

        randomDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

        return navHit.position;
    }

    IEnumerator ActionDelay(float idletime)
    {
        isOccupied = true;
        yield return new WaitForSeconds(idletime);
        isOccupied = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("PLAYER IN RANGE");
            if (Physics.Raycast(transform.position, other.transform.position, out hit, 10f, chaseRayMask))
            {
                if (hit.transform.tag == "Player")
                {
                    Debug.Log("I CAN SEE PLAYER, NOT BEHIND WALL");
                    Chase(other.transform.position);
                }
            }
        }
    }

    void Chase(Vector3 target)
    {
        Debug.Log("TARGET LOCATION IS: " + target);
        agent.ResetPath();
        isOccupied = true;
        agent.destination = target; 
    }

    void DistanceToPlayer()
    {
        //if(!agent.hasPath || )
    }
}
