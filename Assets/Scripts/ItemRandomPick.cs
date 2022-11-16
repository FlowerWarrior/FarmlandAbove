using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemRandomPick : MonoBehaviour
{
    [SerializeField] Image img;
    [SerializeField] Image bgImg;
    internal SeedData seed;

    // Start is called before the first frame update
    void Start()
    {
        img.sprite = seed.inventorySprite;
        bgImg.color = CodeSF.GetRarityColor(seed.rarity);
    }
}
