using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemsFlyingEffectMgr : MonoBehaviour
{
    [SerializeField] GameObject gemPrefab;
    [SerializeField] int count = 7;
    [SerializeField] float timeSpacingMin = 0.08f;
    [SerializeField] float timeSpacingMax = 0.12f;

    private void OnEnable()
    {
        InventorySystem.ItemSold += SpawnEffect;
    }

    private void OnDisable()
    {
        InventorySystem.ItemSold -= SpawnEffect;
    }

    private void SpawnEffect()
    {
        GetComponent<RectTransform>().position = Input.mousePosition;
        StartCoroutine(SpawnEffectIEnum());
    }

    IEnumerator SpawnEffectIEnum()
    {
        for (int i = 0; i < count; i++)
        {
            Instantiate(gemPrefab, transform.parent);
            yield return new WaitForSeconds(Random.Range(timeSpacingMin, timeSpacingMax));
        }
    }
}
