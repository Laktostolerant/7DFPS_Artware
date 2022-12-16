using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static EnemyMove;

public class DoctorMove : MonoBehaviour
{
    public enum DoctorState { Wandering, Fleeing, Idling }
    DoctorState currentState = DoctorState.Wandering;
    
    Animator animator;
    private NavMeshAgent agent;

    bool wanderDelay;

    RaycastHit hitRay;

    [SerializeField] LayerMask seeRayMask;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    bool fleeDelay;

    void Update()
    {
        if (!agent.hasPath)
        {
            Wander();
        }
        float velocity = agent.velocity.magnitude / agent.speed;
        animator.SetFloat("velocity", velocity);
    }

    void Wander()
    {
        SetSpeed(1, 3.5f, 120);
        currentState = DoctorState.Wandering;
        animator.SetBool("running", false);
        agent.destination = RandomNavSphere(transform.position, 10, 7);
        StartCoroutine(ActionDelay(Random.Range(4, 8)));
    }

    void Flee(Vector3 enemyPos)
    {
        SetSpeed(1, 5, 360);
        currentState = DoctorState.Fleeing;
        animator.SetBool("running", true);
        agent.destination = FleeSphere(enemyPos, 15, 7);
    }

    Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance;

        randomDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

        return navHit.position;
    }

    Vector3 FleeSphere(Vector3 enemyLocation, float distance, int layermask)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance;

        randomDirection += enemyLocation;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

        if(Vector3.Distance(navHit.position, enemyLocation) > Vector3.Distance(navHit.position, transform.position))
        {
            return navHit.position;
        }
        else
        {
            FleeSphere(enemyLocation, distance, layermask);
        }
        return transform.position;
    }

    IEnumerator ActionDelay(float idletime)
    {
        currentState = DoctorState.Idling;
        wanderDelay = true;
        yield return new WaitForSeconds(idletime);
        wanderDelay = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && !fleeDelay)
        {
            Vector3 dir = -(transform.position - other.transform.position).normalized;

            if (Physics.Raycast(transform.position, dir, out hitRay, 20f, seeRayMask))
            {
                if (hitRay.transform.tag == "Player")
                {
                    Debug.Log("I MUST FLEE");
                    Debug.DrawRay(transform.position, 10 * dir, UnityEngine.Color.red, 2);
                    Flee(other.transform.position);
                    StartCoroutine(DelayBetweenFleeAttempts());
                }
            }
        }
    }

    IEnumerator DelayBetweenFleeAttempts()
    {
        fleeDelay = true;
        yield return new WaitForSeconds(0.5f);
        fleeDelay = false;
    }

    void SetSpeed(float animationSpeed, float movementSpeed, int angularSpeed)
    {
        animator.speed = animationSpeed;
        agent.speed = movementSpeed;
        agent.angularSpeed = angularSpeed;
    }
}
