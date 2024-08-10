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
    public event Action wallAngle;
    public FallState(PlayerStateMachine ctx, StateFactory factory) : base(ctx, factory)
    {
        instance = this;
    }


    public override void EnterState()
    {
        ctx.Collide += FallState.instance.wallCollide;
        speed = (ctx._magnitude< ctx._walkingSpeed) ? ctx._walkingSpeed : ctx._magnitude;
        ctx._moveDirectionX = ctx._characterController.velocity.x;
        ctx._moveDirectionZ = ctx._characterController.velocity.z;

        //if (slideCheck)
        //{
        //    speed = ctx._slideSpeed;
        //}
        //else
        //{
        //    speed = ctx._walkingSpeed;
        //}
    }
    public override void UpdateState()
    {
        ctx._moveDirectionX = ctx._characterController.velocity.x;
        ctx._moveDirectionZ = ctx._characterController.velocity.z;
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
        ctx.Collide -= FallState.instance.wallCollide;
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

        ctx._grapple.started += OnActionCanceled;
        ctx._grapple.performed += OnActionPerformed;

    }
    public void wallCollide(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("wall"))
        {
            angle = Vector3.SignedAngle(hit.normal, hit.moveDirection,Vector3.up);
            //Debug.Log(angle);
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
