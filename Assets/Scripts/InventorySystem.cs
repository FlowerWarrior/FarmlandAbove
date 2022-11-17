using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    List<Item> items = new List<Item>();
    [SerializeField] int coins = 0;
    int treesCut = 0;
    int goo = 0;

    internal static InventorySystem instance;
    internal static System.Action<int> CoinsUpdated;
    internal static System.Action TreesCutStatUpdated;
    internal static System.Action CoinsAdded;
    internal static System.Action ItemSold;

    #region Methods

    public void AddTreeCutCounter()
    {
        treesCut++;
        TreesCutStatUpdated?.Invoke();
    }

    public int GetTreesCut()
    {
        return treesCut;
    }

    public void AddGooToInv()
    {
        goo++;
    }

    public void RemoveGooFromInv()
    {
        goo--;
        if (goo < 0)
            goo = 0;
    }

    public int GetGooCount()
    {
        return goo;
    }

    public void AddItemToInventory(Item newItem)
    {
        if (newItem is SeedData)
        {
            SeedData newSeed = newItem as SeedData;
            SeedData seedInstance = ScriptableObject.CreateInstance("SeedData") as SeedData;

            seedInstance.cost = newSeed.cost;
            seedInstance.name = newSeed.name;
            seedInstance.itemName = newSeed.itemName;
            seedInstance.inventorySprite = newSeed.inventorySprite;
            seedInstance.plantClass = newSeed.plantClass;
            seedInstance.rarity = newSeed.rarity;
            seedInstance.seedLvl = newSeed.seedLvl;

            items.Add(seedInstance);
        }

        if (newItem is VegetableData)
        {
            VegetableData newVeggie = newItem as VegetableData;
            VegetableData veggieInstance = ScriptableObject.CreateInstance("VegetableData") as VegetableData;

            veggieInstance.name = newVeggie.name;
            veggieInstance.itemName = newVeggie.itemName;
            veggieInstance.inventorySprite = newVeggie.inventorySprite;
            veggieInstance.prefab = newVeggie.prefab;
            veggieInstance.seedClass = newVeggie.seedClass;
            veggieInstance.fullyGrownTicksRequired = newVeggie.fullyGrownTicksRequired;
            veggieInstance.value = newVeggie.value;
            veggieInstance.waterThirst = newVeggie.waterThirst;
            veggieInstance.rarity = newVeggie.rarity;
            veggieInstance.seedLvl = newVeggie.seedLvl;

            items.Add(veggieInstance);
        }
    }

    public void SellItem(Item thisItem)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == thisItem)
            {
                if (items[i] is VegetableData)
                {
                    VegetableData thisVegetable = (VegetableData)items[i];
                    AddCoins(thisVegetable.value[thisVegetable.seedLvl-1]);
                }
                RemoveItemFromInventory(thisItem);
                ItemSold?.Invoke();
                break;
            }
        }
    }

    public void SellAllItems()
    {
        Item[] sellableItems = GetSellableItems();

        if (sellableItems.Length > 0)
        {
            int valueSum = 0;
            for (int i = 0; i < sellableItems.Length; i++)
            {
                if (sellableItems[i] is VegetableData)
                {
                    VegetableData thisVegetable = (VegetableData)sellableItems[i];
                    valueSum += thisVegetable.value[thisVegetable.seedLvl - 1];
                    RemoveItemFromInventory(sellableItems[i]);
                }
            }
            AddCoins(valueSum);
            ItemSold?.Invoke();
        }
    }

    public void RemoveItemFromInventory(Item thisItem)
    {
        items.Remove(thisItem);
    }

    private void CollectHarvestedPlant(VegetableData veggie, SeedData seed)
    {
        AddItemToInventory(veggie);
        //AddItemToInventory(seed);

        // 20% chance to get double seeds - now legacy
        //if (Random.Range(0,5) == 0)
            //AddItemToInventory(((VegetableData)newItem).seedClass);
    }

    public int GetItemsCount()
    {
        return items.Count;
    }

    public Item[] GetSortedItemsAll()
    {
        return SortList(items).ToArray();
    }

    private List<Item> SortList(List<Item> listToSort)
    {
        List<Item> result = new List<Item>();
        result.AddRange(listToSort);

        result.Sort((e1, e2) =>
        {
            int result1 = (e2 is SeedData).CompareTo(e1 is SeedData);
            int result2 = result1 == 0 ? e2.rarity.CompareTo(e1.rarity) : result1;
            int result3 = result2 == 0 ? e2.itemName.CompareTo(e1.itemName) : result2;
            return result3 == 0 ? e2.seedLvl.CompareTo(e1.seedLvl) : result3;
        });

        return result;
    }

    public Item GetItemAtIndex(int index)
    {
        return items[index];
    }

    public Item[] GetSellableItems()
    {
        List<Item> result = new List<Item>();

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] is VegetableData)
            {
                result.Add(items[i]);
            }
        }

        return result.ToArray();
    }

    public Item[] GetAcceptableSeeds(SeedData[] acceptableSeeds)
    {
        List<Item> result = new List<Item>();

        for (int i = 0; i < items.Count; i++)
        {
            for (int x = 0; x < acceptableSeeds.Length; x++)
            {
                if (items[i].name == acceptableSeeds[x].name)
                {
                    result.Add(items[i]);
                    break;
                }
            }
        }
        return SortList(result).ToArray();
    }

    public Item[] GetMergerSeeds(SeedData seed)
    {
        List<Item> result = new List<Item>();

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] is SeedData)
            {
                SeedData thisSeedData = items[i] as SeedData;
                if (thisSeedData.itemName == seed.itemName && thisSeedData.seedLvl == seed.seedLvl)
                {
                    result.Add(items[i]);
                }
            }
        }

        return result.ToArray();
    }

    public int GetCoins()
    {
        return coins;
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        CoinsUpdated?.Invoke(amount);
        CoinsAdded?.Invoke();
    }

    public void SubstractCoins(int amount)
    {
        coins -= amount;
        CoinsUpdated?.Invoke(-amount);
    }

    #endregion

    #region Monobehaviour Callbacks

    private void Awake() => instance = this;

    private void OnEnable()
    {
        GrowSlot.PlantHarvested += CollectHarvestedPlant;
    }

    private void OnDisable()
    {
        GrowSlot.PlantHarvested -= CollectHarvestedPlant;
    }

    #endregion
}
