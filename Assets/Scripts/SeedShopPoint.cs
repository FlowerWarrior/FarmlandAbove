using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedShopPoint : I_Interactable
{
    [Header("Select seeds for this shop")]
    [SerializeField] internal int[] seedsCost;
    [SerializeField] internal SeedData[] seedsCommon;
    [SerializeField] internal SeedData[] seedsRare;
    [SerializeField] internal SeedData[] seedsEpic;
}
