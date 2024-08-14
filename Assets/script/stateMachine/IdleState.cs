using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class IdleState : BaseState
{

    public IdleState(PlayerStateMachine ctx, StateFactory factory) : base(ctx, factory)
    {

    }

    public override void EnterState()
    {
        ctx._moveDirectionX = 0f;
        ctx._moveDirectionY = -0f;
        ctx._moveDirectionZ = 0f;
        ctx._getPCC._drag = 0;
        ctx._TGTSpeed = 0f;
    }

    public override void UpdateState()
    {
        CheckSwitchState();
    }

    public override void ExitState()
    {
       
    }
    private void OnActionCanceled(InputAction.CallbackContext context)
    {
        SwitchState(factory.GrappleStart());
        GrappleStart.instance.held = false;
        Debug.Log("caned");
        return;
    }
    private void OnActionPerformed(InputAction.CallbackContext context)
    {
        SwitchState(factory.GrappleStart());
        GrappleStart.instance.held = true;
        Debug.Log("performed");
        return;
    }
    public override void CheckSwitchState()
    {   //jump walk dash slide
        
        //Dash
        if (ctx._dash.WasPerformedThisFrame())
        {
            SwitchState(factory.Dash());
            return;
        }

        //Move
        if (ctx._move.IsInProgress())
        {
            SwitchState(factory.Move());
            return;
        }

        //Jump
        if (ctx._jump.WasPerformedThisFrame())
        {
            SwitchState(factory.Jump());
            return;
        }

        //Slide
        if (ctx._slide.WasPerformedThisFrame())
        {
            SwitchState(factory.Slide());
            return;
        }

        if (ctx._getPCC._getvelocityVector.y < -0.01f)  
        {
            SwitchState(factory.Fall());
            return;
        }


        ctx._grapple.started += OnActionCanceled;           
        ctx._grapple.performed += OnActionPerformed;

        
    }
}
