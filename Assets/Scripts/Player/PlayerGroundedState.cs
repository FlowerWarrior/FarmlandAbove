using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    protected float speed;
    protected float rotationSpeed;

    private Vector2 inputVector;

    internal static System.Action HitGround;

    public PlayerGroundedState(Character character, PlayerStateMachine stateMachine) : base(character, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        inputVector = Vector2.zero;
        if (character.GetCurrentRbVelocity().y < -4.5f)
        {
            HitGround?.Invoke();
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void HandleInput()
    {
        base.HandleInput();
        inputVector.x = InputMgr.instance.GetHorizontalAxis();
        inputVector.y = InputMgr.instance.GetVerticalAxis();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        character.Move(inputVector, speed, character.groundAcceleration, character.groundDeAcceleration);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}