using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] float health;
    [SerializeField] bool isHuman;
    [SerializeField] bool isFakeSoldier;
    [SerializeField] GameObject soldierModel;
    [SerializeField] GameObject doctorModel;

    private void Start()
    {
        doctorModel.SetActive(false);
    }

    public void TakeDamage(float amountOfDmg)
    {
        health -= amountOfDmg;
        if (health <= 0 && !isHuman)
        {
            Destroy(gameObject);
        }

        if (health <= 0 && isHuman && !isFakeSoldier)
        {
            Destroy(gameObject); //Set dying animation instead of Destroy
            Debug.Log("You killed a normal soldier");
        }

        if (health <= 0 && isHuman && isFakeSoldier)
        {
            Destroy(soldierModel);
            doctorModel.SetActive(true); //Fix so it triggers the character models and animations
            Debug.Log("You killed an innocent doctor!");
        }
    }


}
