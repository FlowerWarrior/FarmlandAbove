using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsAddPrefab : MonoBehaviour
{
    [SerializeField] float lifeSpan;

    void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(DestroyAfter(lifeSpan));
    }

    IEnumerator DestroyAfter(float sec)
    {
        yield return new WaitForSeconds(sec);
        Destroy(gameObject);
    }
}
