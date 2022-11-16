using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataMgr : MonoBehaviour
{
    [SerializeField] private SeedData[] seeds;
    internal static DataMgr instance;

    private void Awake()
    {
        instance = this;
    }

    public SeedData[] GetAllSeeds()
    {
        return seeds;
    }

    public SeedData GetRandomSeed()
    {
        return seeds[Random.Range(0, seeds.Length)];
    }
}
