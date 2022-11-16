using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMgr : MonoBehaviour
{
    [SerializeField] GameObject audioSourcePrefab;
    [SerializeField] GameObject audioSource3DPrefab;
    [Header("Audio Sounds")]

    [SerializeField] AudioSource fallLoopSource;
    float fallVolAcceleration = 0.1f;
    float fallVolume = 0f;

    [SerializeField] AudioSource waterLoopSource;
    internal bool isUsingWater = false;
    float waterLoopVolume = 0f;
    float waterVolumeChangeSpeed = 2f;

    [SerializeField] AudioSource airblowSource;
    internal bool isUsingAirBlow = false;
    float airblowVolume = 0f;
    float airblowVolAccel = 5f;

    [SerializeField] AudioClip[] footsteps;
    [SerializeField] AudioClip plantReady;
    [SerializeField] AudioClip inventoryOpened;
    [SerializeField] AudioClip inventoryClosed;
    [SerializeField] AudioClip seedPlanted;
    [SerializeField] AudioClip jump;
    [SerializeField] AudioClip buy;
    [SerializeField] AudioClip buildBridge;
    [SerializeField] AudioClip notEnough;
    [SerializeField] AudioClip[] axeHits;
    [SerializeField] AudioClip itemReceived;
    [SerializeField] AudioClip coinsAdded;
    [SerializeField] AudioClip interact;
    [SerializeField] AudioClip miniGameIndicatorStop;
    [SerializeField] AudioClip click;
    [SerializeField] AudioClip[] axeSwooshes;
    [SerializeField] AudioClip toolChanged;
    [SerializeField] AudioClip questCompleted;
    [SerializeField] AudioClip mergedSeeds;
    [SerializeField] AudioClip harvestPlant;
    [SerializeField] AudioClip slimeMove;
    [SerializeField] AudioClip slimeHitObstacle;
    [SerializeField] AudioClip closeUI;
    [SerializeField] AudioClip waterRefill;
    [SerializeField] AudioClip plantTakeDmg;
    [SerializeField] AudioClip slimesActivated;
    [SerializeField] AudioClip invItemClicked;
    [SerializeField] AudioClip airTargetCorrect;
    [SerializeField] AudioClip playerGroundHit;
    [SerializeField] AudioClip lightningSpawn;
    [SerializeField] AudioClip shieldDestroyed;

    float slimeHitDelay = 0.3f;
    float slimeHitTimer = 0f;

    bool soundsEnabled = true;

    internal static AudioMgr instance;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        // slime hit delay
        if (slimeHitTimer < slimeHitDelay)
        {
            slimeHitTimer += Time.deltaTime;
        }

        // wateer
        if (isUsingWater)
        {
            waterLoopVolume += waterVolumeChangeSpeed * Time.deltaTime;
        }
        else
        {
            waterLoopVolume -= waterVolumeChangeSpeed * Time.deltaTime;
        }
        waterLoopVolume = Mathf.Clamp(waterLoopVolume, 0f, 1f);

        waterLoopSource.volume = waterLoopVolume;

        // airblow
        if (isUsingAirBlow)
        {
            airblowVolume += airblowVolAccel * Time.deltaTime;
        }
        else
        {
            airblowVolume -= airblowVolAccel * Time.deltaTime;
        }
        airblowVolume = Mathf.Clamp(airblowVolume, 0f, 1f);

        airblowSource.volume = airblowVolume;

        // fall
        float yVel = PlayerRespawner.instance.GetPlayerRbVelocity().y;
        if (yVel < -7f)
        {
            fallVolume += fallVolAcceleration * (-yVel+7) * Time.deltaTime;
        }
        else
        {
            fallVolume = fallVolAcceleration * 13 * Time.deltaTime;
        }

        fallVolume = Mathf.Clamp(fallVolume, 0f, 1f);

        fallLoopSource.volume = fallVolume;
    }

    private void OnEnable()
    {
        Character.Footstep += PlayFootstep;
        GrowSlot.PlantReady += PlayPlantReady;
        InventoryUI.Opened += PlayInventoryOpened;
        InventoryUI.Closed += PlayInventoryClosed;
        GrowSlot.SeedPlanted += PlaySeedPlanted;
        PlayerJumpingState.Jumped += PlayJump;
        UpgradeButton.BoughtUpgrade += PlayBuy;
        SeedReceivedNew.Bought += PlayBuy;
        BridgeSign.BridgeBuilt += PlayBuildBridge;
        NotEnoughMgr.Notified += PlayNotEnough;
        ToolsUseManager.AxeHit += PlayAxeHit;
        SeedReceivedMgr.ItemShowReceived += PlayItemReceived;
        InventorySystem.CoinsAdded += PlayCoinsAdded;
        SlotsInteractor.Interact += PlayInteract;
        PlantASeedMiniGame.IndicatorStopped += PlayMiniGameIndicatorStopped;
        ButtonEffects.ButtonClicked += PlayClick;
        ToolsUseManager.AxeUsed += PlayAxeSwoosh;
        ToolsUseManager.ToolSelected += PlayToolChanged;
        QuestMgr.QuestCompleted += PlayQuestCompleted;
        InventoryUI.MergeComplete += PlayMergedSeeds;
        GrowSlot.PlantHarvested += PlayHarvestPlant;
        Slime.SlimeMoveSound += PlaySlimeMove;
        Slime.SlimeHitObstacle += PlaySlimeHitObstacle;
        UIMGR.PlayerClosedAllMenus += PlayClosedUI;
        UIMGR.PlayerClosedMerger += PlayClosedUI;
        SlotsInteractor.RefillWater += PlayWaterRefilled;
        GrowSlot.TookDamage += PlayPlantTookDamage;
        ZoneTrigger.SlimesActivated += PlaySlimesActivated;
        ItemUI.Clicked += PlayInvItemClicked;
        AirTargetUI.AitTargetCorrect += PlayAirTargetCorrect;
        PlayerGroundedState.HitGround += PlayPlayerHitGround;
        SlimesMgr.LightningHit += PlayLightningSpawnSlime;
        PlantHabitat.ShieldDestroyed += PlayShieldDestroyed;
    }

    private void PlayAudioAtPoint(AudioClip clip, Vector3 location)
    {
        if (!soundsEnabled)
            return;

        if (clip == null)
            return;

        AudioSource.PlayClipAtPoint(clip, location);
    }

    private void PlayAudioEffect(AudioClip clip)
    {
        if (!soundsEnabled)
            return;

        if (clip == null)
            return;

        GameObject obj = Instantiate(audioSourcePrefab);
        obj.GetComponent<AudioSource>().clip = clip;
        obj.GetComponent<audioSourceTemp>().MyStart();
    }

    private void Play3DAudioEffect(AudioClip clip)
    {
        if (!soundsEnabled)
            return;

        if (clip == null)
            return;

        GameObject obj = Instantiate(audioSource3DPrefab);
        obj.GetComponent<AudioSource>().clip = clip;
        obj.GetComponent<audioSourceTemp>().MyStart();
    }


    private void PlayFootstep(Vector3 location)
    {
        PlayAudioAtPoint(footsteps[Random.Range(0, footsteps.Length)], location);
    }

    private void PlayPlantReady(GrowSlot slot, Vector3 location, int islandIndex)
    {
        PlayAudioAtPoint(plantReady, location);
    }

    private void PlayInventoryOpened()
    {
        PlayAudioEffect(inventoryOpened);
    }

    private void PlayInventoryClosed()
    {
        PlayAudioEffect(inventoryClosed);
    }

    private void PlaySeedPlanted(GrowSlot slot, Vector3 location, int islandIndex)
    {
        PlayAudioAtPoint(seedPlanted, location);
    }

    private void PlayJump(Vector3 location)
    {
        PlayAudioAtPoint(jump, location);
    }

    private void PlayBuy()
    {
        PlayAudioEffect(buy);
    }

    private void PlayBuildBridge(Vector3 location)
    {
        PlayAudioAtPoint(buildBridge, location);
    }

    private void PlayNotEnough()
    {
        PlayAudioEffect(notEnough);
    }

    private void PlayAxeHit(Vector3 location)
    {
        PlayAudioAtPoint(axeHits[Random.Range(0, axeHits.Length)], location);
    }

    private void PlayItemReceived()
    {
        PlayAudioEffect(itemReceived);
    }

    private void PlayCoinsAdded()
    {
        PlayAudioEffect(coinsAdded);
    }

    private void PlayInteract()
    {
        PlayAudioEffect(interact);
    }

    private void PlayMiniGameIndicatorStopped()
    {
        PlayAudioEffect(miniGameIndicatorStop);
    }

    private void PlayClick()
    {
        PlayAudioEffect(click);
    }

    private void PlayAxeSwoosh()
    {
        PlayAudioEffect(axeSwooshes[Random.Range(0, axeSwooshes.Length)]);
    }

    private void PlayToolChanged(int toolIndex)
    {
        PlayAudioEffect(toolChanged);
    }

    private void PlayQuestCompleted()
    {
        PlayAudioEffect(questCompleted);
    }

    private void PlayMergedSeeds()
    {
        PlayAudioEffect(mergedSeeds);
    }       

    private void PlayHarvestPlant(VegetableData a, SeedData b)
    {
        PlayAudioEffect(harvestPlant);
    }

    private void PlaySlimeMove(Vector3 pos)
    {
        PlayAudioAtPoint(slimeMove, pos);
    }

    private void PlaySlimeHitObstacle(Vector3 pos)
    {
        if (slimeHitTimer >= slimeHitDelay)
        {
            PlayAudioAtPoint(slimeHitObstacle, pos);
            slimeHitTimer = 0;
        }   
    }

    private void PlayClosedUI()
    {
        PlayAudioEffect(closeUI);
    }

    private void PlayWaterRefilled()
    {
        PlayAudioEffect(waterRefill);
    }

    private void PlayPlantTookDamage()
    {
        PlayAudioEffect(plantTakeDmg);
    }

    private void PlaySlimesActivated()
    {
        PlayAudioEffect(slimesActivated);
    }

    private void PlayInvItemClicked()
    {
        PlayAudioEffect(invItemClicked);
    }

    private void PlayAirTargetCorrect()
    {
        PlayAudioEffect(airTargetCorrect);
    }

    private void PlayPlayerHitGround()
    {
        PlayAudioEffect(playerGroundHit);
    }
    
    private void PlayLightningSpawnSlime(Vector3 pos)
    {
        Play3DAudioEffect(lightningSpawn);
    }

    private void PlayShieldDestroyed(Vector3 pos)
    {
        PlayAudioAtPoint(shieldDestroyed, pos);
    }
}
