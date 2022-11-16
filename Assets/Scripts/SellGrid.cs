using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellGrid : I_ItemsGrid
{
    [SerializeField] GameObject itemPrefab;

    List<Item> sellableItems = new List<Item>();

    public void RefreshItems()
    {
        CodeSF.DestroyAllChildren(transform);

        sellableItems.RemoveRange(0, sellableItems.Count);
        sellableItems.AddRange(InventorySystem.instance.GetSellableItems());

        for (int i = 0; i < sellableItems.Count; i++)
        {
            GameObject newItem = Instantiate(itemPrefab, transform.position, Quaternion.identity, transform);
            newItem.GetComponent<ItemUI>().UpdateItem(sellableItems[i]);
        }

        int itemRows = Mathf.CeilToInt((float)sellableItems.Count / 6f);
        transform.parent.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 164 * itemRows);
    }

    private void OnEnable()
    {
        RefreshItems();
    }

    public override void ItemClicked(ItemUI itemClicked)
    {
        InventorySystem.instance.SellItem(itemClicked.thisItem);
        RefreshItems();
    }

    public void SellAll()
    {
        InventorySystem.instance.SellAllItems();
        RefreshItems();
    }
}
