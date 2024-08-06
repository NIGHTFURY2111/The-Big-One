using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEngine.Windows;

public class SlideState : BaseState
{
    public static SlideState Instance;
    public SlideState(PlayerStateMachine ctx, StateFactory factory) : base(ctx, factory)
    {
        Instance = this;
    }

    Vector3 slidedir;
    float slideSpeed;
    float elapsedTime;
    float percent;
    public override void EnterState()
    {
        elapsedTime = 0;
        //ctx.transform.localScale = new Vector3(1, 0.5f, 1);
        slidedir = ((ctx.MoveDir().magnitude == 0) ? ctx.transform.forward : ctx.MovementVector());
        slideSpeed = (ctx._magnitude>100f) ? ctx._magnitude + 5f : ctx._magnitude + ctx._slideSpeed;
    }

    public override void UpdateState()
    {

        if (slideSpeed > ctx._slideSpeed +20)
        {
            //Debug.Log(slideSpeed);
            //Debug.Log( elapsedTime +" "+ ctx.slideNormalizingTime+ " " + percent);
            //slideSpeed = ctx._magnitude * percent + ctx._slideSpeed * (1 - percent);
            elapsedTime += 0.1f;
            percent = elapsedTime / ctx.slideNormalizingTime;
            slideSpeed = Mathf.Lerp(slideSpeed, ctx._slideSpeed + 20, percent*Time.deltaTime);
        }

        ctx._moveDirectionX = slidedir.x * slideSpeed;
        ctx._moveDirectionZ = slidedir.z * slideSpeed;
        CheckSwitchState();

    }


    public override void ExitState()
    {
        //ctx.transform.localScale = new Vector3(1, 1, 1);
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
    }
}
