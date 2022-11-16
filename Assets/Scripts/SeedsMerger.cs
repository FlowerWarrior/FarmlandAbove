using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedsMerger : MonoBehaviour
{
    internal SeedData selectedSeed;
    internal static System.Action Opened;

    private void OnEnable()
    {
        Opened?.Invoke();
    }
}
