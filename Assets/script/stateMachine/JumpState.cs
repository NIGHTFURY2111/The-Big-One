using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class JumpState : BaseState
{
    public static JumpState instance;
    float tgtVelocity;
    bool coroutineFinished;

    public JumpState(PlayerStateMachine ctx, StateFactory factory) : base(ctx, factory)
    {
        instance = this;
    }

    public override void EnterState()
    {
        ctx.Collide += JumpState.instance.wallCollide;
            
        ctx._getPCC.setmaxlinvel(500);

        ctx._getPCC._drag = 2;

        coroutineFinished = false;
                
        ctx._getPCC._gravity = ctx._gravity;

        tgtVelocity = ctx._getPCC._velocityMagnitude;

        ctx.StartCoroutine(Jumping());
    }       

    public override void FixedState()
    {
        if (coroutineFinished)
        {
            ctx._moveDirection = ctx.MovementVector();
            ctx._getPCC.airMove(ctx._forceAppliedInAir);        
        }
    }

    public override void UpdateState()
    {
        if (coroutineFinished) 
        {
            CheckSwitchState(); 
        }
    }

    IEnumerator Jumping()
    {
        ctx._getPCC.jumpForce(ctx._jumpSpeed);

        yield return new WaitForSecondsRealtime(ctx._jumptime);
        CheckSwitchState();
        coroutineFinished = true;
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
        if (ctx._getPCC._getvelocityVector.y < 0f)
        {
            SwitchState(factory.Fall());
            return;
        }

        if (ctx._getPCC.isGrounded() && !ctx._slide.IsPressed())
        {
            //Debug.Log("lmao");
            SwitchState(factory.Idle());
            return;
        }           

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
