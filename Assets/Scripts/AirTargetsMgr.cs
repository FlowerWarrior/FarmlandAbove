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

    private void Update()
    {
        if (myGrowSlot.airBoostLvl >= 1f)
        {
            isOxygenBarFilled = true;
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
                if (!isActive)
                {
                    isActive = true;
                    StartCoroutine(TargetsLoop());
                }
            }
            else
            {
                isActive = false;
                myGrowSlot.isAirTargetHit = false;
                AirTargetUI.instance.target = null;
                StopAllCoroutines();
            }
        }
        else if (isActive)
        {
            isActive = false;
            myGrowSlot.isAirTargetHit = false;
            AirTargetUI.instance.target = null;
            StopAllCoroutines();
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

    IEnumerator TargetsLoop()
    {
        while (true)
        {
            MoveTargetRandomly();
            yield return new WaitForSeconds(1.4f);
        }
    }
}
