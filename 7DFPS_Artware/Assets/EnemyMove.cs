using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    enum EnemyState { Wandering, Chasing, Fighting }

    EnemyState currentState = EnemyState.Wandering;
    //MAJOR WIP
    //WILL BE REWORKED
    //TO ADD CHASING BEHAVIOUR INSTEAD.

    private NavMeshAgent agent;
    Vector3 spawnPosition;
    RaycastHit hit;

    bool wanderDelay;

    [SerializeField] LayerMask chaseRayMask;
    //Vector3 chaseLocation;

    //// Start is called before the first frame update
    void Start()
    {
        spawnPosition = transform.position;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        ChooseTask();
    }

    void ChooseTask()
    {
        if (currentState == EnemyState.Chasing || currentState == EnemyState.Fighting)
        {
            if(!agent.hasPath)
            {
                Debug.Log("lost track of player... time to wander");
                Wander();
            }

            return;
        }

        if (!wanderDelay)
        {
            Wander();
        }
    }

    void Idle()
    {

    }

    void Wander()
    {
        currentState = EnemyState.Wandering;
        agent.destination = RandomNavSphere(transform.position, 10, 7);
        StartCoroutine(ActionDelay(Random.Range(1, 7)));
    }

    void Chase(Vector3 target)
    {
        agent.ResetPath();
        agent.destination = target;
    }

    void Fight()
    {

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
        wanderDelay = true;
        yield return new WaitForSeconds(idletime);
        wanderDelay = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            Vector3 dir = -(transform.position - other.transform.position).normalized;

            if (Physics.Raycast(transform.position, dir, out hit, 10f, chaseRayMask))
            {
                if (hit.transform.tag == "Player")
                {
                    Debug.DrawRay(transform.position, 10 * dir, Color.red, 2);
                    currentState = EnemyState.Chasing;
                    Chase(other.transform.position);
                }
            }
        }
    }

    void DistanceToPlayer()
    {
        //if(!agent.hasPath || )
    }
}
