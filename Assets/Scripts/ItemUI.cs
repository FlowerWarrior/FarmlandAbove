using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemUI: MonoBehaviour
{
    [SerializeField] Image itemSprite;
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] TextMeshProUGUI itemValue;
    [SerializeField] TextMeshProUGUI itemLevel;
    [SerializeField] GameObject selectedIndicator;
    [SerializeField] Image bgImage;

    internal Item thisItem = null;
    internal static System.Action Clicked;

    // Start is called before the first frame update
    void OnEnable()
    {
        
    }

    public void UpdateItem(Item newItem)
    {
        thisItem = newItem;

        itemSprite.enabled = true;
        itemSprite.sprite = thisItem.inventorySprite;

        itemName.enabled = true;
        itemName.text = thisItem.itemName;

        bgImage.color = CodeSF.GetRarityColor(thisItem.rarity);
        itemLevel.text = $"lv.{newItem.seedLvl}";

        if (newItem is VegetableData)
        {
            VegetableData thisVeggie = (VegetableData) newItem;
            itemValue.text = $"{thisVeggie.value[newItem.seedLvl-1]}$";

            if (newItem.seedLvl == thisVeggie.value.Length)
                itemLevel.text = $"MAX";
        }
        if (newItem is SeedData)
        {
            SeedData seed = (SeedData)newItem;
            itemValue.text = $"{seed.plantClass.value[newItem.seedLvl-1]}$";

            if (newItem.seedLvl == seed.plantClass.value.Length)
                itemLevel.text = $"MAX";
        }

        itemLevel.enabled = true;
    }

    public void OnClicked()
    {
        transform.parent.gameObject.GetComponent<I_ItemsGrid>().ItemClicked(this);
        Clicked?.Invoke();
    }

    public void SetToSelected()
    {
        selectedIndicator.SetActive(true);
    }

    public void SetToDeselected()
    {
        selectedIndicator.SetActive(false);
    }
}
