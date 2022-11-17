using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedLockPanel : MonoBehaviour
{
    [SerializeField] int requiredBridgeId;

    private void OnEnable()
    {
        if (IslandsMgr.instance.bridgesBuilt[requiredBridgeId])
        {
            gameObject.SetActive(false);
        }
    }
}
