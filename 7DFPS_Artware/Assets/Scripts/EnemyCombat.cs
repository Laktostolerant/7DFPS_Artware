using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    Animator animator;
    EnemyMove enemyMove;

    [SerializeField] LayerMask shootRayMask;

    enum WeaponType { Pistol, Submachine }
    [SerializeField] WeaponType currentWeapon;

    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] AudioClip[] gunSounds;
    [SerializeField] AudioClip[] reloadSounds;
    [SerializeField] AudioSource gunAudioSource;
    [SerializeField] GameObject[] weaponModels;

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
        animator = GetComponent<Animator>();
        enemyMove = GetComponent<EnemyMove>();

        if (currentWeapon == WeaponType.Pistol)
        {
            ammoCount = 5;
            damageCount = 5;
            reloadTime = 2;
            firingRate = 0.8f;
            gunShot = gunSounds[0];
            reloady = reloadSounds[0];
            missChance = 3;
            weaponModels[0].SetActive(true);
        }
        else
        {
            ammoCount = 30;
            damageCount = 2;
            reloadTime = 4;
            firingRate = 0.08f;
            gunShot = gunSounds[1];
            reloady = reloadSounds[1];
            missChance = 5;
            weaponModels[1].SetActive(true);
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.F))
        {
            //StartCoroutine(Death());
        }
    }

    public void GoToShoot(Vector3 pos)
    {
        if(!shootDelay)
            StartCoroutine(Shoot(pos));
    }

    IEnumerator Shoot(Vector3 targetPos)
    {
        if (!isReloading)
        {
            Vector3 dir = -(transform.position - targetPos).normalized;
            if (Physics.Raycast(transform.position, dir, out RaycastHit hit, 10, shootRayMask))
            {
                if (hit.transform.tag == "Player")
                {
                    Debug.Log("HIT PLAYER");
                    DealDamage(hit);
                }
            }

            shootDelay = true;
            yield return new WaitForSeconds(firingRate);
            shootDelay = false;

            if (ammoCount < 1)
                StartCoroutine(Reload());
        }
    }

    void DealDamage(RaycastHit player)
    {
        muzzleFlash.Play();
        Target target = player.transform.GetComponent<Target>();
        int roulette = Random.Range(0, 11);
        bool hitShot = roulette > missChance;

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
        ReloadStock();
        isReloading = false;
    }

    void ReloadStock()
    {
        if (currentWeapon == WeaponType.Pistol)
            ammoCount = 5;
        else
            ammoCount = 30;
    }

    IEnumerator Death()
    {
        animator.SetBool("dead", true);
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
}
