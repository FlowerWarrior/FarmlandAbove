using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    internal float crouchOffset = 0;
    internal bool cameraControlsActive = true;

    Vector2 mouseInput;
    Vector3 camOffset;

    Vector3 itemHolderOffset = new Vector3(0.622f, -0.14f, 0.77f);

    [SerializeField] Transform camT;
    [SerializeField] Camera cam;
    [SerializeField] Transform headT;
    [SerializeField] Transform crouchT;
    [SerializeField] Rigidbody playerRb;
    [SerializeField] float playerVelocitySensitivity = -0.2f;
    [SerializeField] float camSensitivity = 500f;
    [SerializeField] float camYDamping = 7f;

    [Header("ItemHolder")]
    [SerializeField] Transform itemHolderT;
    [SerializeField] float itemHolderBobbingScale = 0.24f;

    public static System.Action AfterCameraLateUpdate;

    public void UpdateToolsHolderPos()
    {
        itemHolderT.localPosition = itemHolderOffset - (camT.localPosition * itemHolderBobbingScale);
    }

    // Start is called before the first frame update
    private void Start()
    {
        camOffset = headT.position - playerRb.transform.position;
        transform.rotation = Quaternion.identity;
        targetFOV = normalFOV;
    }

    private void Update()
    {
        if (cameraControlsActive)
        {
            mouseInput.x = InputMgr.instance.GetMouseHorizontal();
            mouseInput.y = InputMgr.instance.GetMouseVertical();
        }
        else
        {
            mouseInput = Vector2.zero;
        }

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, fovSmoothness * Time.deltaTime);
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        // Player Rotation
        Vector3 playerRot = Vector3.zero;
        playerRot.y = mouseInput.x * camSensitivity * Time.deltaTime;
        playerRb.transform.Rotate(playerRot, Space.Self);

        // Move camera to head
        Vector3 camPos = playerRb.transform.position + camOffset;

        camPos.y = Mathf.Lerp(transform.position.y, camPos.y - crouchOffset - (playerRb.velocity.y * playerVelocitySensitivity), camYDamping * Time.deltaTime);
        transform.position = camPos;

        // Camera rotation
        Vector3 camRot = transform.rotation.eulerAngles;

        camRot.y = playerRb.transform.rotation.eulerAngles.y;
        camRot.x -= mouseInput.y * camSensitivity * Time.deltaTime;

        if (camRot.x > 180) camRot.x -= 360;
        if (camRot.x <= -180) camRot.x += 360;

        camRot.x = Mathf.Clamp(camRot.x, -70f, 70f); // to fix

        transform.rotation = Quaternion.Euler(camRot);

        AfterCameraLateUpdate?.Invoke();
    }

    [SerializeField] float fovSmoothness = 3f;
    float targetFOV;
    float runningFOV = 67f, normalFOV = 60f;

    private void OnEnable()
    {
        PlayerStandingState.StartedRunning += SetFOVRunning;
        PlayerStandingState.StoppedRunning += SetFOVNormal;
    }

    private void OnDisable()
    {
        PlayerStandingState.StartedRunning -= SetFOVRunning;
        PlayerStandingState.StoppedRunning -= SetFOVNormal;
    }

    private void SetFOVRunning()
    {
        targetFOV = runningFOV;
    }

    private void SetFOVNormal()
    {
        targetFOV = normalFOV;
    }
}
