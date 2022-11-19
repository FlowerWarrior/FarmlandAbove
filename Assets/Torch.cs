using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : I_Interactable
{
    [SerializeField] internal int lifespan = 30;

    private void Awake()
    {
        myType = interactablePoint.Torch;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CountdownToEnd());
    }

    IEnumerator CountdownToEnd()
    {
        while (lifespan > 0)
        {
            yield return new WaitForSeconds(1);
            lifespan--;
        }
        SlotsInteractor.instance.SafeRemoveFromSlotsInRange(this);
        Destroy(gameObject);
    }
}
