using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterIndicator : MonoBehaviour
{
    [SerializeField] GameObject notEnoughWaterImg;
    [SerializeField] GameObject tooMuchWaterImg;

    public void SetTooMuchWater()
    {
        notEnoughWaterImg.SetActive(false);
        tooMuchWaterImg.SetActive(true);
    }

    public void SetNotEnoughWater()
    {
        notEnoughWaterImg.SetActive(true);
        tooMuchWaterImg.SetActive(false);
    }

    public void HideAll()
    {
        notEnoughWaterImg.SetActive(false);
        tooMuchWaterImg.SetActive(false);
    }
}
