using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeGrid : I_ItemsGrid
{
    [SerializeField] GameObject itemPrefab;
    [SerializeField] SeedsMerger seedsMerger;
    [SerializeField] TMPro.TextMeshProUGUI selectedCountText;

    List<ItemUI> selectedButtons = new List<ItemUI>();

    private void OnEnable()
    {
        selectedCountText.text = $"(0/3)";
        selectedButtons.Clear();
        LoadItems();
    }

    public void LoadItems()
    {
        CodeSF.DestroyAllChildren(transform);

        items = InventorySystem.instance.GetMergerSeeds(seedsMerger.selectedSeed);
        for (int i = 0; i < items.Length; i++)
        {
            GameObject newItem = Instantiate(itemPrefab, transform.position, Quaternion.identity, transform);
            newItem.GetComponent<ItemUI>().UpdateItem(items[i]);
        }

        int itemRows = Mathf.CeilToInt((float)items.Length / 3f);
        transform.parent.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 164 * itemRows);
    }

    public override void ItemClicked(ItemUI itemClicked)
    {
        if (selectedButtons.Contains(itemClicked))
        {
            selectedButtons.Remove(itemClicked);
            itemClicked.SetToDeselected();
        }
        else if (selectedButtons.Count < 3)
        {
            selectedButtons.Add(itemClicked);
            itemClicked.SetToSelected();
        }

        selectedCountText.text = $"({selectedButtons.Count}/3)";
    }
}
