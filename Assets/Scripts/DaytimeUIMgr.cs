using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DaytimeUIMgr : MonoBehaviour
{
    [SerializeField] GameObject imgSun;
    [SerializeField] GameObject imgMoon;

    [SerializeField] Transform sunLight;
    [SerializeField] TextMeshProUGUI textTimeOfDay;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int hour = (int)DayNightMgr.instance.GetCurrentHour();
        if (hour < 12)
        {
            textTimeOfDay.text = $"{hour}:00 AM";
        }
        else if (hour >= 12)
        {
            textTimeOfDay.text = $"{hour-12}:00 PM";
            
        }

        if (hour >= 6 && hour < 18)
        {
            imgSun.SetActive(true);
            imgMoon.SetActive(false);
        }
        else
        {
            imgSun.SetActive(false);
            imgMoon.SetActive(true);
        }
    }
}
