using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchWeaponScript : MonoBehaviour
{
    public static int weaponSelected;
    [SerializeField] GameObject SMGMagText;
    [SerializeField] GameObject SMGSpareMagText;
    [SerializeField] GameObject PistolMagText;
    [SerializeField] GameObject PistolSpareMagText;
    [SerializeField] GameObject Pistol;
    [SerializeField] GameObject SMG;
    bool oneTimeBoolForWeaponSwitch = false;

    void Start()
    {
        Pistol.SetActive(false);
        SMG.SetActive(false);
    }

    void Update()
    {
        int previousWeapon = weaponSelected;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f && PickUpWeapons.pickedUpWeapon && PickUpPistol.pickedUpWeapon)
        {
            if (weaponSelected >= transform.childCount - 1)
            {
                weaponSelected = 0;
            }
            else
            {
                weaponSelected++;
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f && PickUpWeapons.pickedUpWeapon && PickUpPistol.pickedUpWeapon)
        {
            if (weaponSelected <= 0)
            {
                weaponSelected = transform.childCount - 1;
            }
            else
            {
                weaponSelected--;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            weaponSelected = 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount >= 2)
        {
            weaponSelected = 1;
        }

        if (weaponSelected == 0)
        {
            SMGMagText.SetActive(true);
            SMGSpareMagText.SetActive(true);
            PistolMagText.SetActive(false);
            PistolSpareMagText.SetActive(false);
        }

        if (weaponSelected == 1)
        {
            SMGMagText.SetActive(false);
            SMGSpareMagText.SetActive(false);
            PistolMagText.SetActive(true);
            PistolSpareMagText.SetActive(true);
        }

        if (previousWeapon != weaponSelected)
        {
            WeaponSelection();
        }

        SwitchStartingWeapon();
    }

    void SwitchStartingWeapon()
    {
        if (!oneTimeBoolForWeaponSwitch && PickUpWeapons.pickedUpWeapon && PickUpPistol.pickedUpWeapon)
        {
            SMG.SetActive(false);
            Pistol.SetActive(true);
            weaponSelected = 1;
            oneTimeBoolForWeaponSwitch = true;
        }
        
    }

    void WeaponSelection()
    {
        int weaponNumber = 0;

        foreach (Transform weapon in transform)
        {
            if (weaponNumber == weaponSelected)
            {
                weapon.gameObject.SetActive(true);
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
            weaponNumber++;
        }
    }
}
