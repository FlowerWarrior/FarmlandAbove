using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudsFollow : MonoBehaviour
{
    [SerializeField] float height = -220f;

    // Update is called once per frame
    void Update()
    {
        if (PlayerRespawner.instance.rb != null)
        {
            transform.position = PlayerRespawner.instance.rb.transform.position + Vector3.up * height;
        }
    }
}
