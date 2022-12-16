using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    public enum EnemyState { Idling, Wandering, Chasing, Fighting }

    public EnemyState currentState { get; private set; }  = EnemyState.Wandering;

    private NavMeshAgent agent;
    Vector3 spawnPosition;
    RaycastHit hitRay;
    public RaycastHit playerHit { get; private set; }

    Animator animator;

    EnemyCombat enemyCombat;

    bool wanderDelay;

    [SerializeField] LayerMask chaseRayMask;

    void Start()
    {
        enemyCombat = GetComponent<EnemyCombat>();

        spawnPosition = transform.position;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (animator.GetBool("dead"))
        {
            agent.ResetPath();
            return;
        }

        float velocity = agent.velocity.magnitude / agent.speed;
        animator.SetFloat("velocity", velocity);

        ChooseTask();
    }

    void ChooseTask()
    {
        if (currentState == EnemyState.Chasing || currentState == EnemyState.Fighting)
        {
            if(!agent.hasPath)
            {
                Wander();
            }
            return;
        }

        if (!wanderDelay)
        {
            Wander();
        }
    }


    void Wander()
    {
        SetSpeed(0.8f, 2f, 120);
        currentState = EnemyState.Wandering;
        agent.destination = RandomNavSphere(transform.position, 10, 7);
        StartCoroutine(ActionDelay(Random.Range(4, 8)));
    }

    void Chase(Vector3 target)
    {
        animator.SetBool("running", true);
        SetSpeed(1, 3.5f, 120);
        currentState = EnemyState.Chasing;
        agent.ResetPath();
        agent.destination = target;
    }

    void Fight(Vector3 target)
    {
        SetSpeed(0.5f, 1, 360);
        currentState = EnemyState.Fighting;
        agent.ResetPath();
        agent.destination = target;

        enemyCombat.GoToShoot(target);
    }

    Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance;

        randomDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

        return navHit.position;
    }

    IEnumerator ActionDelay(float idletime)
    {
        currentState = EnemyState.Idling;
        wanderDelay = true;
        yield return new WaitForSeconds(idletime);
        wanderDelay = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            Vector3 dir = -(transform.position - other.transform.position).normalized;

            if (Physics.Raycast(transform.position, dir, out hitRay, 10f, chaseRayMask))
            {
                if (hitRay.transform.tag == "Player")
                {
                    Debug.DrawRay(transform.position, 10 * dir, UnityEngine.Color.red, 2);
                    if(Vector3.Distance(transform.position, hitRay.transform.position) > 10)
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

    void SetSpeed(float animationSpeed, float movementSpeed, int angularSpeed)
    {
        animator.speed = animationSpeed;
        agent.speed = movementSpeed;
        agent.angularSpeed = angularSpeed;
    }
}
