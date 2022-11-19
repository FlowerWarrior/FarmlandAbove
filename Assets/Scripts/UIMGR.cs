using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMGR : MonoBehaviour
{
    [SerializeField] GameObject menuPlantSeeds;
    [SerializeField] SeedSelectGrid seedSelectGrid;
    [SerializeField] GameObject menuInventory;
    [SerializeField] GameObject menuSeedShop;
    [SerializeField] GameObject menuReceivedSeed;
    [SerializeField] GameObject menuSettings;
    [SerializeField] GameObject menuSell;
    [SerializeField] GameObject menuUpgrades;
    [SerializeField] GameObject menuPlantingBoost;
    [SerializeField] Slider waterLevelToolSlider;
    [SerializeField] Image waterLevelFillImg;
    [SerializeField] Image waterToolImg;
    [SerializeField] Image waterCrossImg;
    [SerializeField] GameObject[] hideInBuildMode;
    [SerializeField] GameObject[] showInBuildMode;

    internal bool isMenuOverlay = false;
    internal static UIMGR instance;
    internal static System.Action<int> UpdateCoinsUI;
    internal static System.Action PlayerClosedAllMenus;
    internal static System.Action PlayerClosedMerger;

    #region Methods

    public void UpdateWaterLevelToolSlider(float value)
    {
        waterLevelToolSlider.value = value;
        if (value > 0)
        {
            waterLevelFillImg.color = new Color32(95, 215, 255, 200);
            waterToolImg.color = new Color32(255, 255, 255, 255);
            waterCrossImg.gameObject.SetActive(false);
        }
        else
        {
            waterLevelFillImg.color = new Color32(255, 109, 95, 200);
            waterToolImg.color = new Color32(255, 255, 255, 220);
            waterCrossImg.gameObject.SetActive(true);
        }
    }

    public void OpenPlantingBoostMiniGame(SeedData seed)
    {
        menuPlantingBoost.GetComponent<PlantASeedMiniGame>().seed = seed;
        OpenMenu(menuPlantingBoost, true);
    }

    public void OpenSeedSelector(SeedData[] acceptableSeeds)
    {
        OpenMenu(menuPlantSeeds);
        seedSelectGrid.LoadItems(acceptableSeeds);
    }

    public void OpenReceivedSeedMenu(SeedData seed)
    {
        OpenMenu(menuReceivedSeed, false);
        menuReceivedSeed.GetComponent<SeedReceivedNew>().ShowSeed(seed);
    }

    public void OpenUpgradesMenu(PlantHabitat habitat)
    {
        menuUpgrades.GetComponent<UpgradesMenu>().targetHabitat = habitat;
        OpenMenu(menuUpgrades);
    }

    public void OpenSeedShopMenu(SeedShopPoint seedShopPoint)
    {
        //menuSeedShop.GetComponent<SeedShopMgr>().shopPoint = seedShopPoint;
        OpenMenu(menuSeedShop);
    }

    public void OpenInventoryMenu() => OpenMenu(menuInventory);

    public void CloseMergeMenu()
    {
        OpenInventoryMenu();
        PlayerClosedMerger?.Invoke();
    }

    public void OpenSettingsMenu() => OpenMenu(menuSettings);

    public void OpenSellMenu() => OpenMenu(menuSell);

    public void OnMenuClosed()
    {
        SlotsInteractor.instance.canInteract = true;
        isMenuOverlay = false;
    }

    public void CloseAllMenus()
    {
        menuPlantSeeds.SetActive(false);
        menuInventory.SetActive(false);
        menuSettings.SetActive(false);
        menuSeedShop.SetActive(false);
        menuSeedShop.SetActive(false);
        menuSell.SetActive(false);
        menuReceivedSeed.SetActive(false);
        menuUpgrades.SetActive(false);
        menuPlantingBoost.SetActive(false);
        PlayerRespawner.instance.EnablePlayerControls();
        LockCursor();
        OnMenuClosed();
    }

    public void PlayerCloseAllMenus()
    {
        CloseAllMenus();
        PlayerClosedAllMenus?.Invoke();
    }

    public void CloseReceivedMenu()
    {
        menuReceivedSeed.SetActive(false);
    }

    private void OpenMenu(GameObject menu, bool hideOtherWindows = true)
    {
        if (menu.activeInHierarchy)
            return;

        if (hideOtherWindows)
            CloseAllMenus();

        UnlockCursor();
        PlayerRespawner.instance.DisablePlayerControls();
        isMenuOverlay = true;

        menu.SetActive(true);
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SlotsInteractor.instance.canInteract = true;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SlotsInteractor.instance.canInteract = false;
    }

    private void UpdateCoinsStat(int deltaAmount)
    {
        UpdateCoinsUI?.Invoke(InventorySystem.instance.GetCoins());
    }

    public void EnterBuildModeUI()
    {
        for (int i = 0; i < hideInBuildMode.Length; i++)
        {
            hideInBuildMode[i].SetActive(false);
        }
        for (int i = 0; i < showInBuildMode.Length; i++)
        {
            showInBuildMode[i].SetActive(true);
        }
    }

    public void ExitBuildModeUI()
    {
        for (int i = 0; i < hideInBuildMode.Length; i++)
        {
            hideInBuildMode[i].SetActive(true);
        }
        for (int i = 0; i < showInBuildMode.Length; i++)
        {
            showInBuildMode[i].SetActive(false);
        }
    }

    #endregion

    #region MonoBehaviour Callbacks

    private void Awake() => instance = this;

    private void OnEnable()
    {
        InventorySystem.CoinsUpdated += UpdateCoinsStat;
    }

    private void OnDisable()
    {
        InventorySystem.CoinsUpdated -= UpdateCoinsStat;
    }

    private void Start()
    {
        LockCursor();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (isMenuOverlay)
                PlayerCloseAllMenus();
            else
                OpenSettingsMenu();
        }

        if (Input.GetButtonDown("OpenInventory") && !isMenuOverlay)
        {
            OpenInventoryMenu();
        }
    }

    #endregion
}
