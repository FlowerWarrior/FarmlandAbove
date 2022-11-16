using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FecesPoint : I_Interactable
{
    private void Awake()
    {
        myType = interactablePoint.Feces;
    }

    public void SafeDestroy()
    {
        SlotsInteractor.instance.SafeRemoveFromSlotsInRange(this);
        Destroy(gameObject);
    }
}
