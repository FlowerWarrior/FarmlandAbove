using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefillPoint : I_Interactable
{
    private void Awake()
    {
        myType = interactablePoint.WaterRefill;
    }
}