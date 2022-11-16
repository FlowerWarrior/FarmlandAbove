using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] UpgradesMenu upgradesMenu;
    [SerializeField] UpgradeData upgrade;
    [Space(10)]
    [SerializeField] TextMeshProUGUI textName;
    [SerializeField] TextMeshProUGUI textDesc;
    [SerializeField] TextMeshProUGUI textCost;
    [SerializeField] Image buttonImg;
    [Space(10)]
    [SerializeField] Color32 unboughtBgColor;
    [SerializeField] Color32 boughtBgColor;

    internal static System.Action BoughtUpgrade;

    private void OnEnable()
    {
        RefreshThisButton();
    }

    public void OnClick()
    {
        if (upgradesMenu.targetHabitat.boughtUpgrades[upgrade.id] == true)
            return;

        if (InventorySystem.instance.GetCoins() >= upgrade.cost)
        {
            InventorySystem.instance.SubstractCoins(upgrade.cost);

            upgradesMenu.targetHabitat.boughtUpgrades[upgrade.id] = true;
            upgradesMenu.targetHabitat.UpdateUpgradeMeshes();

            BoughtUpgrade?.Invoke();

            RefreshThisButton();
        }
        else
        {
            NotEnoughMgr.instance.ShowNotEnoughNotifAtCursor();
        }
    }

    private void RefreshThisButton()
    {
        textName.text = upgrade.name;
        textDesc.text = upgrade.desc;
        textCost.text = upgrade.cost.ToString();

        if (upgradesMenu.targetHabitat.boughtUpgrades[upgrade.id] == true)
        {
            buttonImg.color = boughtBgColor;
            textCost.gameObject.SetActive(false);
        }
        else
        {
            buttonImg.color = unboughtBgColor;
        }
    }
}
