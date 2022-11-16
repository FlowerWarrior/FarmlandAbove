using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradesPoint : I_Interactable
{
    [SerializeField] internal PlantHabitat plantsHabitat;

    private void Awake()
    {
        myType = interactablePoint.Upgrades;
    }
}
