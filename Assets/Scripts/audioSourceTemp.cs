using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioSourceTemp : MonoBehaviour
{
    AudioSource myAudioSource; 

    public void MyStart()
    {
        StopAllCoroutines();
        myAudioSource = GetComponent<AudioSource>();
        myAudioSource.Play();
        StartCoroutine(DestroyAfter(myAudioSource.clip.length));
    }

    IEnumerator DestroyAfter(float sec)
    {
        yield return new WaitForSeconds(sec);
        Destroy(gameObject);
    }
}
