using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class JumpState : BaseState
{
   public static JumpState instance;
    float speed, actualSpeed;
    public JumpState(PlayerStateMachine ctx, StateFactory factory) : base(ctx, factory)
    {
        instance = this;
    }
    public override void EnterState()
    {
        ctx.Collide += JumpState.instance.wallCollide;
        speed = (ctx._magnitude.Equals(0f)) ? ctx._walkingSpeed: ctx._magnitude;
        //speed = (slideCheck)? ctx._slideSpeed:ctx._walkingSpeed;
        ctx._moveDirectionX = ctx._characterController.velocity.x;
        ctx._moveDirectionZ = ctx._characterController.velocity.z;
        ctx._moveDirectionY = ctx._jumpSpeed;

    }
    public override void UpdateState()
    {
        ctx._moveDirectionX = ctx._characterController.velocity.x;
        ctx._moveDirectionZ = ctx._characterController.velocity.z;
        //Debug.Log(ctx.MovementVector() == Vector3.zero);
        if (ctx.MovementVector() == Vector3.zero)
        {
            ctx._moveDirectionX -= ctx._characterController.velocity.normalized.x * Time.deltaTime;
            ctx._moveDirectionZ -= ctx._characterController.velocity.normalized.z * Time.deltaTime;
        }
        else
        {
            ctx._moveDirectionX += ctx.MovementVector().x * ctx._forceAppliedInAir * Time.deltaTime;
            ctx._moveDirectionZ += ctx.MovementVector().z * ctx._forceAppliedInAir * Time.deltaTime;
        }

        CheckSwitchState();
    }
    public override void ExitState()
    {
        ctx.Collide -= JumpState.instance.wallCollide;
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
        ctx._grapple.canceled += OnActionCanceled;
        ctx._grapple.performed += OnActionPerformed;
    }
    public void wallCollide(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("wall") && ctx._jump.WasPressedThisFrame())
        {
            SwitchState(factory.WallJump());
            return;
        }
    }
}
