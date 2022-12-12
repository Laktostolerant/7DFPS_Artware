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
    enum EnemyState { Wandering, Chasing, Fighting }
    enum WeaponType { Pistol, Submachine }

    EnemyState currentState = EnemyState.Wandering;
    [SerializeField] WeaponType currentWeapon;

    private NavMeshAgent agent;
    Vector3 spawnPosition;
    RaycastHit hit;

    public EnemyPistolScript enemyShoot;

    bool wanderDelay;

    [SerializeField] LayerMask chaseRayMask;

    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] AudioClip[] gunSounds;
    [SerializeField] AudioClip[] reloadSounds;
    [SerializeField] AudioSource gunAudioSource;

    bool shootDelay;
    bool isReloading;

    [Space(10)]
    [Header("ENEMY STATS")]
    [SerializeField] int ammoCount;
    [SerializeField] int damageCount;
    [SerializeField] int reloadTime;
    [SerializeField] int missChance;
    [SerializeField] float firingRate;
    [SerializeField] AudioClip gunShot;
    [SerializeField] AudioClip reloady;

    void Start()
    {
        spawnPosition = transform.position;
        agent = GetComponent<NavMeshAgent>();

        if (currentWeapon == WeaponType.Pistol)
        {
            ammoCount = 5;
            damageCount = 5;
            reloadTime = 2;
            firingRate = 0.8f;
            gunShot = gunSounds[0];
            reloady = reloadSounds[0];
            missChance = 3;
        }
        else
        {
            ammoCount = 30;
            damageCount = 2;
            reloadTime = 4;
            firingRate = 0.1f;
            gunShot = gunSounds[1];
            reloady = reloadSounds[1];
            missChance = 5;
        }
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
        agent.angularSpeed = 360;
        currentState = EnemyState.Chasing;
        agent.ResetPath();
        agent.destination = target;

        if(!shootDelay)
        {
            StartCoroutine(Shoot(target));
        }
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

    IEnumerator Shoot(Vector3 targetPos)
    {
       if(!isReloading)
        {
            Vector3 dir = -(transform.position - targetPos).normalized;
            Debug.Log("I SHOT SHOT LOL");

            if (Physics.Raycast(transform.position, dir, out RaycastHit hit, 10, chaseRayMask))
            {
                if (hit.transform.tag == "Player")
                {
                    DealDamage();
                }
            }

            shootDelay = true;
            yield return new WaitForSeconds(firingRate);
            shootDelay = false;

            if (ammoCount < 1)
                StartCoroutine(Reload());
        }
    }

    void DealDamage()
    {
        muzzleFlash.Play();
        Target target = hit.transform.GetComponent<Target>();
        int roulette = Random.Range(0, 11);
        bool hitShot = roulette > missChance;

        Debug.Log("roulette: " + roulette);
        Debug.Log("hitshot? " + hitShot);

        gunAudioSource.PlayOneShot(gunShot);
        ammoCount--;

        if (hitShot)
            target.TakeDamage(damageCount);
        
    }

    IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(1f);
        gunAudioSource.PlayOneShot(reloady);
        yield return new WaitForSeconds(reloadTime - 1f);
        ReloadGun();
        isReloading = false;
    }

    void ReloadGun()
    {
        if (currentWeapon == WeaponType.Pistol)
            ammoCount = 5;
        else
            ammoCount = 30;
    }
}
