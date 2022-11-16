using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SeedReceivedMgr : MonoBehaviour
{
    [SerializeField] GameObject panelResult;
    [SerializeField] GameObject panelRandomPick;
    [SerializeField] Transform randomPickItemsParent;
    [SerializeField] GameObject randomPickItemPrefab;
    [SerializeField] Image seedImage;
    [SerializeField] Image seedBgImage;
    [SerializeField] TextMeshProUGUI seedRarityText;
    [SerializeField] SeedShopMgr seedShopMgr;
    [SerializeField] float animTime = 2.5f;

    internal static System.Action Bought;
    internal static System.Action ItemShowReceived;
    internal Rarity chestRarity;
    public void UpdateSeedUI(Rarity rarity)
    {
        Bought?.Invoke();
        SeedData seed = new SeedData();
        chestRarity = rarity;

        // Start a random pick panel
        CodeSF.DestroyAllChildren(randomPickItemsParent);

        int itemsCount = 20;
        for (int i = 0; i < itemsCount; i++)
        {
            GameObject newItem = Instantiate(randomPickItemPrefab, randomPickItemsParent);

            if (chestRarity == Rarity.Common)
            {
                newItem.GetComponent<ItemRandomPick>().seed = GetCommonSeed();
            }
            else if (chestRarity == Rarity.Rare)
            {
                newItem.GetComponent<ItemRandomPick>().seed = GetRareSeed();
            }
            else if(chestRarity == Rarity.Epic)
            {
                newItem.GetComponent<ItemRandomPick>().seed = GetEpicSeed();
            }

            if (i == itemsCount-2)
            {
                seed = newItem.GetComponent<ItemRandomPick>().seed;
            }
        }
        panelResult.SetActive(false);
        panelRandomPick.SetActive(true);

        seedImage.sprite = seed.inventorySprite;
        seedBgImage.color = CodeSF.GetRarityColor(seed.rarity);
        seedRarityText.text = seed.rarity.ToString();
        InventorySystem.instance.AddItemToInventory(seed);

        StartCoroutine(ShowResultAfter(animTime));
    }

    private SeedData GetRandomSeed(int commonChance, int rareChance, int epicChance)
    {
        SeedData resultSeed = new SeedData();
        int r = Random.Range(0, commonChance + rareChance + epicChance);
        if (r >= 0 && r < commonChance)
        {
            resultSeed = seedShopMgr.shopPoint.seedsCommon[Random.Range(0, seedShopMgr.shopPoint.seedsCommon.Length)];
        }
        else if (r >= commonChance && r < commonChance + rareChance)
        {
            resultSeed = seedShopMgr.shopPoint.seedsRare[Random.Range(0, seedShopMgr.shopPoint.seedsRare.Length)];
        }
        else if (r >= commonChance + rareChance && r < commonChance + rareChance + epicChance)
        {
            resultSeed = seedShopMgr.shopPoint.seedsEpic[Random.Range(0, seedShopMgr.shopPoint.seedsEpic.Length)];
        }
        return resultSeed;
    }

    private SeedData GetCommonSeed()
    {
        int commonChance = 90;
        int rareChance = 8;
        int epicChance = 2;
        return GetRandomSeed(commonChance, rareChance, epicChance);
    }

    private SeedData GetRareSeed()
    {
        int commonChance = 50;
        int rareChance = 40;
        int epicChance = 10;
        return GetRandomSeed(commonChance, rareChance, epicChance);
    }

    private SeedData GetEpicSeed()
    {
        int commonChance = 10;
        int rareChance = 40;
        int epicChance = 50;
        return GetRandomSeed(commonChance, rareChance, epicChance);
    }

    private void OnRebuyCommon()
    {
        int cost = seedShopMgr.shopPoint.seedsCost[0];
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

    private void OnRebuyRare()
    {
        int cost = seedShopMgr.shopPoint.seedsCost[1];
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

    private void OnRebuyEpic()
    {
        int cost = seedShopMgr.shopPoint.seedsCost[2];
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

    public void OnBuyAgain()
    {
        if (chestRarity == Rarity.Common)
        {
            OnRebuyCommon();
        }
        else if (chestRarity == Rarity.Rare)
        {
            OnRebuyRare();
        }
        else if (chestRarity == Rarity.Epic)
        {
            OnRebuyEpic();
        }
    }

    IEnumerator ShowResultAfter(float sec)
    {
        yield return new WaitForSeconds(sec);
        panelRandomPick.SetActive(false);
        panelResult.SetActive(true);
        ItemShowReceived?.Invoke();
    }
}
