using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickSender : MonoBehaviour
{
    [SerializeField] int ticksPerSecond = 24;
    [SerializeField] int ticksMultiplier = 1;

    internal static System.Action<int> Tick;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TickLoop());
    }

    IEnumerator TickLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f / ticksPerSecond);
            Tick?.Invoke(ticksMultiplier);
        }
    }
}
