using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class JumpState : BaseState
{
    public static JumpState instance;
    private float speed;
    private bool slideCheck = false;
    public JumpState(PlayerStateMachine ctx, StateFactory factory) : base(ctx, factory)
    {
        instance = this;
    }
   
    public override void EnterState()
    {

        speed = (slideCheck)? ctx._slideSpeed:ctx._walkingSpeed;
        ctx._moveDirectionY = ctx._jumpSpeed;
    }
    public override void UpdateState()
    {
        ctx._moveDirectionX = ctx.MovementVector().x * speed;
        ctx._moveDirectionZ = ctx.MovementVector().z * speed;
        CheckSwitchState();
    }
    public override void ExitState()
    {
        slideCheck = false;
    }
    public void slideEnter()
    {
       slideCheck = true;
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
        //if (ctx._collision.gameObject.CompareTag("wall"))
        //{
        //    SwitchState(factory.WallSlide());
        //    return;
        //}

        //if (ctx._grapple.WasPerformedThisFrame())
        //{
        //    SwitchState(factory.GrappleStart());
        //    return;
        //}


        ctx._grapple.canceled += OnActionCanceled;
        ctx._grapple.performed += OnActionPerformed;
    }

}
