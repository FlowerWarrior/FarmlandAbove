using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterIndicator : MonoBehaviour
{
    [SerializeField] GameObject[] uiObjects;

    bool[] indicatorState = { false, false, false, false };

    public void ToggleTooMuchWater(bool state) { indicatorState[0] = state; UpdateIndicator(); }

    public void ToggleNotEnoughWater(bool state) { indicatorState[1] = state; UpdateIndicator(); }

    public void ToggleWarning(bool state) { indicatorState[2] = state; UpdateIndicator(); }

    public void ToggleReady(bool state) { indicatorState[3] = state; UpdateIndicator(); }

    public void DisableAll()
    {
        for (int i = 0; i < uiObjects.Length; i++)
        {
            indicatorState[i] = false;
        }
        UpdateIndicator();
    }

    private void HideAll()
    {
        for (int i = 0; i < uiObjects.Length; i++)
        {
            uiObjects[i].SetActive(false);
        }
    }

    public void UpdateIndicator()
    {
        HideAll();
        for (int i = uiObjects.Length-1; i >= 0; i--)
        {
            if (indicatorState[i])
            {
                uiObjects[i].SetActive(true);
                return;
            }
        }
    }
}
