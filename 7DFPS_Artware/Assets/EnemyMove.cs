using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    enum EnemyState { Wandering, Chasing, Fighting }

    EnemyState currentState = EnemyState.Wandering;

    private NavMeshAgent agent;
    Vector3 spawnPosition;
    RaycastHit hit;

    public EnemyMP5Script enemyShoot;

    bool wanderDelay;

    [SerializeField] LayerMask chaseRayMask;

    [SerializeField] Animation[] enemyAnimations;

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
        //enemyAnimations[1].Play();
        agent.speed = 3.5f;
        agent.angularSpeed = 120;
        currentState = EnemyState.Wandering;
        agent.destination = RandomNavSphere(transform.position, 10, 7);
        StartCoroutine(ActionDelay(Random.Range(1, 7)));
    }

    void Chase(Vector3 target)
    {
        agent.speed = 3.5f;
        agent.angularSpeed = 120;
        currentState = EnemyState.Chasing;
        agent.ResetPath();
        agent.destination = target;
    }

    void Fight(Vector3 target)
    {
        agent.speed = 1f;
        agent.angularSpeed = 240;
        currentState = EnemyState.Chasing;
        agent.ResetPath();
        agent.destination = target;
        enemyShoot.EnemyShoot();
        Debug.Log("PEW PEW PEW IM SHOOTING PEW PEW PEW");
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
                    Debug.DrawRay(transform.position, 10 * dir, UnityEngine.Color.red, 2);
                    if(Vector3.Distance(transform.position, hit.transform.position) > 10)
                    {                     
                        Chase(other.transform.position);
                    }
                    else
                    {
                        Fight(other.transform.position);
                    }
                }
            }
        }
    }

    void DistanceToPlayer()
    {
        //if(!agent.hasPath || )
    }
}
