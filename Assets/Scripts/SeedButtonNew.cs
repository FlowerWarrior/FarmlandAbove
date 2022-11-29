using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SeedButtonNew : MonoBehaviour
{
    [Header("Adjustables")]
    [SerializeField] SeedData seed;
    [Header("References")]
    [SerializeField] Image imgSeed;
    [SerializeField] Image imgBg;
    [SerializeField] TextMeshProUGUI txtName;
    [SerializeField] TextMeshProUGUI txtCost;
    [SerializeField] TextMeshProUGUI txtValue;

    // Start is called before the first frame update
    void Start()
    {
        imgSeed.sprite = seed.inventorySprite;
        imgBg.color = CodeSF.GetRarityColor(seed.rarity);
        txtName.text = $"{seed.itemName}";
        txtCost.text = $"COST: {seed.cost}$";
        txtValue.text = $"CROPS: {seed.plantClass.value[0]}$";
    }

    public void OnClicked()
    {
        if (seed.itemName != "Beetroot" && QuestMgr.instance.currentQuest < quest.None)
            return;
        if (QuestMgr.instance.currentQuest > quest.BuySeed2 && QuestMgr.instance.currentQuest < quest.AgainBuySeed0)
            return;
        if (QuestMgr.instance.currentQuest > quest.AgainBuySeed2 && QuestMgr.instance.currentQuest < quest.None)
            return;

        if (InventorySystem.instance.GetCoins() >= seed.cost)
        {
            UIMGR.instance.OpenReceivedSeedMenu(seed);
        }
        else
        {
            NotEnoughMgr.instance.ShowNotEnoughNotifAtCursor();
        }
    }
}
