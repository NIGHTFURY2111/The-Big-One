using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FallState : BaseState
{
    public static FallState instance;
    private bool slideCheck = false;
    private float angle;
    private float speed;
    public event Action wallAngle;
    public FallState(PlayerStateMachine ctx, StateFactory factory) : base(ctx, factory)
    {
        instance = this;
    }


    public override void EnterState()
    {
        ctx.Collide += FallState.instance.wallCollide;
        if (slideCheck)
        {
            speed = ctx._slideSpeed;
        }
        else
        {
            speed = ctx._walkingSpeed;
        }
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
        ctx.Collide -= FallState.instance.wallCollide;
    }
    public void slideEnter()
    {
        slideCheck = true;
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
        if (ctx._characterController.isGrounded && !ctx._slide.IsPressed())
        {
            SwitchState(factory.Idle());
            return;
        }
        if(ctx._characterController.isGrounded && ctx._slide.IsPressed())
        {
            SwitchState(factory.Slide());
            return;
        }


        

    }
    public void wallCollide(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("wall"))
        {
            angle = Vector3.SignedAngle(hit.normal, hit.moveDirection,Vector3.up);
            Debug.Log(angle);
            if (Mathf.Clamp(angle,-ctx._maxWallMovingAngle,ctx._maxWallMovingAngle) == angle)
            {
                WallRunState wrss = (WallRunState)factory.WallRun();
                wrss._angle = angle;
                wrss._hit = hit;
                SwitchState(factory.WallRun());
                return;
            }
           
            SwitchState(factory.WallSlide());

            
            return;

        }
    

    }
}
