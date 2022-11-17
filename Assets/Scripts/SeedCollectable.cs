using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedCollectable : I_Interactable
{
    [SerializeField] SeedData seed;

    private void Awake()
    {
        myType = interactablePoint.Seed;
    }

    public void PickUp()
    {
        InventorySystem.instance.AddItemToInventory(seed);
        SlotsInteractor.instance.SafeRemoveFromSlotsInRange(this);
        Destroy(gameObject);
    }
}
