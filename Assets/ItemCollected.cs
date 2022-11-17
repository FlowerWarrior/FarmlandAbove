using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemCollected : MonoBehaviour
{
    [SerializeField] Image itemImage;
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] TextMeshProUGUI itemLevel;
    internal int index = 0;

    public void UpdateUI(Item item)
    {
        itemImage.sprite = item.inventorySprite;
        itemName.text = item.itemName;
        itemLevel.text = $"lv.{item.seedLvl}";
    }

    private void Start()
    {
        StartCoroutine(DestroyAfter(1.25f));
    }

    private IEnumerator DestroyAfter(float sec)
    {
        yield return new WaitForSeconds(sec);
        GetComponentInParent<ItemCollectedEffectMgr>().activeItems.Remove(gameObject);
        Destroy(gameObject);
    }
}
