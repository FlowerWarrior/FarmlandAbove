using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantHabitat : MonoBehaviour
{
    [SerializeField] internal int islandIndex;
    [SerializeField] internal SeedData[] acceptableSeeds;
    [SerializeField] internal GameObject[] upgradeMeshes;

    internal bool[] boughtUpgrades = { false, false, false, false };

    bool isShieldActive = false;
    bool wasShieldBought = false;
    int shieldMaxHp = 15;
    [SerializeField] int shieldCurrentHp = 15;
    int shieldRegenTime = 30;

    internal static System.Action<Vector3> ShieldDestroyed;

    private void Start()
    {
        UpdateUpgradeMeshes();
    }

    public void UpdateUpgradeMeshes()
    {
        for (int i = 0; i < boughtUpgrades.Length; i++)
        {
            upgradeMeshes[i].SetActive(boughtUpgrades[i]);
            if (!wasShieldBought && boughtUpgrades[3])
            {
                wasShieldBought = true;
                isShieldActive = true;
            }
        }
    }

    public bool IsForceShieldActive()
    {
        return isShieldActive;
    }

    public void ShieldTakeDamage()
    {
        StopAllCoroutines();
        shieldCurrentHp--;
        shieldCurrentHp = Mathf.Clamp(shieldCurrentHp, 0, shieldMaxHp);
        if (shieldCurrentHp <= 0)
        {
            isShieldActive = false;
            upgradeMeshes[3].SetActive(false);
            ShieldDestroyed?.Invoke(transform.position);
            StartCoroutine(RegenerateShield());
        }
    }

    private IEnumerator RegenerateShield()
    {
        yield return new WaitForSeconds(shieldRegenTime);
        shieldCurrentHp = shieldMaxHp;
        isShieldActive = true;
        upgradeMeshes[3].SetActive(true);
    }
}
