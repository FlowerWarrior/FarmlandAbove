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

    internal static System.Action EnteredFirstNight;
    [HideInInspector] internal bool didEnterFirstNight = false;
    bool didEnterThisNight = false;

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
        int hourOffset = 1;
        return (((float)_currentTicks / (float)_ticksIn24H) * 24f + hourOffset) % 24;
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
            if (didEnterThisNight)
                didEnterThisNight = false;
        }
        // Night Colors
        else
        {
            if (newRot.x > 180 && newRot.x < 270) // Sunrise -> Noon
            {
                sunLight.color = Color.Lerp(_sunsetColor, _nightColor, (newRot.x - 180f) / 90f);
                if (!didEnterFirstNight && newRot.x > 200)
                {
                    EnteredFirstNight?.Invoke();
                }
                if (!didEnterThisNight && newRot.x > 184)
                {
                    TorchesMgr.instance.ReshufflePlaceTargets();
                    didEnterThisNight = true;
                }
            }
            if (newRot.x > 270 && newRot.x < 360) // Noon -> Sunset
            {
                sunLight.color = Color.Lerp(_nightColor, _sunsetColor, (newRot.x - 270f) / 90f);
            }
        }
        
        RenderSettings.ambientLight = sunLight.color;
    }

    public float GetSunLightValue()
    {
        float x = (transform.rotation.eulerAngles.x % 360) / 360f * 20f;
        float y = 0;
        if (x >= 0 && x < 5)
        {
            y = ((3.3f / (-x - 0.6f) + 5.6f) + 4 )/ 2;
        }
        else if (x >= 5 && x < 10)
        {
            y = ((3.3f / (x - 10.6f) + 5.6f) + 4) / 2;
        }
        else if (x >= 10 && x < 15)
        {
            y = ((3.3f / (x - 9.13f) + -3.7f) + 4) / 2;
        }
        else if (x >= 15 && x <= 20)
        {
            y = ((2.1f / (-x + 20.6f) + -3.5f) + 4) / 2;
        }
        y /= 4.5f;
        return y;
    }
}
