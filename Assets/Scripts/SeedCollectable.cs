using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedCollectable : I_Interactable
{
    [SerializeField] SeedData seed;
    [SerializeField] int tutorialIndex = 0;

    internal static System.Action<int> SeedPickedUp;

    private void Awake()
    {
        myType = interactablePoint.Seed;
    }

    public void PickUp()
    {
        InventorySystem.instance.AddItemToInventory(seed);
        SlotsInteractor.instance.SafeRemoveFromSlotsInRange(this);
        SeedPickedUp?.Invoke(tutorialIndex);
        Destroy(gameObject);
    }
}
