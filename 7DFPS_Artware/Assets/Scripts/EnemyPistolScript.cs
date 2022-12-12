using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPistolScript : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] float range;
    [SerializeField] float timeToFire;

    [SerializeField] int ammoCapacity;
    [SerializeField] int currentAmmoInClip;
    public static int ammoCarried = 30;
    [SerializeField] float reloadTime;
    bool isReloading;

    [SerializeField] GameObject gunModel;
    [SerializeField] Camera enemyCamera;
    [SerializeField] ParticleSystem muzzleFlash;

    [SerializeField] AudioSource gunAudioSource;
    [SerializeField] AudioClip gunshot;
    [SerializeField] AudioClip reloadSound;

    public PlayerMovement bulletholeInstantiater;
    public EnemyMove enemyShoot;

    void Start()
    {
        gunModel.SetActive(true);
        isReloading = false;
    }

    public void EnemyShoot()
    {
        if (currentAmmoInClip > 0)
        {
            StartCoroutine(Shoot());
            return;
        }
        else
        {
            StartCoroutine(EnemyReload()); // dödar öron
        }

        if (currentAmmoInClip == 0)
        {
            currentAmmoInClip += ammoCapacity;
        }
    }

    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(timeToFire);

        muzzleFlash.Play();

        currentAmmoInClip--;

        gunAudioSource.PlayOneShot(gunshot);

        if (Physics.Raycast(enemyCamera.transform.position, enemyCamera.transform.forward, out RaycastHit hit, range) && currentAmmoInClip > 0)
        {
            Target target = hit.transform.GetComponent<Target>();

            if (target != null)
            {
                target.TakeDamage(damage);
            }

            bulletholeInstantiater.Shoot(hit);
        }

        Debug.Log("PANG");
    }

    IEnumerator EnemyReload()
    {
        isReloading = true;

        gunAudioSource.PlayOneShot(reloadSound);

        yield return new WaitForSeconds(reloadTime);

        if (currentAmmoInClip == 0)
        {
            currentAmmoInClip += ammoCapacity;
        }

        isReloading = false;
    }
}
