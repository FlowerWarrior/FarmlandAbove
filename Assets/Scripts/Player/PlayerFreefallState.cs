using UnityEngine;

public class PlayerFreefallState : PlayerState
{
    private bool grounded;
    private Vector2 inputVector;

    public PlayerFreefallState(Character character, PlayerStateMachine stateMachine) : base(character, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        inputVector = Vector2.zero;
        grounded = false;

        character.SetViewBobbingMode(Character.ViewBobbingMode.None);
    }

    public override void HandleInput()
    {
        base.HandleInput();
        inputVector.x = Input.GetAxisRaw("Horizontal");
        inputVector.y = Input.GetAxisRaw("Vertical");
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (grounded)
        {
            character.viewBobValue.y = Mathf.PI * 3 / 2;
            stateMachine.ChangeState(character.standing);
        }
        else
        {
            character.Move(inputVector, character.airMoveSpeed, character.airAcceleration, character.airDeAcceleration);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        grounded = character.CheckGroundOverlap();
    }
}