using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CodeSF;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(Rigidbody))]
public class Character : MonoBehaviour
{
    #region Variables

    internal PlayerStateMachine movementSM;
    internal PlayerStandingState standing;
    internal PlayerJumpingState jumping;
    internal PlayerCrouchingState crouching;
    internal PlayerFreefallState freefall;

    Rigidbody rb;
    Vector2 moveSemiInputs;
    Vector2 currentMoveSpeed;
    float currentControlSpeed;
    float currentAcceleration;
    float currentDeAcceleration;

    internal Vector2 viewBobValue;
    Vector2 currentViewBobScale, currentViewBobFrequency;
    Vector2 targetViewBobScale, targetViewBobFrequency;
    ViewBobbingMode viewBobMode = ViewBobbingMode.Idle;
    bool didFootstepSound = false;

    public static System.Action<Vector3> Footstep;

    [SerializeField] internal float maxMoveSpeed = 4f;

    [Header("Ground Controls")]
    [SerializeField] internal float groundMoveSpeed = 4f;
    [SerializeField] internal float sprintMoveSpeed = 5f;
    [SerializeField] internal float groundAcceleration = 70f;
    [SerializeField] internal float groundDeAcceleration = 80f;
    [SerializeField] internal float groundOverlapRadius = 0.17f;
    [SerializeField] LayerMask whatIsGround;
    [SerializeField] Transform groundCheckPoint;

    [Header("Air Controls")]
    [SerializeField] internal float airMoveSpeed = 4f;
    [SerializeField] internal float airAcceleration = 8f;
    [SerializeField] internal float airDeAcceleration = 7f;
    [SerializeField] float additionalGravity = 3f;
    [SerializeField] internal float jumpForce = 70f;

    [Header("Crouch Controls")]
    [SerializeField] CameraController cameraController;
    [SerializeField] internal float crouchCameraOffset = 0.55f;
    [SerializeField] internal float crouchMoveSpeed = 2.2f;
    [SerializeField] internal float crouchAcceleration = 24f;
    [SerializeField] internal float crouchDeAcceleration = 30f;

    [Header("View Bobbing")]
    [SerializeField] internal Transform viewBobbingTransform;
    [SerializeField] float viewBobLerping = 4f;

    [SerializeField] Vector2 walkBobbingScale = new Vector2(0.07f, 0.1f);
    [SerializeField] Vector2 walkBobbingFrequency = new Vector3(1.15f, 2.3f);

    [SerializeField] Vector2 idleBobbingScale = new Vector2(0.05f, 0.08f);
    [SerializeField] Vector2 idleBobbingFrequency = new Vector2(1f, 2f);

    #endregion

    #region Enums

    public enum ViewBobbingMode
    {
        Idle,
        Walk,
        None
    }

    #endregion

    #region Methods

    public void Move(Vector2 moveInputs, float maxSpeed, float acceleration, float deAcceleration)
    {
        currentAcceleration = acceleration;
        currentDeAcceleration = deAcceleration;
        currentControlSpeed = maxSpeed;
        moveSemiInputs = moveInputs;
        moveSemiInputs = Vector2.ClampMagnitude(moveSemiInputs, 1);
    }

    public void HandleSmoothMovement()
    {
        currentMoveSpeed.x = CalculateSmoothMoveScalar(moveSemiInputs.x, currentMoveSpeed.x);
        currentMoveSpeed.y = CalculateSmoothMoveScalar(moveSemiInputs.y, currentMoveSpeed.y);

        Vector3 targetVelocity = currentMoveSpeed.y * transform.forward;
        targetVelocity += currentMoveSpeed.x * transform.right;
        targetVelocity = Vector3.ClampMagnitude(targetVelocity, currentControlSpeed);

        targetVelocity.y = 0;

        rb.AddForce(targetVelocity / 2f, ForceMode.VelocityChange);

        // apply x,z drag
        float drag = 3f; //27.6f;
        Vector3 t_velocity = rb.velocity;

        t_velocity.x *= 1f / drag;
        t_velocity.y *= 1f + 0;
        t_velocity.z *= 1f / drag;

        rb.velocity = t_velocity;
    }

    private float CalculateSmoothMoveScalar(float input, float scalar)
    {
        if (input != 0)
        {
            scalar += input * currentAcceleration * Time.fixedDeltaTime;
            scalar = Mathf.Clamp(scalar, -currentControlSpeed, currentControlSpeed);
        }
        else
        {
            scalar = Mathf.MoveTowards(scalar, 0, currentDeAcceleration * Time.fixedDeltaTime);
        }

        return scalar;
    }

    public void ApplyImpulse(Vector3 force)
    {
        rb.AddForce(force, ForceMode.Impulse);
    }

    public bool CheckGroundOverlap()
    {
        return Physics.OverlapSphere(groundCheckPoint.position, groundOverlapRadius, whatIsGround).Length > 0;
    }

    public float GetCurrentMoveVelocity()
    {
        Vector3 flattenedVector = rb.velocity;
        flattenedVector.y = 0;

        return flattenedVector.magnitude;
    }

    public Vector3 GetCurrentRbVelocity()
    {
        return rb.velocity;
    }

    private void HandleViewBobbing()
    {
        currentViewBobFrequency = Vector2.Lerp(currentViewBobFrequency, targetViewBobFrequency, Time.deltaTime * viewBobLerping);
        currentViewBobScale = Vector2.Lerp(currentViewBobScale, targetViewBobScale, Time.deltaTime * viewBobLerping);

        Vector3 newPos = Vector3.zero;

        viewBobValue.x += Time.deltaTime * currentViewBobFrequency.x * (GetCurrentMoveVelocity() + 1) * GetSinValueFalloffMultiplier(viewBobValue.x);
        viewBobValue.x = WrapSinValue(viewBobValue.x);

        viewBobValue.y += Time.deltaTime * currentViewBobFrequency.y * (GetCurrentMoveVelocity() + 1) * GetSinValueFalloffMultiplier(viewBobValue.y);
        viewBobValue.y = WrapSinValue(viewBobValue.y);

        newPos.x = currentViewBobScale.x * Mathf.Sin(viewBobValue.x);
        newPos.y = currentViewBobScale.y * Mathf.Sin(viewBobValue.y);
        viewBobbingTransform.localPosition = newPos;

        if (viewBobMode == ViewBobbingMode.Walk)
        {
            if (viewBobValue.y > 3f * 3.14159f / 2f && !didFootstepSound)
            {
                Footstep?.Invoke(groundCheckPoint.position); // play when when the sine value is lowest
                didFootstepSound = true;
            }
            if (viewBobValue.y > 0f && viewBobValue.y < 3f * 3.14159f / 2f && didFootstepSound)
            {
                didFootstepSound = false; // reset bool
            }
        }
    }

    public void SetViewBobbingMode(ViewBobbingMode mode)
    {
        viewBobMode = mode;

        switch (viewBobMode)
        {
            case ViewBobbingMode.Idle:
                targetViewBobFrequency = idleBobbingFrequency;
                targetViewBobScale = idleBobbingScale;
                break;
            case ViewBobbingMode.Walk:
                targetViewBobFrequency = walkBobbingFrequency;
                targetViewBobScale = walkBobbingScale;
                break;
            case ViewBobbingMode.None:
                targetViewBobFrequency = Vector2.zero;
                targetViewBobScale = Vector2.zero;
                break;
        }
    }

    public void SetCameraCrouchOffset(bool status)
    {
        if (status)
        {
            cameraController.crouchOffset = crouchCameraOffset;
        }
        else
        {
            cameraController.crouchOffset = 0;
        }
    }

    public Vector3 GetRbPos()
    {
        return rb.transform.position;
    }

    #endregion

    #region MonoBehavior Callbacks

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        movementSM = new PlayerStateMachine();

        standing = new PlayerStandingState(this, movementSM);
        crouching = new PlayerCrouchingState(this, movementSM);
        jumping = new PlayerJumpingState(this, movementSM);
        freefall = new PlayerFreefallState(this, movementSM);

        movementSM.Initialize(standing);
    }

    private void Update()
    {
        movementSM.CurrentState.HandleInput();
        movementSM.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        movementSM.CurrentState.PhysicsUpdate();

        HandleSmoothMovement();
        rb.AddForce(Vector3.down * additionalGravity * rb.mass);
    }

    private void LateUpdate()
    {
        HandleViewBobbing();
        cameraController.UpdateToolsHolderPos();
    }

    private void OnDrawGizmos()
    {
        if (groundCheckPoint != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(groundCheckPoint.position, groundOverlapRadius);
        }

        Gizmos.color = new Color32(0, 72, 186, 120);
        Gizmos.DrawMesh(GetComponent<MeshFilter>().sharedMesh, -1, transform.position, transform.rotation, transform.lossyScale);
    }

    #endregion
}
