using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallState : BaseState
{
    public FallState(PlayerStateMachine ctx, StateFactory factory) : base(ctx, factory)
    {
    }

    public override void UpdateState()
    {   
        CheckSwitchState();
    }

    public override void EnterState()
    {
     
    }

    public override void ExitState()
    {
     
    }

    public override void CheckSwitchState()
    {   //idle dash

        //Dash
        if (ctx._dash.WasPerformedThisFrame())
        {
            SwitchState(factory.Dash());
            return;
        }
        //Idle
        if (ctx._characterController.isGrounded)
        {
            SwitchState(factory.Idle());
            return;
        }

    }
}
