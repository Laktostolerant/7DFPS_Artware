using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFade : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(BulletLifespan());
    }

    IEnumerator BulletLifespan()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
