using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatsUI_Updater : MonoBehaviour
{
    [SerializeField] Slider waterSlider;
    [SerializeField] Slider airboostSlider;
    [SerializeField] Slider lightSlider;
    [SerializeField] Image lightImage;
    [SerializeField] TextMeshProUGUI textName;
    [SerializeField] TextMeshProUGUI textBoost;
    [SerializeField] Image waterImg;
    [SerializeField] Image waterSliderImg;
    [SerializeField] Color32 colPerfectWateredSlider;
    [SerializeField] Color32 colUnperfectWateredSlider;
    [SerializeField] Color32 colPerfectWateredImg;
    [SerializeField] Color32 colUnperfectWateredImg;
    [SerializeField] TextMeshProUGUI waterDesc;
    [SerializeField] GameObject gooTitle;

    private void OnEnable()
    {
        SlotsInteractor.UpdateGrowStats += UpdateStats;
    }

    private void OnDisable()
    {
        SlotsInteractor.UpdateGrowStats -= UpdateStats;
    }

    private void UpdateStats(float waterLevel, string name, int lvl, int growthSpeedBoost, bool isPerfectWatered, int progress, bool isAirBoosted, float airBoostLvl, bool isGoo, float lightLevel)
    {
        waterSlider.value = waterLevel / 100f;
        textName.text = $"{name}(lv{lvl}) {progress}%";

        if (isGoo)
        {
            gooTitle.SetActive(true);
        }
        else
        {
            gooTitle.SetActive(false);
        }

        if (growthSpeedBoost - 100 >= 0)
        {
            textBoost.text = $"+{growthSpeedBoost - 100}% speed";
        }
        else
        {
            textBoost.text = $"{growthSpeedBoost - 100}% speed";
        }

        lightSlider.value = lightLevel;

        airboostSlider.value = airBoostLvl;
        if (isAirBoosted)
        {
            textBoost.color = new Color32(202, 255, 0, 255);
        }
        else
        {
            textBoost.color = new Color32(255, 255, 255, 255);
        }

        if (isPerfectWatered)
        {
            waterImg.color = colPerfectWateredImg;
            waterSliderImg.color = colPerfectWateredSlider;
            waterDesc.gameObject.SetActive(false);
        }
        else
        {
            waterImg.color = colUnperfectWateredImg;
            waterSliderImg.color = colUnperfectWateredSlider;
            waterDesc.gameObject.SetActive(true);
            if (waterLevel > 50)
            {
                waterDesc.text = "(too much water)";
            }
            else
            {
                waterDesc.text = "(too little water)";
            }
        }
    }
}
