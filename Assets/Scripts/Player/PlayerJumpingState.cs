using UnityEngine;

public class PlayerJumpingState : PlayerState
{
    public static System.Action<Vector3> Jumped;

    public PlayerJumpingState(Character character, PlayerStateMachine stateMachine) : base(character, stateMachine)
    {
    }
    
    public override void Enter()
    {
        base.Enter();
        Jump();
        Jumped?.Invoke(character.GetRbPos());

        character.SetViewBobbingMode(Character.ViewBobbingMode.None);
        stateMachine.ChangeState(character.freefall);
    }
    private void Jump()
    {
        character.transform.Translate(Vector3.up * (character.groundOverlapRadius + 0.1f));
        character.ApplyImpulse(Vector3.up * character.jumpForce);
    }

    public override void HandleInput()
    {
        base.HandleInput();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}