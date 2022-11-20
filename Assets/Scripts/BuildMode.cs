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
    [SerializeField] LayerMask torchLayer;

    Transform heldTorch = null;

    internal static System.Action PlacedTorch;
    internal static System.Action CantPlace;

    void EnterBuildMode()
    {
        heldTorch = Instantiate(ghostTorchPrefab, TorchesMgr.instance._ghostHolder).transform;
        heldTorch.gameObject.SetActive(false);
        UIMGR.instance.EnterBuildModeUI();
    }

    void ExitBuildMode()
    {
        if (heldTorch != null)
        {
            Destroy(heldTorch.gameObject);
        }
        UIMGR.instance.ExitBuildModeUI();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Transform camT = Camera.main.transform;
        if (Physics.Raycast(camT.position, camT.forward, out hit, 6f, torchLayer))
        {
            Vector3 uiPos = Camera.main.WorldToScreenPoint(hit.collider.transform.position);
            UIMGR.instance.UpdateTorchTimeUI(uiPos, hit.collider.gameObject.GetComponentInParent<Torch>().lifespan);
        }
        else
        {
            UIMGR.instance.HideTorchTimeUI();
        }

        if (InputMgr.instance.GetButtonDownBuildMode())
        {
            if (heldTorch == null)
            {
                EnterBuildMode();
            }
            else
            {
                ExitBuildMode();
            }
        }

        if (heldTorch == null)
            return;

        RaycastHit groundHit;
        bool hitObstacle = false;
        bool hitGround = false;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out groundHit, rayDistance, groundLayer))
        {
            hitGround = true;
        }
        if (Physics.CheckCapsule(groundHit.point, groundHit.point + Vector3.up * 1.5f, 0.3f, obstaclesLayer))
        {
            hitObstacle = true;
        }
        if (hitGround)
        {
            heldTorch.gameObject.SetActive(true);
            heldTorch.transform.position = groundHit.point;
            // draw light range + add functionality

            MeshRenderer meshRend = heldTorch.GetChild(0).gameObject.GetComponent<MeshRenderer>();

            int currentIsland = PlayerRespawner.instance.currentIsland;
            if (hitObstacle || !TorchesMgr.instance.CanPlaceOnIsland(currentIsland))
            {
                Material[] newMats = new Material[meshRend.materials.Length];
                for (int i = 0; i < meshRend.materials.Length; i++)
                {
                    newMats[i] = matWrong;
                }
                meshRend.materials = newMats;

                if (Input.GetButtonDown("Fire1"))
                {
                    if (!TorchesMgr.instance.CanPlaceOnIsland(currentIsland))
                        CantPlace?.Invoke();
                }
            }
            else
            {
                Material[] newMats = new Material[meshRend.materials.Length];
                for (int i = 0; i < meshRend.materials.Length; i++)
                {
                    newMats[i] = matAcceptable;
                }
                meshRend.materials = newMats;

                if (Input.GetButtonDown("Fire1") && !UIMGR.instance.isMenuOverlay && TorchesMgr.instance.CanPlaceOnIsland(currentIsland))
                {
                    // build
                    Instantiate(realTorchPrefab, heldTorch.position, heldTorch.rotation, TorchesMgr.instance._realTorchHolders[currentIsland]);
                    ExitBuildMode();
                    PlacedTorch?.Invoke();
                }
            }
        }
        else
        {
            heldTorch.gameObject.SetActive(false);
        }
    }
}
