using UnityEngine;

public class PlayerCrouchingState : PlayerGroundedState
{
    private bool crouch;

    public PlayerCrouchingState(Character character, PlayerStateMachine stateMachine) : base(character, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        crouch = true;
        speed = character.crouchMoveSpeed;
        character.SetCameraCrouchOffset(true);
    }

    public override void HandleInput()
    {
        base.HandleInput();
        crouch = InputMgr.instance.GetButtonCrouch();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!crouch)
        {
            stateMachine.ChangeState(character.standing);
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

    public override void Exit()
    {
        character.SetCameraCrouchOffset(false);
    }
}