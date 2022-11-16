using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsUpdater : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI coinsCountText;

    void OnEnable()
    {
        try
        {
            UpdateCount(InventorySystem.instance.GetCoins());
            UIMGR.UpdateCoinsUI += UpdateCount;
        } 
        catch { }
    }

    void Start()
    {
        UpdateCount(InventorySystem.instance.GetCoins());
        UIMGR.UpdateCoinsUI += UpdateCount;
    }

    void OnDisable()
    {
        UIMGR.UpdateCoinsUI -= UpdateCount;
    }

    void UpdateCount(int value)
    {
        coinsCountText.text = $"{value}";
    }
}
