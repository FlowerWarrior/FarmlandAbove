using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroyOnStart : MonoBehaviour
{
    [SerializeField] float timeDelay;

    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(DestroyAfter(timeDelay));
    }

    IEnumerator DestroyAfter(float sec)
    {
        yield return new WaitForSeconds(sec);
        Destroy(gameObject);
    }
}
