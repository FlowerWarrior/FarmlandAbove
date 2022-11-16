using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SeedShopMgr : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI commonCostTxt;
    [SerializeField] TextMeshProUGUI rareCostTxt;
    [SerializeField] TextMeshProUGUI epicCostTxt;
    internal SeedShopPoint shopPoint;

    private void OnEnable()
    {
        commonCostTxt.text = $"{shopPoint.seedsCost[0]}$";
        rareCostTxt.text = $"{shopPoint.seedsCost[1]}$";
        epicCostTxt.text = $"{shopPoint.seedsCost[2]}$";
    }

    public void OnClickButtonCommon()
    {
        int cost = shopPoint.seedsCost[0];
        if (InventorySystem.instance.GetCoins() >= cost)
        {
            InventorySystem.instance.SubstractCoins(cost);
            //UIMGR.instance.OpenReceivedSeedMenu(Rarity.Common);
        }
        else
        {
            NotEnoughMgr.instance.ShowNotEnoughNotifAtCursor();
        }
    }

    public void OnClickButtonRare()
    {
        int cost = shopPoint.seedsCost[1];
        if (InventorySystem.instance.GetCoins() >= cost)
        {
            InventorySystem.instance.SubstractCoins(cost);
            //UIMGR.instance.OpenReceivedSeedMenu(Rarity.Rare);
        }
        else
        {
            NotEnoughMgr.instance.ShowNotEnoughNotifAtCursor();
        }
    }

    public void OnClickButtonEpic()
    {
        int cost = shopPoint.seedsCost[2];
        if (InventorySystem.instance.GetCoins() >= cost)
        {
            InventorySystem.instance.SubstractCoins(cost);

            //UIMGR.instance.OpenReceivedSeedMenu(Rarity.Epic);
        }
        else
        {
            NotEnoughMgr.instance.ShowNotEnoughNotifAtCursor();
        }
    }
}
