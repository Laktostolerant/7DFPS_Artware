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
    public static int ammoToAddToInventory = 30;
    // Start is called before the first frame update
    void Start()
    {
        gunOnGround.SetActive(true);
        infoText.SetActive(false);
        gunInHands.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerCloseEnough && Input.GetKeyDown(KeyCode.E) && !pickedUpWeapon)
        {
            //Destroy(gunOnGround);
            gunOnGround.SetActive(false);
            infoText.SetActive(false);
            pickedUpWeapon = true;
            gunInHands.SetActive(true);
        }

        if (isPlayerCloseEnough && Input.GetKeyDown(KeyCode.E) && pickedUpWeapon)
        {
            gunOnGround.SetActive(false);
            infoText.SetActive(false);
            WeaponScript.ammoCarriedByPlayer += ammoToAddToInventory;
        }

        Debug.Log(pickedUpWeapon);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerCloseEnough = true;

            infoText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            isPlayerCloseEnough = false;

            infoText.SetActive(false);
        }
    }
}
