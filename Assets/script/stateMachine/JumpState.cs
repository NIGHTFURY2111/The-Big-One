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
    float tgtVelocity;
    bool jumpCompleted;
    public JumpState(PlayerStateMachine ctx, StateFactory factory) : base(ctx, factory)
    {
        instance = this;
    }
   
    public override void EnterState()
    {
        speed = (slideCheck)? ctx._slideSpeed:ctx._walkingSpeed;
        
        ctx._getPCC._drag = 0;

        ctx._getPCC._gravity = ctx._gravity;

        ctx._getPCC.setmaxlinvel(500);

        //tgtVelocity = ctx._getPCC._velocityMagnitude;

        jumpCompleted = false;

        ctx._moveDirectionY = ctx._jumpSpeed;

        tgtVelocity = ctx._getPCC.setCurrentVelocity(ctx._walkingSpeed);

        ctx.StartCoroutine(Jumping());
    }
    public override void FixedState()
    {
        ctx._moveDirection = ctx.MovementVector();
        //ctx._getPCC.calculateAccelration(tgtVelocity);
        //ctx._getPCC.move();
        ctx._getPCC.airMove(ctx._forceAppliedInAir);
    }

    public override void UpdateState()
    {
        if (jumpCompleted) 
        {
            CheckSwitchState();
        }
        
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

    IEnumerator Jumping()
    {
        ctx._getPCC.jumpForce(ctx._jumpSpeed);
        yield return new WaitForSecondsRealtime(ctx._jumptime);     
        CheckSwitchState();
        jumpCompleted = true;
    }

    public override void CheckSwitchState()
    {   
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
        //idle
        if (ctx._getPCC.isGrounded())
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
