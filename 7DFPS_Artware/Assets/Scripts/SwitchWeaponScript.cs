using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchWeaponScript : MonoBehaviour
{
    [SerializeField] int weaponSelected;
    [SerializeField] GameObject SMGAmmo;
    [SerializeField] GameObject PistolAmmo;

    // Start is called before the first frame update
    void Start()
    {
        WeaponSelection();
    }

    // Update is called once per frame
    void Update()
    {
        int previousWeapon = weaponSelected;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
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
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
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

        if (previousWeapon != weaponSelected)
        {
            WeaponSelection();
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
