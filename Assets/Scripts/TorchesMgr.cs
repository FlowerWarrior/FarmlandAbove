using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchesMgr : MonoBehaviour
{
    [SerializeField] internal Transform _ghostHolder;
    [SerializeField] internal Transform[] _realTorchHolders;
    [SerializeField] internal Transform[] _placeTargets;

    internal static TorchesMgr instance;

    private void Awake()
    {
        instance = this;
        PlayerRespawner.PlayerFellBelow += RemoveGhostTorches;
    }

    private void Start()
    {
        ReshufflePlaceTargets();
        TogglePlaceTargetsVisiblity(false);
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
            float distance = Vector3.Distance(pos, _realTorchHolders[islandId].GetChild(i).position);
            lightLevel += 1f / (Mathf.Pow(distance, 0.48f));
        }
        return Mathf.Clamp(lightLevel, 0, 1);
    }
    
    public void ReshufflePlaceTargets()
    {
        for (int i = 0; i < _placeTargets.Length; i++)
        {
            for (int k = 0; k < _placeTargets[i].childCount; k++)
            {
                // disable all
                for (int m = 0; m < _placeTargets[i].GetChild(k).childCount; m++)
                {
                    _placeTargets[i].GetChild(k).GetChild(m).gameObject.SetActive(false);
                }

                // enable random
                int rnd = Random.Range(0, _placeTargets[i].GetChild(k).childCount);
                _placeTargets[i].GetChild(k).GetChild(rnd).gameObject.SetActive(true);
            }

        }
    }

    public void TogglePlaceTargetsVisiblity(bool newState)
    {
        for (int i = 0; i < _placeTargets.Length; i++)
        {
            _placeTargets[i].gameObject.SetActive(newState);
        }
    }
}
