using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeSign : I_Interactable
{
    [SerializeField] GameObject bridgeBuilt;
    [SerializeField] GameObject islandDestination;
    [SerializeField] internal int cost;

    internal bool isBuilt = false;
    internal static System.Action<Vector3> BridgeBuilt;

    private void Awake()
    {
        myType = interactablePoint.BridgeSign;
    }

    private void Start()
    {
        bridgeBuilt.SetActive(false);
        islandDestination.SetActive(false);
    }

    public void BuyBridge()
    {
        if (isBuilt)
            return;

        if (InventorySystem.instance.GetCoins() >= cost)
        {
            InventorySystem.instance.SubstractCoins(cost);
            bridgeBuilt.SetActive(true);
            islandDestination.SetActive(true);
            SlotsInteractor.instance.SafeRemoveFromSlotsInRange(this);
            isBuilt = this;
            gameObject.SetActive(false);

            BridgeBuilt?.Invoke(transform.position);
        }
        else
        {
            NotEnoughMgr.instance.ShowNotEnoughNotifAtCursor();
        }
    }
}
