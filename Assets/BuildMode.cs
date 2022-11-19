using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMode : MonoBehaviour
{
    [SerializeField] LayerMask obstaclesLayer;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float rayDistance;
    [SerializeField] GameObject ghostTorchPrefab;
    [SerializeField] GameObject realTorchPrefab;
    [SerializeField] Material matAcceptable;
    [SerializeField] Material matWrong;

    Transform heldTorch = null;

    // Start is called before the first frame update
    void Start()
    {
        EnterBuildMode();
    }

    void EnterBuildMode()
    {
        heldTorch = Instantiate(ghostTorchPrefab).transform;
        heldTorch.gameObject.SetActive(false);
        UIMGR.instance.EnterBuildModeUI();
    }

    void ExitBuildMode()
    {
        if (heldTorch != null)
        {
            Destroy(heldTorch);
        }
        UIMGR.instance.ExitBuildModeUI();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit groundHit, hit;
        bool hitObstacle = false;
        bool hitGround = false;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out groundHit, rayDistance, groundLayer))
        {
            hitGround = true;
        }

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, rayDistance, obstaclesLayer))
        {
            hitObstacle = true;
        }

        if (hitGround)
        {
            heldTorch.gameObject.SetActive(true);
            heldTorch.transform.position = groundHit.point;
            // draw light range + add functionality

            if (hitObstacle)
            {
                heldTorch.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = matWrong;
            }
            else
            {
                heldTorch.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = matAcceptable;
            }
        }
        else
        {
            heldTorch.gameObject.SetActive(false);
        }
    }
}
