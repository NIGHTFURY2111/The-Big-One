using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : BaseState
{
    public JumpState(PlayerStateMachine ctx, StateFactory factory) : base(ctx, factory)
    {

    }

    public override void EnterState()
    {
        ctx._moveDirectionY = ctx._jumpSpeed;
    }
    public override void UpdateState()
    {
        CheckSwitchState();
    }
    public override void ExitState()
    {
    }

    public override void CheckSwitchState()
    {   //dash fall idle

        //Dash
        if (ctx._dash.WasPerformedThisFrame())
        {
            SwitchState(factory.Dash());
            return;
        }
        //Fall
        if (ctx._moveDirectionY <= 0f)
        {
            SwitchState(factory.Fall());
            return;
        }
        //idle (TODO)
        if (ctx._characterController.isGrounded)
        {
            SwitchState(factory.Idle());
            return;
        }


    }
}
