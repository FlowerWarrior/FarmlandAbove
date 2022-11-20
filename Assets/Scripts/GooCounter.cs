using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooCounter : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] TMPro.TextMeshProUGUI countText;

    void OnEnable()
    {
        InventorySystem.GooUpdated += UpdateCount;
    }

    void OnDisable()
    {
        InventorySystem.GooUpdated -= UpdateCount;
    }

    void Start()
    {
        panel.SetActive(false);
    }

    void UpdateCount(int value)
    {
        panel.SetActive(true);
        countText.text = $"{value}";
    }
}
