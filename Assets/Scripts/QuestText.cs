using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestText : MonoBehaviour
{

    private void OnEnable()
    {
        QuestMgr.ShowQuest += SetQuestText;
        GetComponent<TextMeshProUGUI>().enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);
    }

    private void SetQuestText(quest currentQuest)
    {
        string newText = "";
        switch (currentQuest)
        {
            case quest.BuySeed0:
                newText = "Buy beetroot seeds (0/3)";
                break;
            case quest.BuySeed1:
                newText = "Buy beetroot seeds (1/3)";
                break;
            case quest.BuySeed2:
                newText = "Buy beetroot seeds (2/3)";
                break;
            case quest.AgainBuySeed0:
                newText = "Buy more beetroot seeds (0/3)";
                break;
            case quest.AgainBuySeed1:
                newText = "Buy more beetroot seeds (1/3)";
                break;
            case quest.AgainBuySeed2:
                newText = "Buy more beetroot seeds (2/3)";
                break;
            case quest.CloseShop:
                newText = "Exit the shop";
                break;
            case quest.OpenInventory:
                newText = "Press 'Q' to open inventory";
                break;
            case quest.ClickAnySeed:
                newText = "Click on any seed";
                break;
            case quest.Merge:
                newText = "MERGE seeds";
                break;
            case quest.CloseInventory:
                newText = "Close inventory";
                break;
            case quest.SellCrop:
                newText = "Sell crops at the air balloon";
                break;
            case quest.PlantSeed:
                newText = "Plant a seed";
                break;
            case quest.PickPlantBoost:
                newText = "Pick speed boost for plant";
                break;
            case quest.RefillWaterTool:
                newText = "Refill water tool at the well";
                break;
            case quest.Water:
                newText = "Water the plant (tool 1)";
                break;
            case quest.UseSprinkler:
                newText = "Use air blower (tool 3) and hit targets";
                break;
            case quest.PlantSeed2:
                newText = "Plant other seeds (0/2)";
                break;
            case quest.PlantSeed3:
                newText = "Plant other seeds (1/2)";
                break;
            case quest.CollectCrop:
                newText = "Harvest plants when ready (0/3)";
                break;
            case quest.CollectCrop2:
                newText = "Harvest plants when ready (1/3)";
                break;
            case quest.CollectCrop3:
                newText = "Harvest plants when ready (2/3)";
                break;
            case quest.CutDownTrees:
                newText = "Use axe (tool 2) to cut through trees & cactuses";
                break;
            case quest.PickupCactusSeed:
                newText = "Pickup Cactus Seed";
                break;
            case quest.None:
                GetComponent<TextMeshProUGUI>().enabled = false;
                transform.GetChild(0).gameObject.SetActive(false);
                return;
            case quest.QuestCompleted:
                newText = "QUEST COMPLETED!";
                GetComponent<TextMeshProUGUI>().text = newText;
                GetComponent<TextMeshProUGUI>().enabled = true;
                transform.GetChild(0).gameObject.SetActive(false);
                return;
        }
        GetComponent<TextMeshProUGUI>().text = newText;
        GetComponent<TextMeshProUGUI>().enabled = true;
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
