using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotsInteractor : MonoBehaviour
{
    [SerializeField] RectTransform panelPlantSeed_RT;
    [SerializeField] RectTransform panelGrowingStats_RT;
    [SerializeField] RectTransform panelHarvest_RT;
    [SerializeField] RectTransform panelSell_RT;
    [SerializeField] RectTransform panelShop_RT;
    [SerializeField] RectTransform panelBridge_RT;
    [SerializeField] RectTransform panelUpgrades_RT;
    [SerializeField] RectTransform panelWaterRefill_RT;
    [SerializeField] RectTransform panelPickupGoo_RT;
    [SerializeField] RectTransform panelUseGoo_RT;
    internal RectTransform currentPanel = null;

    [SerializeField] Vector3 offset;

    List<I_Interactable> slotsInRange = new List<I_Interactable>();
    internal static SlotsInteractor instance;
    internal bool canInteract = true;
    I_Interactable closestSlot = null;

    public static System.Action<float, string, int, int, bool, int, bool, float, bool> UpdateGrowStats; // waterLevel, name, lvl
    internal static System.Action Interact;
    internal static System.Action RefillWater;

    #region Methods
    public void SafeAddToSlotsInRange(I_Interactable slot)
    {
        if (!slotsInRange.Contains(slot))
            slotsInRange.Add(slot);
    }

    public void SafeRemoveFromSlotsInRange(I_Interactable slot)
    {
        if (slotsInRange.Contains(slot))
        {
            if (slot.myType == interactablePoint.GrowSlot)
            {
                GrowSlot myGrowSlot = (GrowSlot)slot;
                myGrowSlot.DisabledGrowStatsUI();
            }
            slotsInRange.Remove(slot);
        }
            
    }

    private void SetClosestSlotInRange()
    {
        if (slotsInRange.Count == 0)
        {
            closestSlot = null;
            return;
        }

        Vector3 playerPos = PlayerRespawner.instance.playerInstance.transform.GetChild(0).position;
        I_Interactable newClosest = slotsInRange[0];

        for (int i = 0; i < slotsInRange.Count; i++)
        {
            if (Vector3.Distance(slotsInRange[i].transform.position, playerPos) < Vector3.Distance(newClosest.transform.position, playerPos))
            {
                newClosest = slotsInRange[i];
            }
        }

        if (closestSlot != newClosest)
        {
            if (closestSlot != null)
            {
                if (closestSlot.myType == interactablePoint.GrowSlot)
                {
                    GrowSlot myGrowSlot = (GrowSlot)closestSlot;
                    myGrowSlot.DisabledGrowStatsUI();
                }
            }

            closestSlot = newClosest;

            if (closestSlot.myType == interactablePoint.GrowSlot)
            {
                GrowSlot myGrowSlot = (GrowSlot)closestSlot;
                myGrowSlot.EnabledGrowStatsUI();
            }
        }
    }

    private void UpdateInteractUIPanels()
    {
        panelPlantSeed_RT.gameObject.SetActive(false);
        panelHarvest_RT.gameObject.SetActive(false);
        panelGrowingStats_RT.gameObject.SetActive(false);
        panelSell_RT.gameObject.SetActive(false);
        panelShop_RT.gameObject.SetActive(false);
        panelBridge_RT.gameObject.SetActive(false);
        panelUpgrades_RT.gameObject.SetActive(false);
        panelWaterRefill_RT.gameObject.SetActive(false);
        panelPickupGoo_RT.gameObject.SetActive(false);
        panelUseGoo_RT.gameObject.SetActive(false);

        if (currentPanel != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(closestSlot.transform.position + offset);

            if (screenPos.z < 0) // fix when obj behind camera
                return;

            currentPanel.gameObject.SetActive(true);
            currentPanel.position = screenPos;
        }
    }

    private void UpdateGrowingStatsUI()
    {
        if (closestSlot != null)
        {
            if (closestSlot.myType == interactablePoint.GrowSlot)
            {
                GrowSlot growSlotInstance = (GrowSlot)closestSlot;
                UpdateGrowStats?.Invoke(
                    growSlotInstance.plantWaterLevel, 
                    growSlotInstance.currentSeed.plantClass.itemName, 
                    growSlotInstance.currentSeed.seedLvl,
                    (int)(growSlotInstance.GetPlantGrowthMultiplier()*100f), 
                    growSlotInstance.IsPerfectWatered(),
                    (int)(growSlotInstance.plantProgress*100f),
                    growSlotInstance.IsAirBoostActive(),
                    growSlotInstance.airBoostLvl,
                    growSlotInstance.isGoo);
            }
        }
    }

    private bool IsCurrentPanelOnScreen()
    {
        if (currentPanel == null)
            return false;

        if (currentPanel.localPosition.x > -Screen.width / 2f && currentPanel.localPosition.x < Screen.width / 2f)
        {
            if (currentPanel.localPosition.y > -Screen.height / 2f && currentPanel.localPosition.y < Screen.height / 2f)
            {
                return true;
            }
        }

        return false;
    }

    private void HandleInteractions()
    {
        // Inputs
        bool interactAction = Input.GetButtonDown("Interact") && canInteract && IsCurrentPanelOnScreen();

        if (closestSlot != null)
        {
            switch (closestSlot.myType)
            {
                // Grow Slot Point
                case interactablePoint.GrowSlot:
                    if (QuestMgr.instance.currentQuest < quest.PlantSeed)
                    {
                        currentPanel = null;
                        break;
                    }

                    GrowSlot growSlotInstance = (GrowSlot)closestSlot;
                    switch (growSlotInstance.currentState)
                    {
                        case (GrowSlot.PlantState.Empty):
                            if (InventorySystem.instance.GetGooCount() > 0 && !growSlotInstance.isGoo)
                            {
                                currentPanel = panelUseGoo_RT;
                                if (interactAction)
                                {
                                    // use goo
                                    growSlotInstance.EnableGoo();
                                    InventorySystem.instance.RemoveGooFromInv();
                                    Interact?.Invoke();
                                }
                            }
                            else
                            {
                                currentPanel = panelPlantSeed_RT;
                                if (interactAction)
                                {
                                    UIMGR.instance.OpenSeedSelector(growSlotInstance.GetAcceptableSeeds());
                                    Interact?.Invoke();
                                }
                            }
                            break;

                        case (GrowSlot.PlantState.Growing):
                            currentPanel = panelGrowingStats_RT;
                            UpdateGrowingStatsUI();
                            break;

                        case (GrowSlot.PlantState.ReadyForHarvest):
                            currentPanel = panelHarvest_RT;
                            if (interactAction)
                            {
                                growSlotInstance.Harvest();
                                Interact?.Invoke();
                            }
                            break;
                    }
                    break;

                // Sell Point
                case interactablePoint.Sell:
                    if (QuestMgr.instance.currentQuest < quest.SellCrop)
                    {
                        currentPanel = null;
                        break;
                    } 

                    currentPanel = panelSell_RT;
                    if (interactAction)
                    {
                        UIMGR.instance.OpenSellMenu();
                        Interact?.Invoke();
                    }
                    break;

                // Shop Point
                case interactablePoint.Shop:

                    currentPanel = panelShop_RT;
                    if (interactAction)
                    {
                        UIMGR.instance.OpenSeedShopMenu((SeedShopPoint)closestSlot);
                        Interact?.Invoke();
                    }
                    break;

                // BridgeSign Point
                case interactablePoint.BridgeSign:
                    if (QuestMgr.instance.currentQuest < quest.None)
                    {
                        currentPanel = null;
                        break;
                    }

                    currentPanel = panelBridge_RT;
                    if (interactAction)
                    {
                        BridgeSign bridgeSign = (BridgeSign)closestSlot;
                        bridgeSign.BuyBridge();
                        Interact?.Invoke();
                    }
                    break;

                // Upgrades Point
                case interactablePoint.Upgrades:
                    if (QuestMgr.instance.currentQuest < quest.None)
                    {
                        currentPanel = null;
                        break;
                    }

                    currentPanel = panelUpgrades_RT;
                    if (interactAction)
                    {
                        UpgradesPoint upgradesPoint = (UpgradesPoint)closestSlot;
                        UIMGR.instance.OpenUpgradesMenu(upgradesPoint.plantsHabitat);
                        Interact?.Invoke();
                    }
                    break;

                // RefillWater Point
                case interactablePoint.WaterRefill:
                    if (QuestMgr.instance.currentQuest < quest.RefillWaterTool)
                    {
                        currentPanel = null;
                        break;
                    }

                    currentPanel = panelWaterRefill_RT;
                    if (interactAction)
                    {
                        RefillWater?.Invoke();
                    }
                    break;

                // Pickup Goo point
                case interactablePoint.Feces:
                    currentPanel = panelPickupGoo_RT;
                    if (interactAction)
                    {
                        // pickup goo
                        FecesPoint goo = (FecesPoint)closestSlot;
                        goo.SafeDestroy();
                        InventorySystem.instance.AddGooToInv();
                    }
                    break;
            }
        }
        else
        {
            currentPanel = null;
        }

        UpdateInteractUIPanels();
    }

    private void SendPlantRequestToSlot(SeedData seed, int plantingBoostValue)
    {
        if (closestSlot != null)
        {
            if (closestSlot.myType == interactablePoint.GrowSlot)
            {
                GrowSlot growSlotInstance = (GrowSlot)closestSlot;
                if (growSlotInstance.currentState == GrowSlot.PlantState.Empty)
                {
                    growSlotInstance.PlantSeed(seed, plantingBoostValue);
                }
            }
        }
    }

    private void IncrementPlantWaterLevel(float amount)
    {
        if (closestSlot == null)
            return;

        if (closestSlot.myType == interactablePoint.GrowSlot)
        {
            GrowSlot growSlotInstance = (GrowSlot)closestSlot;
            growSlotInstance.IncrementPlantWaterLevel(amount);
        }
    }

    #endregion

    #region MonoBehaviour Callbacks

    private void Awake() => instance = this;

    private void OnEnable()
    {
        PlantASeedMiniGame.PlantASeed += SendPlantRequestToSlot;
        ToolsUseManager.UsingWaterToolTick += IncrementPlantWaterLevel;
    }

    private void OnDisable()
    {
        PlantASeedMiniGame.PlantASeed -= SendPlantRequestToSlot;
        ToolsUseManager.UsingWaterToolTick -= IncrementPlantWaterLevel;
    }

    private void Update()
    {
        SetClosestSlotInRange();
        HandleInteractions();
    }

    #endregion
}
