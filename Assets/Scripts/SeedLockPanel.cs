using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedLockPanel : MonoBehaviour
{
    [SerializeField] int requiredIslandId;

    private void OnEnable()
    {
        if (ZoneTrigger.instance.discoveredIslands[requiredIslandId])
        {
            gameObject.SetActive(false);
        }
    }
}
