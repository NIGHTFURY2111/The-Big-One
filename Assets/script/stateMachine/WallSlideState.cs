using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public  class WallSlideState : BaseState
{
    float gavity;
    

    public WallSlideState(PlayerStateMachine ctx, StateFactory factory) : base(ctx, factory)
    {
        gavity = ctx._gravity;
    }


    public override void EnterState()
    {
        ctx._gravity = 0;
        ctx._moveDirectionX = 0f;
        ctx._moveDirectionY = ctx._wallSlideSpeed;
        ctx._moveDirectionZ = 0f;
    }

    public override void UpdateState()
    {
        CheckSwitchState();
    }

    public override void ExitState()
    {
        ctx._gravity = gavity;
       
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
        if(ctx._isGrounded)
        {
            SwitchState(factory.Idle());
            return;
        }
        if(ctx._jump.WasPerformedThisFrame())
        {
            SwitchState(factory.WallJump());
            return;
        }
        ctx._grapple.started += OnActionCanceled;
        ctx._grapple.performed += OnActionPerformed;

    }
   
}
