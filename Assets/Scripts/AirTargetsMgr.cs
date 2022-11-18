using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AirTargetsMgr : MonoBehaviour
{
    [SerializeField] GrowSlot myGrowSlot;
    [SerializeField] Transform target;
    bool isActive = false;
    bool isOxygenBarFilled = false;
    float targetChangeTime = 1.4f;
    float timer = 0;

    internal static System.Action FullyChargedO2;

    private void Update()
    {
        if (myGrowSlot.airBoostLvl >= 1f)
        {
            isOxygenBarFilled = true;
            FullyChargedO2?.Invoke();
        }
        else if (myGrowSlot.airBoostLvl < 0.74f)
        {
            isOxygenBarFilled = false;
        }

        if (myGrowSlot.areStatsUIShown && myGrowSlot.isAirBoosted && !isOxygenBarFilled)
        {
            if (myGrowSlot.currentState == GrowSlot.PlantState.Growing)
            {
                AirTargetUI.instance.target = target;
                myGrowSlot.isAirTargetHit = AirTargetUI.instance.isHittingTargetUI;
                timer += Time.deltaTime;
                if (timer >= targetChangeTime)
                {
                    MoveTargetRandomly();
                    timer = 0;
                }
                if (!isActive)
                {
                    isActive = true;
                }
            }
            else
            {
                isActive = false;
                myGrowSlot.isAirTargetHit = false;
                AirTargetUI.instance.target = null;
            }
        }
        else if (isActive)
        {
            isActive = false;
            myGrowSlot.isAirTargetHit = false;
            AirTargetUI.instance.target = null;
        }
    }

    private void MoveTargetRandomly()
    {
        Vector3 offset;
        offset.x = Random.Range(-0.2f, 0.2f);
        offset.y = Random.Range(-0.3f, 0.3f);
        offset.z = Random.Range(-0.2f, 0.2f);
        target.position = transform.position + offset;    
    }
}
