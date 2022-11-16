using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class I_ItemsGrid : MonoBehaviour
{
    internal Item[] items;
    public abstract void ItemClicked(ItemUI item);
}
