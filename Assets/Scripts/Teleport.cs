using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] Transform destination;
    [SerializeField] float teleportAfter;

    float timer = 0f;
    bool isPlayerInside = false;

    public static System.Action Teleported;

    private void Update()
    {
        if (isPlayerInside)
            timer += Time.deltaTime;
        if (timer >= teleportAfter)
        {
            PlayerRespawner.instance.TeleportPlayerTo(destination);
            Teleported?.Invoke();
            isPlayerInside = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            timer = 0f;
            isPlayerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            timer = 0f;
            isPlayerInside = false;
        }
    }
}
