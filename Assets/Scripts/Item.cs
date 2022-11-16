using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    public Rarity rarity;
    public Sprite inventorySprite;
    public string itemName;
    public int seedLvl = 1;
}
