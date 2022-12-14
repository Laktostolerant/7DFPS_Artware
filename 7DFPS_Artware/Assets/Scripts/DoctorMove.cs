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

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!agent.hasPath)
        {
            Wander();
        }
    }

    void Wander()
    {
        animator.speed = 1f;
        agent.speed = 3.5f;
        agent.angularSpeed = 120;
        currentState = DoctorState.Wandering;
        agent.destination = RandomNavSphere(transform.position, 10, 7);
        StartCoroutine(ActionDelay(Random.Range(4, 8)));
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
        currentState = DoctorState.Idling;
        wanderDelay = true;
        yield return new WaitForSeconds(idletime);
        wanderDelay = false;
    }
}
