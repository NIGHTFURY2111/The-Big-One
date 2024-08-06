using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
public class MoveState : BaseState
{
   
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
    private void OnActionCanceled(InputAction.CallbackContext context)
    {
        SwitchState(factory.GrappleStart());
        GrappleStart.instance.held = false;
        return;
    }
    private void OnActionPerformed(InputAction.CallbackContext context)
    {
        SwitchState(factory.GrappleStart());
        GrappleStart.instance.held = true;
        return;
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

        ctx._grapple.started += OnActionCanceled;
        ctx._grapple.performed += OnActionPerformed;

    }
}
