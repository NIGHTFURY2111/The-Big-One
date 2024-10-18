using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class SlideState : BaseState
{
    public event Action slideExit;
    float elapsedTime;
    float percent;
    //public static SlideState Instance;
    public SlideState(PlayerStateMachine ctx, StateFactory factory) : base(ctx, factory)
    {
        //Instance = this;
    }

    Vector3 slidedir;
    public override void EnterState()
    {
        elapsedTime = 0;
        ctx._TGTSpeed = (ctx._getPCC._currentVelocityMagnitude > 100f) ? ctx._getPCC._currentVelocityMagnitude: ctx._getPCC._currentVelocityMagnitude + ctx._slideSpeed;
        ctx._getPCC.SetMaxlinVel(ctx._TGTSpeed);

        slidedir = ((ctx.MoveDir().magnitude == 0) ? ctx.transform.forward : ctx.MovementVector());
        ctx._getPCC._drag = ctx._getMoveDragDebug;
        ctx.transform.localScale = new Vector3(1, 0.5f, 1);
    }

    public override void UpdateState()
    {
        if (ctx._TGTSpeed > ctx._slideSpeed + 30)
        {
            //Debug.Log(slideSpeed);
            //Debug.Log( elapsedTime +" "+ ctx.slideNormalizingTime+ " " + percent);
            //slideSpeed = ctx._magnitude * percent + ctx._slideSpeed * (1 - percent);
            elapsedTime += 0.1f;
            percent = elapsedTime / ctx.slideNormalizingTime;
            ctx._TGTSpeed = Mathf.Lerp(ctx._TGTSpeed, ctx._slideSpeed + 20, percent * Time.deltaTime);
        }
            ctx._getPCC.calculateAccelration(ctx._TGTSpeed);
        ctx._moveDirectionX = slidedir.x * ctx._slideSpeed;
        ctx._moveDirectionZ = slidedir.z * ctx._slideSpeed;
        CheckSwitchState();

    }

    public override void FixedState()
    {
        ctx._getPCC.calculateAccelration(ctx._TGTSpeed);
        ctx._getPCC.Move();
    }

    public override void ExitState()
    {
        ctx.transform.localScale = new Vector3(1, 1, 1);
        //slideExit?.Invoke();
        
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
    {   //jump fall idle dash

        //Fall
        if (!ctx._isGrounded)
        {
            SwitchState(factory.Fall());
            return;
        }

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

        ctx._grapple.started += OnActionCanceled;
        ctx._grapple.performed += OnActionPerformed;
    }
}
