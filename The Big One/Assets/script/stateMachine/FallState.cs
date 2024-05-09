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
            angle = Vector3.Angle(hit.normal, hit.moveDirection);
            if (angle <= ctx._maxWallMovingAngle && angle >= ctx._minWallMovingAngle)
            {
                Debug.Log(angle);
                SwitchState(factory.WallRun());
                return;
            }
           
            SwitchState(factory.WallSlide());
            
            return;

        }
    

    }
}
