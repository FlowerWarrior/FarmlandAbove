using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class GrowSlot : I_Interactable
{
    #region Variables

    [SerializeField] VegetableData currentVegetable = null;
    [SerializeField] internal SeedData currentSeed { get; private set; } = null;
    //[SerializeField] GameObject plantReadyCanvas;
    [SerializeField] ParticleSystem hitParticles;
    [SerializeField] WaterIndicator waterIndicator;
    [SerializeField] HPbar hpbar;
    [SerializeField] AirTargetsMgr airTargets;

    GameObject plantInstance;
    Animation plantGrowAnimation;

    internal float plantProgress = 0f;

    internal bool isAirTargetHit = false;
    internal bool areStatsUIShown = false;
    internal bool isAirBoosted = false;
    bool isTouchingAirArea = false;
    float airBoostFillSpeed = 0.32f;
    float airBoostUsageSpeed = 0.01f;

    internal float lightLevel = 0; // 0 <-> 1
    internal float airBoostLvl = 0; // 0 <-> 1
    internal bool isGoo = false;
    [SerializeField] GameObject gooMesh;

    [SerializeField] GameObject bushyGrass;

    bool isPerfectWatered = false;
    internal float plantWaterLevel = 0; // 0 <-> 100
    internal int hp = 100;
    internal float plantingBoost = 0; // in %
    float ticksReceived = 0;

    int islandIndex = 0;
    PlantHabitat myHabitat;

    internal PlantState currentState = PlantState.Empty;
    internal static System.Action<GrowSlot, Vector3, int> SeedPlanted;
    internal static System.Action<VegetableData, SeedData> PlantHarvested;
    internal static System.Action<GrowSlot, Vector3, int> PlantReady;
    internal static System.Action ReachedPerfectWater;
    internal static System.Action TookDamage;
    internal static System.Action CutGrass;
    internal static System.Action<Vector3> GrassSpawned;

    internal enum PlantState
    {
        Empty,
        Growing,
        ReadyForHarvest
    }

    #endregion

    #region Methods

    private void OnTick(int ticks)
    {
        if (currentState == PlantState.Growing)
        {
            IncrementPlantProgress(ticks);

            if (isGoo)
            {
                plantWaterLevel = 68.75f;
            }
            else
            {
                if (plantWaterLevel > 0f)
                {
                    float waterRetention = 1f;
                    if (myHabitat.boughtUpgrades[2] == true)
                    {
                        waterRetention = 2f;
                    }
                    plantWaterLevel -= (currentVegetable.waterThirst / waterRetention) / 30f;
                }
                else
                {
                    plantWaterLevel = 0f;
                }
            }

            lightLevel = DayNightMgr.instance.GetSunLightValue() + TorchesMgr.instance.GetLightLevel(transform.position, islandIndex);
            lightLevel = Mathf.Clamp(lightLevel, 0f, 1f);

            if (IsPerfectWatered())
            {
                waterIndicator.ToggleTooMuchWater(false);
                waterIndicator.ToggleNotEnoughWater(false);
            }
            else
            {
                waterIndicator.gameObject.SetActive(true);
                if (plantWaterLevel > 50f)
                {
                    waterIndicator.ToggleTooMuchWater(true);
                    waterIndicator.ToggleNotEnoughWater(false);
                }
                else
                {
                    waterIndicator.ToggleTooMuchWater(false);
                    waterIndicator.ToggleNotEnoughWater(true);
                }
            }
        }
    }

    public void PlantSeed(SeedData seed, int plantingBoostValue)
    {
        VegetableData veggie = (VegetableData)ScriptableObject.CreateInstance("VegetableData");

        veggie.itemName = seed.plantClass.itemName;
        veggie.inventorySprite = seed.plantClass.inventorySprite;
        veggie.prefab = seed.plantClass.prefab;
        veggie.rarity = seed.plantClass.rarity;
        veggie.seedClass = seed.plantClass.seedClass;
        veggie.value = seed.plantClass.value;
        veggie.waterThirst = seed.plantClass.waterThirst;
        veggie.fullyGrownTicksRequired = seed.plantClass.fullyGrownTicksRequired;

        veggie.seedLvl = seed.seedLvl;

        InstantiatePlant(veggie, seed);
        plantingBoost = plantingBoostValue;
        currentState = PlantState.Growing;

        SeedPlanted?.Invoke(this, transform.position, islandIndex);
    }

    private void SetPlantProgress(float value) // 0 is just planted, 1 is fully grown
    {
        string animName = plantGrowAnimation.clip.name;
        plantGrowAnimation[animName].speed = 0;
        plantGrowAnimation[animName].normalizedTime = value;
        plantProgress = value;

        if (value >= 1 && currentState == PlantState.Growing)
        {
            OnPlantReadyForHarvest();
        }
    }

    private void OnPlantReadyForHarvest()
    {
        currentState = PlantState.ReadyForHarvest;
        waterIndicator.ToggleReady(true);
        //Vector3 plantReadyImgPos = plantInstance.transform.GetChild(2).position;
        //Instantiate(plantReadyCanvas, plantReadyImgPos, Quaternion.identity, transform);
        PlantReady?.Invoke(this, transform.position, islandIndex);
    }

    public float GetPlantGrowthMultiplier()
    {
        float multiplier = 1;
        if (myHabitat.boughtUpgrades[1] == true)
        {
            multiplier *= 1.5f;
        }
        multiplier += (plantingBoost / 100f * multiplier);

        if (IsAirBoostActive())
            multiplier *= 3f;

        if (!IsPerfectWatered() || isBushyGrass || !IsAirBoostActive())
            multiplier *= 0.1f;

        multiplier *= lightLevel;

        return multiplier;
    }

    private void IncrementPlantProgress(int ticks)
    {
        ticksReceived += ((float)ticks) * GetPlantGrowthMultiplier();
        SetPlantProgress(((float)ticksReceived) / ((float)currentVegetable.fullyGrownTicksRequired));
    }

    public void InstantiatePlant(VegetableData vegetableToSpawn, SeedData seed)
    {
        currentVegetable = vegetableToSpawn;
        currentSeed = seed;
        currentState = PlantState.Growing;
        plantInstance = Instantiate(currentVegetable.prefab, transform.position, Quaternion.identity, transform);
        plantGrowAnimation = plantInstance.GetComponent<Animation>();
        plantGrowAnimation.Play();
        waterIndicator.gameObject.transform.position = plantInstance.transform.GetChild(2).position;
        hpbar.gameObject.transform.position = plantInstance.transform.GetChild(2).position;
        hp = 100;
        SetPlantProgress(0);
    }

    public void Harvest()
    {
        int worth = currentVegetable.value[currentVegetable.seedLvl-1];

        if (myHabitat.boughtUpgrades[0] == true)
        {
            worth += worth/2;
        }

        currentVegetable.value[currentVegetable.seedLvl-1] = worth;
        PlantHarvested?.Invoke(currentVegetable, currentSeed);

        ResetGrowSlot();
    }

    public bool IsPerfectWatered()
    {
        if (plantWaterLevel >= 31.25f && plantWaterLevel <= 68.75f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsAirBoostActive()
    {
        return (airBoostLvl > 0.4375f);
    }

    public void IncrementPlantWaterLevel(float amount)
    {
        plantWaterLevel += amount;
        plantWaterLevel = Mathf.Clamp(plantWaterLevel, 0f, 100f);

        if (IsPerfectWatered())
        {
            if (!isPerfectWatered)
            {
                ReachedPerfectWater?.Invoke();
                isPerfectWatered = true;
            }
        }
        else
        {
            isPerfectWatered = false;
        }
    }

    public void TakeDamageFromSlime()
    {
        if (myHabitat.IsForceShieldActive())
        {
            myHabitat.ShieldTakeDamage();
            return;
        }

        hp -= 4;
        hitParticles.Emit(8);
        TookDamage?.Invoke();
        hpbar.ShowAfterHit((float)hp / 100f);

        if (hp <= 0)
        {
            ResetGrowSlot();
        }
    }

    public void EnableGoo()
    {
        isGoo = true;
        gooMesh.SetActive(true);
    }

    private void ResetGrowSlot()
    {
        currentState = PlantState.Empty;
        waterIndicator.DisableAll();
        hpbar.HideHpBar();
        ticksReceived = 0;
        plantProgress = 0f;
        airBoostLvl = 0;
        isGoo = false;
        gooMesh.SetActive(false);
        bushyGrass.SetActive(false);

        // Destroy childs with exceptions
        for (int i = 6; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        currentVegetable = null;
        plantInstance = null;
    }

    public SeedData[] GetAcceptableSeeds()
    {
        return transform.parent.gameObject.GetComponent<PlantHabitat>().acceptableSeeds;
    } 

    public void EnabledGrowStatsUI()
    {
        areStatsUIShown = true;
    }

    public void DisabledGrowStatsUI()
    {
        areStatsUIShown = false;
    }

    private void ToggleBushyGrass(bool state)
    {
        isBushyGrass = state;
        bushyGrass.SetActive(state);
        if (state)
        {
            GrassSpawned?.Invoke(transform.position);
        }
        else
        {
            bushyGrassTimeDelay = Random.Range(30f, 200f);
        }
        waterIndicator.ToggleWarning(state);
    }

    public void CutBushyGrass()
    {
        ToggleBushyGrass(false);
        CutGrass?.Invoke();
    }

    #endregion

    #region MonoBehaviour Callbacks

    private void Awake()
    {
        myType = interactablePoint.GrowSlot;
        islandIndex = transform.parent.GetComponent<PlantHabitat>().islandIndex;
        myHabitat = GetComponentInParent<PlantHabitat>();
        if (!isGoo)
            gooMesh.SetActive(false);

        ToggleBushyGrass(false);
        bushyGrassTimeDelay = Random.Range(10f, 200f);

        if (currentVegetable != null)
        {
            VegetableData initnialVeggie = currentVegetable;
            ResetGrowSlot();
            PlantSeed(initnialVeggie.seedClass, 30);
        }
    }

    private void OnEnable()
    {
        TickSender.Tick += OnTick;
    }

    private void OnDisable()
    {
        TickSender.Tick -= OnTick;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.2f);
    }

    internal bool isBushyGrass = false;
    float bushyGrassTimeDelay, bushyGrassTimer = 0;
    private void Update()
    {
        if (isGoo)
        {
            airBoostLvl = 1f;
        }
        else
        {
            if (isAirTargetHit)
            {
                airBoostLvl += airBoostFillSpeed * Time.deltaTime;
            }
            else if (!isGoo)
            {
                float usageMultiplier = 1f;
                if (myHabitat.boughtUpgrades[4] == true)
                {
                    usageMultiplier = 0.5f;
                }
                airBoostLvl -= airBoostUsageSpeed * usageMultiplier * Time.deltaTime;
            }
        }

        airBoostLvl = Mathf.Clamp(airBoostLvl, 0, 1);

        if (currentState == PlantState.Growing && !isGoo)
        {
            bushyGrassTimer += Time.deltaTime;
            if (bushyGrassTimer >= bushyGrassTimeDelay)
            {
                bushyGrassTimer = 0;
                ToggleBushyGrass(true);
            }
        }
    }

    private void FixedUpdate() // Order of Exec 1
    {
        isTouchingAirArea = false;
        StartCoroutine(CheckAtFixedUpdateEnd());
    }

    private void OnTriggerStay(Collider other) // Order of Exec 2
    {
        if (other.gameObject.layer == 6) //airblow area layer
        {
            isTouchingAirArea = true;
        }
    }

    private IEnumerator CheckAtFixedUpdateEnd() // Order of Exec 3
    {
        yield return new WaitForFixedUpdate();
        isAirBoosted = isTouchingAirArea;
    }

    #endregion
}
