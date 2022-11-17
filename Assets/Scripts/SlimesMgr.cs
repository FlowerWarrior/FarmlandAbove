using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SlimesMgr : MonoBehaviour
{
    [SerializeField] internal Transform[] spawnParents;

    List<List<GrowSlot>> targetGrowSlots = new List<List<GrowSlot>>();
    List<List<Slime>> activeSlimes = new List<List<Slime>>();
    [SerializeField] Slime[] initialWinterSlimes;

    [SerializeField] GameObject slimePrefab;
    [SerializeField] GameObject lightningBoltPrefab;
    [SerializeField] internal bool slimeSpawningEnabled = false;
    [SerializeField] float spawnHeight = 3f;
    [SerializeField] int maxSlimesPerIsland = 2;

    internal static SlimesMgr instance;
    internal static System.Action<int> SlimeTargetAvailable;
    internal static System.Action<Vector3> LightningHit;

    #region Methods

    public Transform GetRandomPointOnIsland(int islandIndex, Transform oldT)
    {
        Transform newT = spawnParents[islandIndex].GetChild(Random.Range(0, spawnParents[islandIndex].childCount));
        while (newT == oldT)
        {
            newT = spawnParents[islandIndex].GetChild(Random.Range(0, spawnParents[islandIndex].childCount));
        }
        return newT;
    }

    public GrowSlot GetTargetSlotAtIsland(int islandIndex)
    {
        int length = targetGrowSlots[islandIndex].Count;
        if (length == 0)
            return null;

        return targetGrowSlots[islandIndex][Random.Range(0, length)];
    }

    private void AddToTargets(GrowSlot slot, Vector3 pos, int islandIndex)
    {
        targetGrowSlots[islandIndex].Add(slot);
        SlimeTargetAvailable?.Invoke(islandIndex);
    }

    private void RemoveFromTargets(GrowSlot slot, Vector3 pos, int islandIndex)
    {
        targetGrowSlots[islandIndex].Remove(slot);
    }

    private IEnumerator SlimeSpawnRoutine()
    {
        yield return new WaitForSeconds(Random.Range(15, 38));
        //yield return new WaitForSeconds(Random.Range(11, 15));
        if (slimeSpawningEnabled)
        {
            // spawn on random available island
            System.Random rnd = new System.Random();
            int[] rndIndexs = new int[]{0,1,2}.OrderBy(x => rnd.Next()).ToArray();

            DeleteNullSlimes();
            for (int i = 0; i < spawnParents.Length; i++)
            {
                if (activeSlimes[rndIndexs[i]].Count < maxSlimesPerIsland)
                {
                    Vector3 pos = GetRandomPointOnIsland(rndIndexs[i], null).position;
                    GameObject newLightningBolt = Instantiate(lightningBoltPrefab, pos, Quaternion.identity);
                    LightningHit?.Invoke(pos);
                    yield return new WaitForSeconds(3);
                    Destroy(newLightningBolt);

                    Quaternion rot = Quaternion.Euler(new Vector3(0, Random.Range(0f, 360f), 0));
                    Slime newSlime = Instantiate(slimePrefab, pos + Vector3.up*spawnHeight, rot).GetComponent<Slime>();
                    newSlime.islandIndex = rndIndexs[i];
                    activeSlimes[rndIndexs[i]].Add(newSlime);
                    break;
                }
            }
        }
        StartCoroutine(SlimeSpawnRoutine());
    }

    private void DeleteNullSlimes()
    {
        for (int i = 0; i < activeSlimes.Count; i++)
        {
            for (int k = 0; k < activeSlimes[i].Count; k++)
            {
                if (activeSlimes[i][k] == null)
                {
                    activeSlimes[i].RemoveAt(k);
                }
            }
        }
    }

    #endregion

    #region MonoBehaviour Callbacks

    private void Awake()
    {
        instance = this;
        for (int i = 0; i < spawnParents.Length; i++)
        {
            targetGrowSlots.Add(new List<GrowSlot>());
            activeSlimes.Add(new List<Slime>());
        }
        for (int j = 0; j < initialWinterSlimes.Length; j++)
        {
            // 2 - winter island
            activeSlimes[2].Add(initialWinterSlimes[j]);
        }
    }

    private void Start()
    {
        StartCoroutine(SlimeSpawnRoutine());
    }

    private void OnEnable()
    {
        GrowSlot.SeedPlanted += AddToTargets;
        GrowSlot.PlantReady += RemoveFromTargets;
    }

    private void OnDisable()
    {
        GrowSlot.SeedPlanted -= AddToTargets;
        GrowSlot.PlantReady -= RemoveFromTargets;
    }

    private void OnDrawGizmos()
    {
        for(int i = 0; i < spawnParents.Length; i++)
        {
            for (int j = 0; j < spawnParents[i].childCount; j++)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawSphere(spawnParents[i].GetChild(j).position, 0.2f);
            }
        }
    }

    #endregion
}
