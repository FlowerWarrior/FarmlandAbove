using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "new Upgrade")]
public class UpgradeData : ScriptableObject
{
    public new string name;
    public string desc;
    public int cost;
    public int id;
    public bool isBought;
}
