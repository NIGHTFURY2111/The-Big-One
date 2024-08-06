using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveState : BaseState
{
    float speed;
    float elapsedTime;
    float percent;

    public MoveState(PlayerStateMachine ctx, StateFactory factory) : base(ctx, factory)
    {
    }

    public override void EnterState()
    {
    }
    public override void UpdateState()
    {
        ctx._moveDirectionX = ctx.MovementVector().x * ctx._walkingSpeed;
        ctx._moveDirectionZ = ctx.MovementVector().z * ctx._walkingSpeed;
        CheckSwitchState();
    }
    public override void ExitState()
    {
    }

    public override void CheckSwitchState()
    {   //idle dash jump  slide fall
        
        //Fall
        if (!ctx._characterController.isGrounded)
        {
            SwitchState(factory.Fall());
            return;
        }
        //Idle
        if (ctx.MoveDir().magnitude == 0f)
        {
            SwitchState(factory.Idle());
            return;
        }

        //Dash
        if (ctx._dash.WasPerformedThisFrame())
        {
            SwitchState(factory.Dash());
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
