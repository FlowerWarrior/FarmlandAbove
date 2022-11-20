using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradesMenu : MonoBehaviour
{
    internal PlantHabitat targetHabitat;
    internal static System.Action OpenedUpgradesMenu;
    internal static System.Action ClosedUpgradesMenu;

    private void OnEnable()
    {
        OpenedUpgradesMenu?.Invoke();
    }

    private void OnDisable()
    {
        ClosedUpgradesMenu?.Invoke();
    }
}
