using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellPoint : I_Interactable
{
    private void Awake()
    {
        myType = interactablePoint.Sell;
    }
}