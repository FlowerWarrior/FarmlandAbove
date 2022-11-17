using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeScript : MonoBehaviour
{
    [SerializeField] ToolsUseManager toolsMgr;
    [SerializeField] GameObject axeMesh1;
    [SerializeField] GameObject axeMesh2;
    [SerializeField] int upgradeAxeAfterTreesCut = 10;
    internal int axePower = 1;
    bool didUpgrade = false;

    private void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag == "Tree")
        {
            toolsMgr.OnHitTree(collider.gameObject, collider.ClosestPoint(GetComponent<SphereCollider>().transform.position));
        }
    }

    private void OnEnable()
    {
        UpdateMesh();
        InventorySystem.TreesCutStatUpdated += UpdateMesh;
    }   

    private void OnDisable()
    {
        InventorySystem.TreesCutStatUpdated -= UpdateMesh;
    }

    private void UpdateMesh()
    {
        if (InventorySystem.instance.GetTreesCut() >= upgradeAxeAfterTreesCut)
        {
            axeMesh1.SetActive(false);
            axeMesh2.SetActive(true);
            axePower = 100;
            if (InventorySystem.instance.GetTreesCut() == upgradeAxeAfterTreesCut && !didUpgrade)
            {
                WelcomeTxt.instance.ShowTitle("Axe upgraded");
                didUpgrade = true;
            }
        }
        else
        {
            axeMesh1.SetActive(true);
            axeMesh2.SetActive(false);
            axePower = 1;
        }
    }
}
