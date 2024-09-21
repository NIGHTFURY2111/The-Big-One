using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class FallState : BaseState
{
    public static FallState instance;
    private float angle;
    private float speed;
    private float tgtVelocity;
    public event Action wallAngle;

    float test;
    //public event Action wallAngle;
    //private bool slideCheck = false;

    public FallState(PlayerStateMachine ctx, StateFactory factory) : base(ctx, factory)
    {
        instance = this;
    }

    

    public override void EnterState()
    {
        //ctx.Collide += FallState.instance.wallCollide;

        ctx._getPCC._drag = 0;

        tgtVelocity = ctx._getPCC._currentVelocityMagnitude;
        ctx._getPCC.SetMaxlinVel(500);
        ctx._getPCC.SetCurrentVelocity(ctx._getPCC._currentVelocityMagnitude);

        test = ctx._getPCC._currentHorizontalVelocityMagnitude;
            
    }

    public override void FixedState()
    {   
        ctx._moveDirection = ctx.MovementVector();
        ctx._getPCC.AirMove(ctx._forceAppliedInAir);
        ctx._getPCC.AirMoveForceLimit(test, ctx._getMaxVelocityInAir);
    }

    public override void UpdateState()
    {
        CheckSwitchState();
    }


    public override void ExitState()
    {
        ctx._getPCC.AirMoveForceLimit(0,float.MaxValue);
        //ctx.Collide -= FallState.instance.wallCollide;
        //slideCheck = false;
    }
    //public void slideEnter()
    //{
    //    slideCheck = true;
    //}
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
    {   //idle dash slide

        //Dash

        if (ctx._dash.WasPerformedThisFrame())
        {
            SwitchState(factory.Dash());
            return;
        }                   

        //Idle
        if (ctx._isGrounded && !ctx._slide.IsPressed())
        {
            SwitchState(factory.Idle());
            return;
        }

        //slide
        if(ctx._isGrounded && ctx._slide.IsPressed())
        {
            SwitchState(factory.Slide());
            return;
        }

        if (ctx._getPCC.WallRunCheckRight(ctx._getWallRunAngle, ctx._getWallRunRaycastDistance) || ctx._getPCC.WallRunCheckLeft(ctx._getWallRunAngle, ctx._getWallRunRaycastDistance))
        {
            if (ctx._getPCC._currentHorizontalVelocityMagnitude > ctx._getWallRunReqSpeed)
            {
                SwitchState(factory.WallRun());
                return;
            }
        }

            //if (ctx._grapple.WasPerformedThisFrame())
            //{
            //    SwitchState(factory.GrappleStart());
            //    return;
            //}

            ctx._grapple.started += OnActionCanceled;
            ctx._grapple.performed += OnActionPerformed;

    }

    //public void wallCollide(ControllerColliderHit hit)
    //{
    //    if (hit.gameObject.CompareTag("wall"))
    //    {
    //        angle = Vector3.SignedAngle(hit.normal, hit.moveDirection,Vector3.up);
    //        if (Mathf.Clamp(angle,-ctx._maxWallMovingAngle,ctx._maxWallMovingAngle) == angle)
    //        {
    //            WallRunState wrss = (WallRunState)factory.WallRun();
    //            wrss._angle = angle;
    //            wrss._hit = hit;
    //            SwitchState(factory.WallRun());
    //            return;
    //        }
           
    //        //SwitchState(factory.WallSlide()); issue is if you are wall jumping and then attach to a wall while falling down you enter in a slide which feels awkward will have to think about wall slide as a whole

    //        SwitchState(factory.WallSlide());
    //        return;
    //    }
}

