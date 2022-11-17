using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollectedEffectMgr : MonoBehaviour
{
    [SerializeField] GameObject prefab;

    internal static ItemCollectedEffectMgr instance;
    private void Awake()
    {
        instance = this;
    }

    public void ShowItem(Item item)
    {
        GameObject obj = Instantiate(prefab, transform);
        obj.GetComponent<ItemCollected>().UpdateUI(item);
    }
}
