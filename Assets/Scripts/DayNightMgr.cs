using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightMgr : MonoBehaviour
{
    [SerializeField] int _ticksIn24H;
    [SerializeField] float rotationOffset;
    [SerializeField] internal int _currentTicks = 330;

    [SerializeField] Light sunLight;
    [SerializeField] Color32 _sunAboveColor, _sunsetColor, _nightColor;

    public static DayNightMgr instance;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        TickSender.Tick += RotateAboutTicks;
    }

    public float GetCurrentHour()
    {
        return ((float)_currentTicks / (float)_ticksIn24H) * 24f;
    }

    private void RotateAboutTicks(int ticks)
    {
        Vector3 newRot = Vector3.zero;

        // Day/Night cycle rotation
        _currentTicks += ticks;
        _currentTicks = (int)Mathf.Repeat(_currentTicks, _ticksIn24H);

        float cycleRotValue = (float)_currentTicks / (float)_ticksIn24H;
        newRot.x = rotationOffset + 360f * cycleRotValue;
        newRot.x = Mathf.Repeat(newRot.x, 360f);

        transform.rotation = Quaternion.Euler(newRot);
        
        // Day Colors
        if (newRot.x > 0 && newRot.x < 180)
        {
            if (newRot.x > 0 && newRot.x < 90) // Sunrise -> Noon
            {
                sunLight.color = Color.Lerp(_sunsetColor, _sunAboveColor, newRot.x/90f);
            }
            if (newRot.x > 90 && newRot.x < 180) // Noon -> Sunset
            {
                sunLight.color = Color.Lerp(_sunAboveColor, _sunsetColor, (newRot.x-90f)/90f);
            }
        }
        // Night Colors
        else
        {
            if (newRot.x > 180 && newRot.x < 270) // Sunrise -> Noon
            {
                sunLight.color = Color.Lerp(_sunsetColor, _nightColor, (newRot.x - 180f) / 90f);
            }
            if (newRot.x > 270 && newRot.x < 360) // Noon -> Sunset
            {
                sunLight.color = Color.Lerp(_nightColor, _sunsetColor, (newRot.x - 270f) / 90f);
            }
        }
        
        RenderSettings.ambientLight = sunLight.color;
    }
}
