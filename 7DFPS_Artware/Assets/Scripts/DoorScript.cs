using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    bool isPlayerCloseEnough;
    [SerializeField] GameObject infoText;
    [SerializeField] BoxCollider triggerCollider;

    bool opened = false;
    Animator animator;

    private void Start()
    {
        isPlayerCloseEnough = false;
        opened = true;
        infoText.SetActive(false);
    }

    void Update()
    {
        if (isPlayerCloseEnough && Input.GetKeyDown(KeyCode.E))
        {
            animator = transform.GetComponentInParent<Animator>();

            opened = !opened;

            animator.SetBool("Opened", !opened);

            infoText.SetActive(false);

            Destroy(triggerCollider);
        }
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