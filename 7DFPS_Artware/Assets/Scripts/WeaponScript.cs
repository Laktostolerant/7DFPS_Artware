using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] float range;
    [SerializeField] float fireRate;

    float nextTimeToFire;
    
    [SerializeField] int ammoCapacity;
    [SerializeField] int currentAmmoInClip;
    [SerializeField] int ammoCarriedByPlayer;
    [SerializeField] float reloadTime;
    bool isReloading;

    [SerializeField] GameObject gunModel;
    [SerializeField] Camera camera;
    [SerializeField] ParticleSystem muzzleFlash;
    // Start is called before the first frame update
    void Start()
    {
        gunModel.SetActive(true);
        isReloading = false;
    }

    private void OnEnable()
    {
        isReloading = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isReloading)
        {
            return;
        }

        if (currentAmmoInClip <= 0 && ammoCarriedByPlayer > 0)
        {           
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetButtonDown("Reload") && currentAmmoInClip != ammoCapacity && ammoCarriedByPlayer > 0)
        {
            StartCoroutine(Reload());
        }

        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire && currentAmmoInClip > 0)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        muzzleFlash.Play();

        currentAmmoInClip--;

        RaycastHit hit;
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, range))
        {
            Target target = hit.transform.GetComponent<Target>();

            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }
     }

    IEnumerator Reload()
    {
        isReloading = true;

        gunModel.SetActive(false);

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
