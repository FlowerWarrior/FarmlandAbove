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
    [SerializeField] Transform t_GrowSlot;
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
                target = null; //t_GrowSlot; //maybe change to dynamic
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
                target = t_GrowSlot;
                break;
            case quest.RefillWaterTool:
                target = t_WaterWell;
                break;
            case quest.Water:
                target = null;
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
