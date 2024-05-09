using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IdleState : BaseState
{
    public IdleState(PlayerStateMachine ctx, StateFactory factory) : base(ctx, factory)
    {

    }

    public override void EnterState()
    {
        ctx._moveDirectionX = 0f;
        ctx._moveDirectionY = -2f;
        ctx._moveDirectionZ = 0f;
    }

    public override void UpdateState()
    {
        CheckSwitchState();
    }

    public override void ExitState()
    {
       
    }

    public override void CheckSwitchState()
    {   //jump walk dash slide
        
        //Dash
        if (ctx._dash.WasPerformedThisFrame())
        {
            SwitchState(factory.Dash());
            return;
        }

        //Move
        if (ctx._move.IsInProgress())
        {
            SwitchState(factory.Move());
            return;
        }

        //Jump
        if (ctx._jump.WasPerformedThisFrame())
        {
            SwitchState(factory.Jump());
            return;
        }

        //Slide
        if (ctx._slide.WasPerformedThisFrame())
        {
            SwitchState(factory.Slide());
            return;
        }
    }
}
