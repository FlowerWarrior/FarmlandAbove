using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMgr : MonoBehaviour
{
    [SerializeField] float xMouseSenstivity;
    [SerializeField] float yMouseSenstivity;

    public bool movementEnabled = true;

    public static InputMgr instance;

    private void Awake() => instance = this;

    #region Methods

    public bool GetButtonDownJump()
    {
        if (movementEnabled)
            return Input.GetButtonDown("Jump");
        else
            return false;
    }

    public bool GetButtonCrouch()
    {
        if (movementEnabled)
            return Input.GetButton("Crouch");
        else
            return false;
    }

    public bool GetButtonSprint()
    {
        if (movementEnabled)
            return Input.GetButton("Sprint");
        else
            return false;
    }

    public float GetHorizontalAxis()
    {
        if (movementEnabled)
            return Input.GetAxisRaw("Horizontal");
        else
            return 0f;
    }

    public float GetVerticalAxis()
    {
        if (movementEnabled)
            return Input.GetAxisRaw("Vertical");
        else
            return 0f;
    }

    public float GetMouseHorizontal()
    {
        if (movementEnabled)
            return Input.GetAxis("Mouse X") * xMouseSenstivity;
        else
            return 0f;
    }

    public float GetMouseVertical()
    {
        if (movementEnabled)
            return Input.GetAxis("Mouse Y") * yMouseSenstivity;
        else
            return 0f;
    }

    public float GetMouseScroll()
    {
        if (movementEnabled)
            return Input.GetAxis("Mouse ScrollWheel");
        else
            return 0f;
    }

    #endregion
}
