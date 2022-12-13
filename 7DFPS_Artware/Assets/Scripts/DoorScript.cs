using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    [SerializeField] Transform PlayerCamera;
    [Header("MaxDistance you can open or close the door")]
    [SerializeField] float MaxDistance = 5;

    private bool opened = false;
    private Animator animator;
    [SerializeField] GameObject infoText;

    private void Start()
    {
        opened = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Pressed();
        }

        Debug.Log(opened);
    }

    void Pressed()
    {
        RaycastHit doorhit;

        if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.forward, out doorhit, MaxDistance))
        {
            opened = true;
            if (doorhit.transform.tag == "Door")
            {
                animator = doorhit.transform.GetComponentInParent<Animator>();

                opened = !opened;

                animator.SetBool("Opened", !opened);
            }
        }
    }
}