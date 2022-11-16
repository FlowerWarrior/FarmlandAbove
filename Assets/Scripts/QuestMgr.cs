using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestMgr : MonoBehaviour
{
    internal static System.Action<quest> ShowQuest;
    internal static System.Action QuestCompleted;

    internal static QuestMgr instance;
    internal quest currentQuest = quest.None;

    internal bool tutorialInProgress = true;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        WelcomeTxt.instance.ShowTitle("- TUTORIAL-");
        StartCoroutine(StartTutorialAfter(2));
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

        UIMGR.PlayerClosedAllMenus += () => { SetQuestCompleted(quest.CloseShop); SetQuestCompleted(quest.CloseInventory);
            if (currentQuest == quest.ClickAnySeed
            || currentQuest == quest.Merge) { OverrideCurrentQuest(quest.OpenInventory); } };

        UIMGR.PlayerClosedMerger += () => { if (currentQuest == quest.Merge) 
                                            OverrideCurrentQuest(quest.ClickAnySeed); };

        InventoryUI.Opened += () => { SetQuestCompleted(quest.OpenInventory); };

        ItemUI.Clicked += () => { if (currentQuest == quest.ClickAnySeed) SetQuestCompleted(quest.ClickAnySeed);
            else if (currentQuest == quest.Merge) OverrideCurrentQuest(quest.ClickAnySeed); };

        InventoryUI.MergeComplete += () => { SetQuestCompleted(quest.Merge); };

        PlantASeedMiniGame.seedMiniGameOpened += () => { SetQuestCompleted(quest.PlantSeed); };

        PlantASeedMiniGame.PlantASeed += (SeedData a, int b) => { SetQuestCompleted(quest.PickPlantBoost); };

        SlotsInteractor.RefillWater += () => { SetQuestCompleted(quest.RefillWaterTool); };

        GrowSlot.ReachedPerfectWater += () => { SetQuestCompleted(quest.Water); };

        GrowSlot.PlantReady += (GrowSlot a, Vector3 b, int c) => { SetQuestCompleted(quest.UseSprinkler); };

        GrowSlot.PlantHarvested += (VegetableData a, SeedData b) => { SetQuestCompleted(quest.CollectCrop); };
        
        InventorySystem.ItemSold += () => { SetQuestCompleted(quest.SellCrop); };

        ZoneTrigger.DiscoveredIsland += (int id) => { if (id == 1) { OverrideCurrentQuest(quest.CutDownTrees); tutorialInProgress = true; }  };

        Tree.TreeDestroyed += () => { SetQuestCompleted(quest.CutDownTrees); };
    }

    private void IncrementBuySeed()
    {
        if (currentQuest <= quest.BuySeed2)
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

        if (currentQuest == quest.SellCrop && completedQuest == quest.SellCrop)
        {
            OverrideCurrentQuest(quest.None);
            UIMGR.instance.CloseAllMenus();
            WelcomeTxt.instance.ShowTitle("- TUTORIAL COMPLETED-");
            tutorialInProgress = false;
        }
        else if (currentQuest == completedQuest && completedQuest == quest.CutDownTrees)
        { 
            OverrideCurrentQuest(quest.None);
            tutorialInProgress = false;
            WelcomeTxt.instance.ShowTitle("- TUTORIAL 2 COMPLETED-");
        }
        else if (currentQuest == completedQuest)
        {
            currentQuest++;
            ShowQuest?.Invoke(currentQuest);
        }

        if (completedQuest == quest.CloseShop ||
            completedQuest == quest.CloseInventory ||
            completedQuest == quest.Water ||
            completedQuest == quest.SellCrop)
        {
            QuestCompleted?.Invoke();
            ShowQuest?.Invoke(quest.QuestCompleted);
            yield return new WaitForSeconds(1.4f);
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
    OpenInventory,
    ClickAnySeed,
    Merge,
    CloseInventory,
    PlantSeed,
    PickPlantBoost,
    RefillWaterTool,
    Water,
    UseSprinkler,
    CollectCrop,
    SellCrop,
    None,
    CutDownTrees,
    QuestCompleted,
}