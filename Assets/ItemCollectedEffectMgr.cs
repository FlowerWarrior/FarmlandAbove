using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollectedEffectMgr : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    internal List<GameObject> activeItems = new List<GameObject>();

    float spacing = 145f;
    int activeCount = 0;

    internal static ItemCollectedEffectMgr instance;
    private void Awake()
    {
        instance = this;
    }

    public void ShowItem(Item item)
    {
        if (activeItems.Count == 0)
        {
            activeCount = 0;
        }
        GameObject obj = Instantiate(prefab, transform);
        obj.GetComponent<ItemCollected>().UpdateUI(item);
        obj.GetComponent<RectTransform>().localPosition = Vector2.zero + Vector2.up * spacing * activeCount;
        activeItems.Add(obj);
        activeCount++;
    }
}
