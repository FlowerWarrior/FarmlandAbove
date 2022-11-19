using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestArrow : MonoBehaviour
{
    [SerializeField] float hideDistance;
    [SerializeField] float distance;
    [SerializeField] float height;
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] Transform t_Tree;
    [SerializeField] Transform t_Shop;
    [SerializeField] Transform[] t_GrowSlot;
    [SerializeField] Transform t_Sell;
    [SerializeField] Transform t_WaterWell;
    [SerializeField] Transform t_CactusSeed;
    internal Transform target = null;
    bool isVisible = true;

    private void OnEnable()
    {
        QuestMgr.ShowQuest += UpdateTarget;
        CameraController.AfterCameraLateUpdate += UpdateTransform;
    }

    private void OnDisable()
    {
        QuestMgr.ShowQuest -= UpdateTarget;
        CameraController.AfterCameraLateUpdate -= UpdateTransform;
    }

    private void UpdateTarget(quest currentQuest)
    {
        switch (currentQuest)
        {
            case quest.OpenInventory:
                target = null;
                break;
            case quest.CollectCrop:
                Transform readyGrowSlot = null;
                for (int i = 1; i < t_GrowSlot.Length; i++)
                {
                    if (t_GrowSlot[i].gameObject.GetComponent<GrowSlot>().currentState == GrowSlot.PlantState.ReadyForHarvest)
                    {
                        readyGrowSlot = t_GrowSlot[i];
                        break;
                    }
                }
                target = readyGrowSlot;
                break;
            case quest.BuySeed0:
                target = t_Shop;
                break;
            case quest.BuySeed1:
                target = t_Shop;
                break;
            case quest.BuySeed2:
                target = t_Shop;
                break;
            case quest.AgainBuySeed0:
                target = t_Shop;
                break;
            case quest.AgainBuySeed1:
                target = t_Shop;
                break;
            case quest.AgainBuySeed2:
                target = t_Shop;
                break;
            case quest.SellCrop:
                target = t_Sell;
                break;
            case quest.PlantSeed:
                Transform closestGrowSlot = t_GrowSlot[0];
                for (int i = 1; i < t_GrowSlot.Length; i++)
                {
                    Vector3 playerPos = PlayerRespawner.instance.rb.transform.position;
                    float oldDistance = Vector3.Distance(closestGrowSlot.position, playerPos);
                    float newDistance = Vector3.Distance(t_GrowSlot[i].position, playerPos);
                    if (newDistance < oldDistance)
                    {
                        closestGrowSlot = t_GrowSlot[i];
                    }
                }
                target = closestGrowSlot;
                break;
            case quest.RefillWaterTool:
                target = t_WaterWell;
                break;
            case quest.Water:
                Transform targetGrowSlot = null;
                for (int i = 0; i < t_GrowSlot.Length; i++)
                {
                    if (t_GrowSlot[i].gameObject.GetComponent<GrowSlot>().currentState == GrowSlot.PlantState.Growing)
                    {
                        targetGrowSlot = t_GrowSlot[i];
                        break;
                    }
                }
                target = targetGrowSlot;
                break;
            case quest.None:
                target = null;
                break;
            case quest.CutDownTrees:
                target = t_Tree;
                break;
            case quest.PickupCactusSeed:
                target = t_CactusSeed;
                break;
            case quest.QuestCompleted:
                target = null;
                break;
            case quest.OpenBuildMode:
                target = null;
                break;
            case quest.PlaceTorch:
                target = null;
                break;
            case quest.PlantSeed2:
                Transform emptySlot = null;
                for (int i = 1; i < t_GrowSlot.Length; i++)
                {
                    if (t_GrowSlot[i].gameObject.GetComponent<GrowSlot>().currentState == GrowSlot.PlantState.Empty)
                    {
                        emptySlot = t_GrowSlot[i];
                        break;
                    }
                }
                target = emptySlot;
                break;
            case quest.PlantSeed3:
                Transform emptySlot2 = null;
                for (int i = 1; i < t_GrowSlot.Length; i++)
                {
                    if (t_GrowSlot[i].gameObject.GetComponent<GrowSlot>().currentState == GrowSlot.PlantState.Empty)
                    {
                        emptySlot2 = t_GrowSlot[i];
                        break;
                    }
                }
                target = emptySlot2;
                break;
        }
    }

    void UpdateTransform()
    {
        if (target == null)
        {
            if (isVisible)
            {
                meshRenderer.enabled = false;
                isVisible = false;
            }

            return;
        }

        Vector3 lookTarget = target.position;
        lookTarget.y = transform.position.y;

        if (Vector3.Distance(lookTarget, transform.position) < hideDistance)
        {
            if (isVisible)
            {
                meshRenderer.enabled = false;
                isVisible = false;
            }
        }
        else
        {
            if (!isVisible)
            {
                meshRenderer.enabled = true;
                isVisible = true;
            }
        }
           

        Transform playerT = Camera.main.transform;
        Vector3 pos = playerT.position + distance * playerT.forward + new Vector3(0, height, 0);
        transform.position = pos;

        transform.LookAt(lookTarget);
    }
}
