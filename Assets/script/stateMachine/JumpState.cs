using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class JumpState : BaseState
{
    //public static JumpState instance;
    float tgtVelocity;
    bool jumpCompleted;
    //private float speed;
    //private bool slideCheck = false;

    float test;
    public JumpState(PlayerStateMachine ctx, StateFactory factory) : base(ctx, factory)
    {
        //instance = this;
    }
   
    public override void EnterState()
    {
        //speed = (slideCheck)? ctx._slideSpeed:ctx._walkingSpeed;

        ctx._getPCC._drag = 0;

        //tgtVelocity = ctx._getPCC._velocityMagnitude;

        jumpCompleted = false;


        ctx._getPCC.SetMaxlinVel(500);

        ctx._moveDirectionY = ctx._jumpSpeed;

        tgtVelocity = ctx._getPCC.SetCurrentVelocity(ctx._walkingSpeed);

        test = ctx._getPCC._currentHorizontalVelocityMagnitude;

        ctx.StartCoroutine(Jumping());
    }
    public override void FixedState()
    {
        ctx._moveDirection = ctx.MovementVector();
        ctx._getPCC.AirMove(ctx._forceAppliedInAir);
        ctx._getPCC.AirMoveForceLimit(test, ctx._getMaxVelocityInAir);

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
        ctx._getPCC.AirMoveForceLimit(0,float.MaxValue);
        //slideCheck = false;
    }

    IEnumerator Jumping()
    {
        ctx._getPCC.JumpForce(ctx._jumpSpeed);
        yield return new WaitForSecondsRealtime(ctx._jumptime);     
        CheckSwitchState();
        jumpCompleted = true;
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
        if (ctx._isGrounded)
        {
            SwitchState(factory.Idle());
            return;
        }

        ctx._grapple.canceled += OnActionCanceled;
        ctx._grapple.performed += OnActionPerformed;

        if (ctx._getPCC.WallRunCheckRight(ctx._getWallRunAngle, ctx._getWallRunRaycastDistance))
        {
            SwitchState(factory.WallRun());
            //ctx._getisWall = true;
            return;
        }
    }

}
