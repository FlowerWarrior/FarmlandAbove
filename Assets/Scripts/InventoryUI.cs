using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : I_ItemsGrid
{
    [SerializeField] GameObject itemPrefab;
    [SerializeField] RectTransform mergeButtonRT;
    [SerializeField] RectTransform deleteButtonRT;
    ItemUI selectedButton = null;

    internal static System.Action Opened;
    internal static System.Action Closed;
    internal static System.Action MergeComplete;

    public void RefreshItems()
    {
        CodeSF.DestroyAllChildren(transform);

        Item[] items = InventorySystem.instance.GetSortedItemsAll();

        for (int i = 0; i < items.Length; i++)
        {
            GameObject newItem = Instantiate(itemPrefab, transform.position, Quaternion.identity, transform);
            newItem.GetComponent<ItemUI>().UpdateItem(items[i]);
        }

        int itemRows = Mathf.CeilToInt((float)items.Length / 6f);
        transform.parent.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 164 * itemRows);
    }

    private void OnEnable()
    {
        Opened?.Invoke();
        RefreshItems();
        mergeButtonRT.gameObject.SetActive(false);
        deleteButtonRT.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        Closed?.Invoke();
    }

    public override void ItemClicked(ItemUI itemClicked)
    {
        if (selectedButton == itemClicked)
        {
            selectedButton = null;
            mergeButtonRT.gameObject.SetActive(false);
            deleteButtonRT.gameObject.SetActive(false);
        }
        else
        {
            selectedButton = itemClicked;
            Vector3 buttonPos = itemClicked.gameObject.GetComponent<RectTransform>().position;

            if (itemClicked.thisItem is SeedData)
            {
                SeedData seed = (SeedData)itemClicked.thisItem;
                if (seed.seedLvl < seed.plantClass.value.Length)
                {
                    mergeButtonRT.gameObject.SetActive(true);
                    mergeButtonRT.position = buttonPos + new Vector3(0, -104f, 0);
                }
                
                //deleteButtonRT.gameObject.SetActive(true);
                //deleteButtonRT.position = buttonPos + new Vector3(0, -164f, 0);
            }

            if (itemClicked.thisItem is VegetableData)
            {
                //mergeButtonRT.gameObject.SetActive(false);
                //deleteButtonRT.gameObject.SetActive(true);
                //deleteButtonRT.position = buttonPos + new Vector3(0, -85f, 0);
            }
        }
    }

    public void MergeButtonClicked()
    {
        if (selectedButton == null || selectedButton.thisItem is not SeedData)
            return;

        Item[] mergeItems = InventorySystem.instance.GetMergerSeeds((SeedData)selectedButton.thisItem);
        if (mergeItems.Length >= 3)
        {
            // merge
            for (int i = 0; i < 3; i++)
            {
                InventorySystem.instance.RemoveItemFromInventory(mergeItems[i]);
            }

            SeedData newSeed = (SeedData)selectedButton.thisItem;
            newSeed.seedLvl += 1;
            InventorySystem.instance.AddItemToInventory(newSeed);
            MergeComplete?.Invoke();
            mergeButtonRT.gameObject.SetActive(false);
            selectedButton = null;
            RefreshItems();
        }
        else
        {
            NotEnoughMgr.instance.ShowNotEnoughSeeds();
        }
    }

    public void DeleteButtonClicked()
    {
        if (selectedButton == null)
            return;

        mergeButtonRT.gameObject.SetActive(false);
        deleteButtonRT.gameObject.SetActive(false);

        InventorySystem.instance.RemoveItemFromInventory(selectedButton.thisItem);
        selectedButton = null;
        RefreshItems();
    }
}
