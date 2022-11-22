using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SeedReceivedNew : MonoBehaviour
{
    [SerializeField] GameObject panelResult;
    [SerializeField] Image seedImage;
    [SerializeField] Image seedBgImage;
    [SerializeField] TextMeshProUGUI seedValueTxt;
    [SerializeField] TextMeshProUGUI seedCostTxt;

    internal static System.Action Bought;
    SeedData mySeed;

    public void ShowSeed(SeedData seed)
    {
        mySeed = seed;
        seedImage.sprite = seed.inventorySprite;
        seedBgImage.color = CodeSF.GetRarityColor(seed.rarity);
        seedValueTxt.text = "Value: " + seed.plantClass.value[0].ToString() + "$";
        seedCostTxt.text = "Cost: " + seed.cost.ToString() + "$";
        panelResult.SetActive(true);
    }

    public void OnBuyClicked()
    {
        if (QuestMgr.instance.currentQuest > quest.BuySeed2 && QuestMgr.instance.currentQuest < quest.AgainBuySeed0)
        {
            return;
        }
        if (QuestMgr.instance.currentQuest > quest.AgainBuySeed2 && QuestMgr.instance.currentQuest < quest.None)
        {
            return;
        }

        if (InventorySystem.instance.GetCoins() >= mySeed.cost)
        {
            InventorySystem.instance.SubstractCoins(mySeed.cost);
            InventorySystem.instance.AddItemToInventory(mySeed);
            Bought?.Invoke();
        }
        else
        {
            NotEnoughMgr.instance.ShowNotEnoughNotifAtCursor();
        }
    }
}
