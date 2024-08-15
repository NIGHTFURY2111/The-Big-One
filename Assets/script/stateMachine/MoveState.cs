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
    public override void ExitState()
    {
    }

    public override void EnterState()
    {
        ctx._TGTSpeed = ctx._walkingSpeed;
        ctx._getPCC.SetMaxlinVel(ctx._walkingSpeed);
        ctx._getPCC._TGTvelocityMagnitude = ctx._walkingSpeed;
        //ctx._getPCC._drag = 0;     
    }

    public override void FixedState()
    {
        ctx._moveDirectionX = ctx.MovementVector().x;
        ctx._moveDirectionZ = ctx.MovementVector().z;


        ctx._getPCC.calculateAccelration(ctx._TGTSpeed);
        ctx._getPCC.Move();
    }

    public override void UpdateState()
    {
        CheckSwitchState();
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
        if (!ctx._isGrounded) //ctx._characterController.isGrounded
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
