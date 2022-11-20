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

    public float GetLightLevel(Vector3 pos, int islandId)
    {
        float lightLevel = 0;
        for (int i = 0; i < _realTorchHolders[islandId].childCount; i++)
        {
            lightLevel += 1f / Vector3.Distance(pos, _realTorchHolders[islandId].GetChild(i).position);
        }
        return Mathf.Clamp(lightLevel, 0, 1);
    }
}
