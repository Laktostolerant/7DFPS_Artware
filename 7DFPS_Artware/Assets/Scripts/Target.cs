using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Target : MonoBehaviour
{
    bool isAlive;
    [SerializeField] float health;
    [SerializeField] bool isHuman;
    [SerializeField] bool isFakeSoldier;
    [SerializeField] GameObject soldierModel;
    [SerializeField] GameObject dyingDoctor;
    [SerializeField] Animator animator;
    [SerializeField] NavMeshAgent navMesh;
    [SerializeField] EnemyCombat enemyShootScript;
    [SerializeField] GameObject gunToDrop;
    Vector3 soldierPosition;

    private void Start()
    {
        isAlive = true;
    }

    private void Update()
    {
        soldierPosition = GameObject.Find("EnemySoldierFake").transform.position;
    }

    public void TakeDamage(float amountOfDmg)
    {
        health -= amountOfDmg;
        if (health <= 0 && !isHuman)
        {
            Destroy(gameObject);
        }

        if (health <= 0 && isHuman && !isFakeSoldier && isAlive)
        {
            animator.SetBool("dead", true);
            Debug.Log("You killed a normal soldier");
            navMesh.enabled = false;
            Destroy(enemyShootScript);
            Instantiate(gunToDrop, soldierPosition, Quaternion.identity);
            isAlive = false;
        }

        if (health <= 0 && isHuman && isFakeSoldier)
        {
            Destroy(soldierModel);
            //doctorModel.SetActive(true); //Fix so it triggers the character models and animations
            Instantiate(dyingDoctor, soldierPosition, Quaternion.identity);
            Debug.Log("You killed an innocent doctor!");
        }
    }
}
