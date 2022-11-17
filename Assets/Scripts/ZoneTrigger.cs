using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneTrigger : MonoBehaviour
{
    [SerializeField] internal int islandIndex;
    internal bool[] discoveredIslands = { true, false, false };

    internal static System.Action<int> DiscoveredIsland;
    internal static System.Action SlimesActivated;

    internal static ZoneTrigger instance;
    private void Awake() => instance = this;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Slime")
        {
            other.gameObject.GetComponent<Slime>().islandIndex = islandIndex;
        }

        if (other.tag == "Player")
        {
            PlayerRespawner.instance.currentIsland = islandIndex;
            if (discoveredIslands[islandIndex])
                return;

            switch (islandIndex)
            {
                case 1:
                    WelcomeTxt.instance.ShowTitle("Discovered -DESERT ISLAND-");
                    break;
                case 2:
                    StartCoroutine(ShowWinterUnlocked());
                    SlimesMgr.instance.slimeSpawningEnabled = true;
                    break;
            }
            discoveredIslands[islandIndex] = true;
            DiscoveredIsland?.Invoke(islandIndex);
        }
    }

    private IEnumerator ShowWinterUnlocked()
    {
        WelcomeTxt.instance.ShowTitle("Discovered -WINTER ISLAND-");
        yield return new WaitForSeconds(2);
        WelcomeTxt.instance.ShowTitle("Slimes will spawn on all islands every 10-30s!");
        SlimesActivated?.Invoke();
    }
}
