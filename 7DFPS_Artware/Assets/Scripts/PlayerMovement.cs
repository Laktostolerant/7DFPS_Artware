using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //XD
    float walkingSpeed;
    float runningSpeed;
    public Camera playerCamera;
    public float lookSensitivity = 2.0f;
    public float lookXLimit = 45.0f;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    [HideInInspector]
    public bool canMove = true;

    public float t = 0.01f;
    Coroutine fovCoroutine;

    RaycastHit hit;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (!Physics.Raycast(transform.position, Vector3.down, out hit, 0.01f))
            characterController.Move(new Vector3(0, -0.08f, 0));

        bool isCrouching = (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.C));
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        bool isAiming = Input.GetKey(KeyCode.Mouse1);

        if (isCrouching)
            Crouch(1, 2, 3, 0.5f);
        else if(!isCrouching && !Physics.Raycast(transform.position, Vector3.up, out hit, 1.4f))
            Crouch(2, 4, 7, 0.85f);

        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;
        
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        //PHYSICAL MOVE
        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSensitivity;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSensitivity, 0);

            if (fovCoroutine != null)
                StopCoroutine(fovCoroutine);

            if (isRunning && moveDirection.x != 0 && moveDirection.z != 0)
                fovCoroutine = StartCoroutine(ZoomCoroutine(playerCamera, 80, 0.5f));

            if(!isRunning && isAiming)
                fovCoroutine = StartCoroutine(ZoomCoroutine(playerCamera, 30, 0.5f));

            if (!isRunning && !isAiming)
                fovCoroutine = StartCoroutine(ZoomCoroutine(playerCamera, 60, 0.25f));
        }
    }

    IEnumerator ZoomCoroutine(Camera targetCamera, float toFOV, float duration)
    {
        float counter = 0;
        while (counter < duration)
        {
            counter += Time.deltaTime;

            float fOVTime = counter / duration;

            targetCamera.fieldOfView = Mathf.Lerp(targetCamera.fieldOfView, toFOV, fOVTime);
            yield return null;
        }
    }

    void Crouch(int height, int walk, int run, float heightOffset)
    {
        characterController.height = height;
        walkingSpeed = walk;
        runningSpeed = run;
        playerCamera.transform.position = new Vector3(transform.position.x, transform.position.y + heightOffset, transform.position.z);
    }
}
