using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class SlideState : BaseState
{
    public event Action slideExit;
    public static SlideState Instance;
    public SlideState(PlayerStateMachine ctx, StateFactory factory) : base(ctx, factory)
    {
        Instance = this;
    }

    Vector3 slidedir;
    public override void EnterState()
    {
        ctx.transform.localScale = new Vector3(1, 0.5f, 1);
        slidedir = ((ctx.MoveDir().magnitude == 0) ? ctx.transform.forward : ctx.MovementVector());
    }

    public override void UpdateState()
    {
        ctx._moveDirectionX = slidedir.x * ctx._slideSpeed;
        ctx._moveDirectionZ = slidedir.z * ctx._slideSpeed;
        CheckSwitchState();

    }


    public override void ExitState()
    {
        ctx.transform.localScale = new Vector3(1, 1, 1);
        slideExit?.Invoke();
        
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
    {   //jump fall idle dash

        //Fall
            //if (!ctx._characterController.isGrounded)
            //{
            //    SwitchState(factory.Fall());
            //    return;
            //}
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
        //Idle
        if (ctx._slide.WasReleasedThisFrame())
        {
            SwitchState(factory.Idle());
            return;
        }

        ctx._grapple.started += OnActionCanceled;
        ctx._grapple.performed += OnActionPerformed;
    }
}
