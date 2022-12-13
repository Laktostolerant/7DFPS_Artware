using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpWeapons : MonoBehaviour
{
    bool isPlayerCloseEnough;
    public static bool pickedUpWeapon;

    [SerializeField] GameObject gunOnGround;
    [SerializeField] GameObject gunInHands;
    [SerializeField] GameObject gunToHolster;

    [SerializeField] int lowestAmmoCount;
    [SerializeField] int highestAmmoCount;
    int ammoToAddToInventory;
    
    void Start()
    {
        gunOnGround.SetActive(true);
        gunInHands.SetActive(false);
        CheckWeaponsOnPlayer.hasPistol = false;
        CheckWeaponsOnPlayer.hasSMG = false;
    }

    void Update()
    {
        if (isPlayerCloseEnough && !pickedUpWeapon)
        {
            pickedUpWeapon = true;
            gunInHands.SetActive(true);
            CheckWeaponsOnPlayer.hasSMG = true;
            SwitchWeaponScript.weaponSelected = 0;
            Destroy(gunOnGround);
        }

        if (isPlayerCloseEnough && pickedUpWeapon)
        {
            ammoToAddToInventory = Random.Range(lowestAmmoCount, highestAmmoCount + 1);
            WeaponScript.ammoCarriedByPlayer += ammoToAddToInventory;
            gunOnGround.SetActive(false);
        }

        if (isPlayerCloseEnough && !pickedUpWeapon && CheckWeaponsOnPlayer.hasPistol)
        {
            pickedUpWeapon = true;
            gunInHands.SetActive(true);
            CheckWeaponsOnPlayer.hasSMG = true;
            SwitchWeaponScript.weaponSelected = 0;
            gunToHolster.SetActive(false);
            Destroy(gunOnGround);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerCloseEnough = true;        
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerCloseEnough = false;
        }
    }
}
