using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] float health;

    public void TakeDamage(float amountOfDmg)
    {
        health -= amountOfDmg;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
