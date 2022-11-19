using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchesMgr : MonoBehaviour
{
    [SerializeField] internal Transform _ghostHolder;
    [SerializeField] internal Transform[] _realTorchHolders;

    internal static TorchesMgr instance;

    private void Awake()
    {
        instance = this;
        PlayerRespawner.PlayerFellBelow += RemoveGhostTorches;
    }

    private void RemoveGhostTorches()
    {
        for (int i = 0; i < _ghostHolder.childCount; i++)
        {
            Destroy(_ghostHolder.GetChild(i).gameObject);
        }
    }

    public bool CanPlaceOnIsland(int islandIndex)
    {
        return (_realTorchHolders[islandIndex].childCount < 2);
    }
}
