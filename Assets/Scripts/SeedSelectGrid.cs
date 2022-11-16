using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedSelectGrid : I_ItemsGrid
{
    [SerializeField] GameObject itemPrefab;

    public void LoadItems(SeedData[] acceptableSeeds)
    {
        CodeSF.DestroyAllChildren(transform);

        items = InventorySystem.instance.GetAcceptableSeeds(acceptableSeeds);
        for (int i = 0; i < items.Length; i++)
        {
            GameObject newItem = Instantiate(itemPrefab, transform.position, Quaternion.identity, transform);
            newItem.GetComponent<ItemUI>().UpdateItem(items[i]);
        }

        int itemRows = Mathf.CeilToInt((float)items.Length / 6f);
        transform.parent.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 164 * itemRows);
    }

    public override void ItemClicked(ItemUI itemClicked)
    {
        UIMGR.instance.OpenPlantingBoostMiniGame((SeedData)itemClicked.thisItem);
    }
}
