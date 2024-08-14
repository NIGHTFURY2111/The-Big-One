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
    private bool slideCheck = false;
    private float angle;
    private float speed;
    //public event Action wallAngle;
    public event Action wallAngle;
    float tgtVelocity;

    public FallState(PlayerStateMachine ctx, StateFactory factory) : base(ctx, factory)
    {
        instance = this;
    }

    

    public override void EnterState()
    {
        ctx.Collide += FallState.instance.wallCollide;
                            
        ctx._getPCC._drag = 0;

        ctx._getPCC._gravity = ctx._gravity;

        ctx._getPCC.setmaxlinvel(500);

        tgtVelocity = ctx._getPCC._velocityMagnitude;

        ctx._getPCC.setCurrentVelocity(ctx._getPCC._velocityMagnitude);
    }

    public override void FixedState()
    {
        ctx._moveDirection = ctx.MovementVector();
        ctx._getPCC.airMove(ctx._forceAppliedInAir);
    }

    public override void UpdateState()
    {
        CheckSwitchState();
    }


    public override void ExitState()
    {
        slideCheck = false;
        ctx.Collide -= FallState.instance.wallCollide;
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
    {   //idle dash

        //Dash

        if (ctx._dash.WasPerformedThisFrame())
        {
            SwitchState(factory.Dash());
            return;
        }                   

        //Idle
        if (ctx._getPCC.isGrounded() && !ctx._slide.IsPressed())
        {
            SwitchState(factory.Idle());
            return;
        }

        if(ctx._getPCC.isGrounded() && ctx._slide.IsPressed())
        {
            SwitchState(factory.Slide());
            return;
        }

        //if (ctx._grapple.WasPerformedThisFrame())
        //{
        //    SwitchState(factory.GrappleStart());
        //    return;
        //}


        ctx._grapple.started += OnActionCanceled;
        ctx._grapple.performed += OnActionPerformed;



    }

    public void wallCollide(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("wall"))
        {
            angle = Vector3.SignedAngle(hit.normal, hit.moveDirection,Vector3.up);
            if (Mathf.Clamp(angle,-ctx._maxWallMovingAngle,ctx._maxWallMovingAngle) == angle)
            {
                WallRunState wrss = (WallRunState)factory.WallRun();
                wrss._angle = angle;
                wrss._hit = hit;
                SwitchState(factory.WallRun());
                return;
            }
           
            //SwitchState(factory.WallSlide()); issue is if you are wall jumping and then attach to a wall while falling down you enter in a slide which feels awkward will have to think about wall slide as a whole

            
            SwitchState(factory.WallSlide());
            return;

        }
    

    }
}
