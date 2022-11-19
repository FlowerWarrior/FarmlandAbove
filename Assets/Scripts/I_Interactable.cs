using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]

public abstract class I_Interactable : MonoBehaviour
{
    internal interactablePoint myType;

    internal virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            SlotsInteractor.instance.SafeAddToSlotsInRange(this);
        }
    }

    internal virtual void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            SlotsInteractor.instance.SafeRemoveFromSlotsInRange(this);
        }
    }
}
enum interactablePoint
{
    Shop,
    Sell,
    Upgrades,
    BridgeSign,
    GrowSlot,
    WaterRefill,
    Feces,
    Seed,
    Torch
}