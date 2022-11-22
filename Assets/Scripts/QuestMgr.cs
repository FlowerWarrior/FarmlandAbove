using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestMgr : MonoBehaviour
{
    [SerializeField] bool showTutorial = true;

    internal static System.Action<quest> ShowQuest;
    internal static System.Action QuestCompleted;

    internal static QuestMgr instance;
    internal quest currentQuest = quest.None;

    internal bool tutorialInProgress = true;
    int currentTool = 0;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (showTutorial)
        {
            WelcomeTxt.instance.ShowTitle("- TUTORIAL-");
            StartCoroutine(StartTutorialAfter(2));
        }
        else
        {
            OverrideCurrentQuest(quest.None);
        }
    }

    IEnumerator StartTutorialAfter(float sec)
    {
        yield return new WaitForSeconds(sec);
        currentQuest = quest.BuySeed0;
        ShowQuest?.Invoke(currentQuest);
    }

    private void OnEnable()
    {
        SeedReceivedNew.Bought += () => { IncrementBuySeed(); };

        UIMGR.PlayerClosedAllMenus += () => { SetQuestCompleted(quest.CloseShop); SetQuestCompleted(quest.CloseShop2); SetQuestCompleted(quest.CloseInventory);
            if (currentQuest == quest.ClickAnySeed
            || currentQuest == quest.Merge) { OverrideCurrentQuest(quest.OpenInventory); } };

        UIMGR.PlayerClosedMerger += () => { if (currentQuest == quest.Merge) 
                                            OverrideCurrentQuest(quest.ClickAnySeed); };

        InventoryUI.Opened += () => { SetQuestCompleted(quest.OpenInventory); };

        ItemUI.Clicked += () => { if (currentQuest == quest.ClickAnySeed) SetQuestCompleted(quest.ClickAnySeed);
            else if (currentQuest == quest.Merge) OverrideCurrentQuest(quest.ClickAnySeed); };

        InventoryUI.MergeComplete += () => { SetQuestCompleted(quest.Merge); };

        PlantASeedMiniGame.seedMiniGameOpened += () => { SetQuestCompleted(quest.PlantSeed); };

        GrowSlot.SeedPlanted += (GrowSlot a, Vector3 b, int c) => { SetQuestCompleted(quest.PickPlantBoost); SetQuestCompleted(quest.PlantSeed3); SetQuestCompleted(quest.PlantSeed2); };

        SlotsInteractor.RefillWater += () => { SetQuestCompleted(quest.RefillWaterTool); };

        GrowSlot.ReachedPerfectWater += () => { SetQuestCompleted(quest.Water); };

        AirTargetsMgr.FullyChargedO2 += () => { SetQuestCompleted(quest.UseSprinkler); };

        GrowSlot.PlantReady += (GrowSlot a, Vector3 b, int c) => { SetQuestCompleted(quest.UseSprinkler); };

        GrowSlot.PlantHarvested += (VegetableData a, SeedData b) => { SetQuestCompleted(quest.CollectCrop3); SetQuestCompleted(quest.CollectCrop2); SetQuestCompleted(quest.CollectCrop); };
        
        InventorySystem.ItemSold += () => { SetQuestCompleted(quest.SellCrop); };

        ZoneTrigger.DiscoveredIsland += (int id) => { if (id == 1) { 
                if (currentTool == 1)
                {
                    OverrideCurrentQuest(quest.CutDownTrees); tutorialInProgress = true;
                }
                else
                {
                    OverrideCurrentQuest(quest.SelectAxeTool); tutorialInProgress = true;
                }
            }
            if (id == 2) { OverrideCurrentQuest(quest.SelectBlowTool); tutorialInProgress = true; } };

        DayNightMgr.EnteredFirstNight += () => { if (currentQuest == quest.None) { OverrideCurrentQuest(quest.OpenBuildMode); tutorialInProgress = true; } };

        Tree.TreeDestroyed += () => { SetQuestCompleted(quest.CutDownTrees); };

        UpgradesMenu.OpenedUpgradesMenu += () => { SetQuestCompleted(quest.GoToUpgradesPoint); };
        UpgradesMenu.ClosedUpgradesMenu += () => { if (currentQuest == quest.BuyTutorialUpgrade) OverrideCurrentQuest(quest.GoToUpgradesPoint); };

        UpgradeButton.BoughtUpgrade += () => { SetQuestCompleted(quest.BuyTutorialUpgrade); };

        SeedCollectable.SeedPickedUp += (int id) => { if (id == 0) SetQuestCompleted(quest.PickupCactusSeed); };

        ToolsUseManager.ToolSelected += (int a) => {
            currentTool = a;
            if (currentQuest == quest.SelectWaterTool && a == 0)
                SetQuestCompleted(quest.SelectWaterTool);
            else if (currentQuest == quest.Water && a != 0)
                OverrideCurrentQuest(quest.SelectWaterTool);

            if (currentQuest == quest.SelectAxeTool && a == 1)
                SetQuestCompleted(quest.SelectAxeTool);
            else if (currentQuest == quest.CutDownTrees && a != 1)
                OverrideCurrentQuest(quest.SelectAxeTool);

            if (currentQuest == quest.SelectBlowTool && a == 2)
                SetQuestCompleted(quest.SelectBlowTool);
            else if (currentQuest == quest.BlowOffSlimes && a != 2)
                OverrideCurrentQuest(quest.SelectBlowTool);

            if (currentQuest == quest.SelectSprinkler && a == 2)
                SetQuestCompleted(quest.SelectSprinkler);
            else if (currentQuest == quest.UseSprinkler && a != 2)
                OverrideCurrentQuest(quest.SelectSprinkler);
        };

        Slime.SlimeFellOffToVoid += () => { SetQuestCompleted(quest.BlowOffSlimes); };
    }

    private void IncrementBuySeed()
    {
        if (currentQuest <= quest.BuySeed2)
        {
            currentQuest++;
            ShowQuest?.Invoke(currentQuest);
        }

        if (currentQuest >= quest.AgainBuySeed0 && currentQuest <= quest.AgainBuySeed2)
        {
            currentQuest++;
            ShowQuest?.Invoke(currentQuest);
        }
    }

    private void OverrideCurrentQuest(quest newQuest)
    {
        currentQuest = newQuest;
        ShowQuest?.Invoke(currentQuest);
    }

    private void SetQuestCompleted(quest completedQuest)
    {
        if (completedQuest != currentQuest) return;
        StartCoroutine(ShowCompletedAndStartNext(completedQuest));
    }

    IEnumerator ShowCompletedAndStartNext(quest completedQuest)
    {
        if (!tutorialInProgress) yield break;

        else if (currentQuest == completedQuest && completedQuest == quest.PickupCactusSeed)
        { 
            OverrideCurrentQuest(quest.None);
            tutorialInProgress = false;
            //WelcomeTxt.instance.ShowTitle("- TUTORIAL 2 COMPLETED-");
        }
        else if (currentQuest == completedQuest)
        {
            if (completedQuest == quest.RefillWaterTool && currentTool == 0)
            {
                OverrideCurrentQuest(quest.Water);
                yield break;
            }

            currentQuest++;
            ShowQuest?.Invoke(currentQuest);
        }

        if (completedQuest == quest.CloseShop ||
            completedQuest == quest.BuyTutorialUpgrade ||
            completedQuest == quest.SellCrop ||
            completedQuest == quest.PlaceTorch)
        {
            QuestCompleted?.Invoke();
            ShowQuest?.Invoke(quest.QuestCompleted);
            yield return new WaitForSeconds(1.2f);
            if (completedQuest == quest.BuyTutorialUpgrade)
            {
                tutorialInProgress = false;
                UIMGR.instance.CloseAllMenus();
            }
            ShowQuest?.Invoke(currentQuest);
        }
    }
}

public enum quest
{
    BuySeed0,
    BuySeed1,
    BuySeed2,
    CloseShop,
    PlantSeed,
    PickPlantBoost,
    RefillWaterTool,
    SelectWaterTool,
    Water,
    SelectSprinkler,
    UseSprinkler,
    PlantSeed2,
    PlantSeed3,
    CollectCrop,
    CollectCrop2,
    CollectCrop3,
    SellCrop,
    AgainBuySeed0,
    AgainBuySeed1,
    AgainBuySeed2,
    CloseShop2,
    OpenInventory,
    ClickAnySeed,
    Merge,
    CloseInventory,
    GoToUpgradesPoint,
    BuyTutorialUpgrade,
    None,
    OpenBuildMode,
    PlaceTorch,
    SelectAxeTool,
    CutDownTrees,
    PickupCactusSeed,
    QuestCompleted,
    SelectBlowTool,
    BlowOffSlimes,
    PickupGoo,
}