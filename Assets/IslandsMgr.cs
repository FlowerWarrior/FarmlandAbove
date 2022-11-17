using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandsMgr : MonoBehaviour
{
    [SerializeField] GameObject[] islands;
    internal bool[] bridgesBuilt = { false, false };

    internal static IslandsMgr instance;
    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        BridgeSign.BridgeBuilt += OnBridgeBuilt;
    }

    private void OnBridgeBuilt(Vector3 pos, int bridgeId)
    {
        bridgesBuilt[bridgeId] = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 1; i < islands.Length; i++)
        {
            islands[i].SetActive(false);
        }
    }
}
