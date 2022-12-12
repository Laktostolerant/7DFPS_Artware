using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpWeapons : MonoBehaviour
{
    bool isPlayerCloseEnough;
    public static bool pickedUpWeapon;

    [SerializeField] GameObject gunOnGround;
    [SerializeField] GameObject gunInHands;
    [SerializeField] GameObject infoText;
    [SerializeField] GameObject gunToHolster;

    [SerializeField] int lowestAmmoCount;
    [SerializeField] int highestAmmoCount;
    int ammoToAddToInventory;
    
    void Start()
    {
        gunOnGround.SetActive(true);
        infoText.SetActive(false);
        gunInHands.SetActive(false);
        CheckWeaponsOnPlayer.hasPistol = false;
        CheckWeaponsOnPlayer.hasSMG = false;
    }

    void Update()
    {
        if (isPlayerCloseEnough && Input.GetKeyDown(KeyCode.E) && !pickedUpWeapon)
        {
            infoText.SetActive(false);
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

        if (isPlayerCloseEnough && Input.GetKeyDown(KeyCode.E) && !pickedUpWeapon && CheckWeaponsOnPlayer.hasPistol)
        {
            infoText.SetActive(false);
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

            if (!pickedUpWeapon)
            {
                infoText.SetActive(true);
            }            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerCloseEnough = false;

            if (!pickedUpWeapon)
            {
                infoText.SetActive(true);
            }
        }
    }
}
