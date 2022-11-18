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
    [SerializeField] RectTransform panelPickupSeed_RT;
    [SerializeField] RectTransform panelCutGrass_RT;
    internal RectTransform currentPanel = null;

    [SerializeField] Vector3 offset;

    List<I_Interactable> slotsInRange = new List<I_Interactable>();
    internal static SlotsInteractor instance;
    internal bool canInteract = true;
    I_Interactable closestSlot = null;

    public static System.Action<float, string, int, int, bool, int, bool, float, bool> UpdateGrowStats; // waterLevel, name, lvl
    internal static System.Action InteractMenu;
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

    private void UpdateInteractMenuUIPanels()
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
        panelPickupSeed_RT.gameObject.SetActive(false);
        panelCutGrass_RT.gameObject.SetActive(false);

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

    private void HandleInteractMenuions()
    {
        // Inputs
        bool InteractMenuAction = Input.GetButtonDown("Interact") && canInteract && IsCurrentPanelOnScreen();

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

                    if (growSlotInstance.isBushyGrass)
                    {
                        currentPanel = panelCutGrass_RT;
                        if (InteractMenuAction)
                        {
                            growSlotInstance.CutBushyGrass();
                        }
                        break;
                    }

                    switch (growSlotInstance.currentState)
                    {
                        case (GrowSlot.PlantState.Empty):
                            if (QuestMgr.instance.currentQuest > quest.PlantSeed && QuestMgr.instance.currentQuest < quest.PlantSeed2)
                            {
                                currentPanel = null;
                                break;
                            }     

                            if (InventorySystem.instance.GetGooCount() > 0 && !growSlotInstance.isGoo)
                            {
                                currentPanel = panelUseGoo_RT;
                                if (InteractMenuAction)
                                {
                                    // use goo
                                    growSlotInstance.EnableGoo();
                                    InventorySystem.instance.RemoveGooFromInv();
                                    InteractMenu?.Invoke();
                                }
                            }
                            else
                            {
                                currentPanel = panelPlantSeed_RT;
                                if (InteractMenuAction)
                                {
                                    UIMGR.instance.OpenSeedSelector(growSlotInstance.GetAcceptableSeeds());
                                    InteractMenu?.Invoke();
                                }
                            }
                            break;

                        case (GrowSlot.PlantState.Growing):
                            currentPanel = panelGrowingStats_RT;
                            UpdateGrowingStatsUI();
                            break;

                        case (GrowSlot.PlantState.ReadyForHarvest):
                            if (QuestMgr.instance.currentQuest < quest.CollectCrop)
                                break;

                            currentPanel = panelHarvest_RT;
                            if (InteractMenuAction)
                            {
                                growSlotInstance.Harvest();
                                InteractMenu?.Invoke();
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
                    if (InteractMenuAction)
                    {
                        UIMGR.instance.OpenSellMenu();
                        InteractMenu?.Invoke();
                    }
                    break;

                // Shop Point
                case interactablePoint.Shop:

                    currentPanel = panelShop_RT;
                    if (InteractMenuAction)
                    {
                        UIMGR.instance.OpenSeedShopMenu((SeedShopPoint)closestSlot);
                        InteractMenu?.Invoke();
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
                    if (InteractMenuAction)
                    {
                        BridgeSign bridgeSign = (BridgeSign)closestSlot;
                        bridgeSign.BuyBridge();
                        InteractMenu?.Invoke();
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
                    if (InteractMenuAction)
                    {
                        UpgradesPoint upgradesPoint = (UpgradesPoint)closestSlot;
                        UIMGR.instance.OpenUpgradesMenu(upgradesPoint.plantsHabitat);
                        InteractMenu?.Invoke();
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
                    if (InteractMenuAction)
                    {
                        RefillWater?.Invoke();
                    }
                    break;

                // Pickup Goo point
                case interactablePoint.Feces:
                    currentPanel = panelPickupGoo_RT;
                    if (InteractMenuAction)
                    {
                        // pickup goo
                        FecesPoint goo = (FecesPoint)closestSlot;
                        goo.SafeDestroy();
                        InventorySystem.instance.AddGooToInv();
                    }
                    break;

                // Pickup Goo point
                case interactablePoint.Seed:
                    currentPanel = panelPickupSeed_RT;
                    if (InteractMenuAction)
                    {
                        // pickup
                        SeedCollectable collectable = (SeedCollectable)closestSlot;
                        collectable.PickUp();
                    }
                    break;
            }
        }
        else
        {
            currentPanel = null;
        }

        UpdateInteractMenuUIPanels();
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
        HandleInteractMenuions();
    }

    #endregion
}
