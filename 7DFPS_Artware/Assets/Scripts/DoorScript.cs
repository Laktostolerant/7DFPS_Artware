using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorScript : MonoBehaviour
{
    bool isPlayerCloseEnough;
    [SerializeField] bool transitionaryDoor;
    [SerializeField] GameObject infoText;
    [SerializeField] BoxCollider triggerCollider;
    [SerializeField] int sceneToTransitionTo;
    [SerializeField] Animator transitionAnimator;
    [SerializeField] Animator animator;

    bool opened;
    bool changedScene;

    private void Start()
    {
        isPlayerCloseEnough = false;
        opened = true;
        changedScene = true;
        infoText.SetActive(false);
    }

    void Update()
    {
        if (isPlayerCloseEnough && Input.GetKeyDown(KeyCode.E) && !transitionaryDoor)
        {
            opened = !opened;

            infoText.SetActive(false);

            Destroy(triggerCollider);
        }

        if (isPlayerCloseEnough && Input.GetKeyDown(KeyCode.E) && transitionaryDoor)
        {
            opened = !opened;

            changedScene = !changedScene;

            animator.SetBool("Opened", !opened);

            transitionAnimator.SetTrigger("SceneChange");

            infoText.SetActive(false);

            Destroy(triggerCollider);

            StartCoroutine(SceneTransition());
        }
    }

    IEnumerator SceneTransition()
    {
        yield return new WaitForSeconds(3f);
        transitionAnimator.SetTrigger("SceneChange");
        SceneManager.LoadScene(sceneToTransitionTo);
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