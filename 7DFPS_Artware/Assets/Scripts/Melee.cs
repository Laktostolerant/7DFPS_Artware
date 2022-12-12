using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] float range;
    [SerializeField] float fireRate;
    [SerializeField] float impactForce;
    [SerializeField] AudioSource meleeAudioSource;
    [SerializeField] AudioClip meleeSound;


    float nextTimeToFire;

    bool isReloading;

    [SerializeField] Camera camera;

    void Update()
    {
        if (Input.GetKey(KeyCode.V) && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Punch();
        }
    }

    void Punch()
    {
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out RaycastHit hit, range))
        {
            Target target = hit.transform.GetComponent<Target>();

            if (target != null)
            {
                target.TakeDamage(damage);
                hit.rigidbody.AddForce(-hit.normal * impactForce);
                meleeAudioSource.PlayOneShot(meleeSound);
            }
        }
    }
}
