using UnityEngine;

public class PlayerStandingState : PlayerGroundedState
{
    private bool jump;
    private bool crouch;
    private bool isSprinting = false;

    public static System.Action StartedRunning;
    public static System.Action StoppedRunning;

    public PlayerStandingState(Character character, PlayerStateMachine stateMachine) : base(character, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        jump = false;
        crouch = false;
        isSprinting = false;
        speed = character.groundMoveSpeed;
    }

    public override void HandleInput()
    {
        base.HandleInput();
        crouch = InputMgr.instance.GetButtonCrouch();
        jump = InputMgr.instance.GetButtonDownJump();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!character.CheckGroundOverlap())
        {
            stateMachine.ChangeState(character.freefall);
        }

        if (jump)
        {
            stateMachine.ChangeState(character.jumping);
        }

        else if (crouch)
        {
            stateMachine.ChangeState(character.crouching);
        }

        // Handle sprinting
        if (InputMgr.instance.GetButtonSprint())
        {
            if (!isSprinting)
            {
                isSprinting = true;
                StartedRunning?.Invoke();
            }
            speed = character.sprintMoveSpeed;
        }
        else
        {
            if (isSprinting)
            {
                isSprinting = false;
                StoppedRunning?.Invoke();
            }
            speed = character.groundMoveSpeed;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (character.GetCurrentMoveVelocity() > 0.05f)
        {
            character.SetViewBobbingMode(Character.ViewBobbingMode.Walk);
        }
        else
        {
            character.SetViewBobbingMode(Character.ViewBobbingMode.Idle);
        }

    }
}