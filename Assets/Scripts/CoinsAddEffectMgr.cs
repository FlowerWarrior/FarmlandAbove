using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsAddEffectMgr : MonoBehaviour
{
    [SerializeField] GameObject effectPrefab;

    private void OnEnable()
    {
        InventorySystem.CoinsUpdated += SpawnCoinsEffect;
    }

    private void OnDisable()
    {
        InventorySystem.CoinsUpdated -= SpawnCoinsEffect;
    }

    private void SpawnCoinsEffect(int deltaCoins)
    {
        if (deltaCoins == 0)
            return;

        GameObject obj = Instantiate(effectPrefab, transform);

        string displayText;
        if (deltaCoins > 0)
            displayText = $"+{deltaCoins}";
        else
            displayText = $"{deltaCoins}";

        obj.transform.GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = displayText;
    }
}
