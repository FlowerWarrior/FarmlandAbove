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
                newText = "Plant the seed";
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
            case quest.CollectCrop:
                newText = "Harvest the plant";
                break;
            case quest.CutDownTrees:
                newText = "Use axe (tool 2) to cut through trees & cactuses";
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
