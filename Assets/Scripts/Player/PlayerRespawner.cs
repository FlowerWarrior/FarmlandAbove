using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawner : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] Transform[] respawnPoints;
    [SerializeField] float yLimit;
    [SerializeField] float respawnDelay; 
    internal Rigidbody rb;

    internal GameObject playerInstance;
    internal static PlayerRespawner instance;
    internal static System.Action PlayerFellBelow;

    [HideInInspector] internal int currentIsland = 0;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        DestroyPlayersInScene();
        SpawnPlayer();
    }

    private void DestroyPlayersInScene()
    {
        GameObject[] playersFound = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in playersFound)
        {
            Destroy(player);
        }

        rb = null;
    }

    private void SpawnPlayer()
    {
        InputMgr.instance.movementEnabled = true;
        playerInstance = Instantiate(playerPrefab, respawnPoints[currentIsland].position, respawnPoints[currentIsland].rotation);
        rb = playerInstance.transform.GetChild(0).gameObject.GetComponent<Rigidbody>();
    }

    IEnumerator RespawnPlayerAfter(float sec)
    {
        yield return new WaitForSeconds(sec);
        SpawnPlayer();
    }

    private void FixedUpdate()
    {
        if (rb == null)
            return;

        if (rb.transform.position.y < yLimit)
        {
            DestroyPlayersInScene();
            PlayerFellBelow?.Invoke();
            StartCoroutine(RespawnPlayerAfter(respawnDelay));
        }
    }

    public void DisablePlayerControls()
    {
        InputMgr.instance.movementEnabled = false;
        playerInstance.transform.GetChild(1).GetComponent<CameraController>().cameraControlsActive = false;
    }

    public void EnablePlayerControls()
    {
        InputMgr.instance.movementEnabled = true;
        playerInstance.transform.GetChild(1).GetComponent<CameraController>().cameraControlsActive = true;
    }

    public Vector3 GetPlayerCamPos()
    {
        return playerInstance.transform.GetChild(1).GetChild(0).position;
    }

    public Vector3 GetPlayerRbVelocity()
    {
        if (rb != null)
        {
            return rb.velocity;
        }
        else
        {
            return Vector3.zero;
        }
    }

    public void TeleportPlayerTo(Transform destination)
    {
        rb.position = destination.position;
        rb.rotation = destination.rotation;
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < respawnPoints.Length; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(respawnPoints[i].position, 0.3f);
        }
    }
}
