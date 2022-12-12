using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PistolScript : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] float range;
    [SerializeField] float fireRate;

    [SerializeField] int ammoCapacity;
    [SerializeField] int currentAmmoInClip;
    public static int ammoCarriedByPlayer = 0;
    [SerializeField] float reloadTime;
    bool isReloading;

    [SerializeField] GameObject gunModel;
    [SerializeField] Camera camera;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] Text magazineAmmoText;
    [SerializeField] Text spareAmmoText;

    [SerializeField] AudioSource gunAudioSource;
    [SerializeField] AudioClip gunshot;
    [SerializeField] AudioClip reloadSound;

    public PlayerMovement bulletholeInstantiater;

    void Start()
    {
        gunModel.SetActive(true);
        isReloading = false;
    }

    private void OnEnable()
    {
        isReloading = false;
    }

    void Update()
    {
        magazineAmmoText.text = currentAmmoInClip.ToString();
        spareAmmoText.text = ammoCarriedByPlayer.ToString();

        if (isReloading)
        {
            return;
        }

        if (Input.GetButtonDown("Reload") && currentAmmoInClip != ammoCapacity && ammoCarriedByPlayer > 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetButtonDown("Fire1") && currentAmmoInClip > 0)
        {            
            Shoot();
        }
    }

    void Shoot()
    {
        muzzleFlash.Play();

        currentAmmoInClip--;

        gunAudioSource.PlayOneShot(gunshot);

        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out RaycastHit hit, range))
        {
            Target target = hit.transform.GetComponent<Target>();

            if (target != null)
            {
                target.TakeDamage(damage);
            }

            bulletholeInstantiater.Shoot(hit);
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;

        gunModel.SetActive(false);

        gunAudioSource.PlayOneShot(reloadSound);

        yield return new WaitForSeconds(reloadTime);

        if (ammoCarriedByPlayer + currentAmmoInClip <= ammoCapacity)
        {
            if (ammoCarriedByPlayer < 0)
            {
                ammoCarriedByPlayer = 0;
            }
            currentAmmoInClip = currentAmmoInClip + ammoCarriedByPlayer;
            ammoCarriedByPlayer = 0;
        }
        else
        {
            ammoCarriedByPlayer -= ammoCapacity - currentAmmoInClip;
            if (ammoCarriedByPlayer < 0)
            {
                ammoCarriedByPlayer = 0;
            }
            currentAmmoInClip = ammoCapacity;
        }

        gunModel.SetActive(true);

        isReloading = false;
    }
}
