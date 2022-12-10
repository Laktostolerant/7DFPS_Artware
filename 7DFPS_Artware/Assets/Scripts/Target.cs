using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] float health;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float amountOfDmg)
    {
        health -= amountOfDmg;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
