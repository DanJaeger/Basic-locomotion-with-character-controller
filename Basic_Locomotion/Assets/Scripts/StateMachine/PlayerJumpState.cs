using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerJumpState : PlayerBaseState, IRootState
{
    public PlayerJumpState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base(currentContext, playerStateFactory) {
        IsRootState = true;
    }
    public override void CheckSwitchStates()
    {
        if (Context.CharacterController.isGrounded)
        {
            SwitchState(Factory.Grounded());
        }
    }

    public override void EnterState()
    {
        HandleJump();
        InitializeSubState();
    }

    public override void ExitState()
    {
        Context.Anim.SetBool(Context.IsJumpingHash, false);
    }

    public override void InitializeSubState()
    {
        
    }

    public override void UpdateState()
    {
        HandleGravity();
        CheckSwitchStates();
    }
    void HandleJump()
    {
        Context.IsJumping = true;
        Context.CurrentMovementY = Context.InitialJumpVelocity;
        Context.AppliedMovementY = Context.InitialJumpVelocity;
        Context.Anim.SetBool(Context.IsJumpingHash, true);
    }
    public void HandleGravity()
    {
        bool isFalling;
        float fallMultiplier = 2.0f; 
        if (Context.HoldJump)
            isFalling = Context.CurrentMovementY <= 0 || !Context.IsJumpPressed;
        else
            isFalling = Context.CurrentMovementY <= 0;

        if (isFalling)
        {
            float previousYVelocity = Context.CurrentMovementY;
            Context.CurrentMovementY = Context.CurrentMovementY + (Context.Gravity * fallMultiplier * Time.deltaTime);
            Context.AppliedMovementY = Mathf.Max((previousYVelocity + Context.CurrentMovementY) * 0.5f, -10.0f);
        }
        else
        {
            float previousYVelocity = Context.CurrentMovementY;
            Context.CurrentMovementY = Context.CurrentMovementY + (Context.Gravity * Time.deltaTime);
            Context.AppliedMovementY = (previousYVelocity + Context.CurrentMovementY) * 0.5f;
        }
    }
}
