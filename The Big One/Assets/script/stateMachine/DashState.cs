using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DashState : BaseState
{
    public DashState(PlayerStateMachine ctx, StateFactory factory) : base(ctx, factory)
    {
    }
    Vector3 DashVector;

    public override void UpdateState()
    {
        ctx.StartCoroutine(Dashing());
    }

    public override void EnterState()
    {
    }

    public override void ExitState()
    {
    }

    IEnumerator Dashing()
    {
        //moveDirection = MovementVector( ) * dashSpeed;
        DashVector = ((ctx.MoveDir().magnitude == 0) ? ctx.transform.forward : ctx.MovementVector()) * ctx._dashSpeed;
        ctx._moveDirectionX = DashVector.x;
        ctx._moveDirectionZ = DashVector.z;
        yield return new WaitForSecondsRealtime(ctx._dashtime);
        CheckSwitchState();
    }

    public override void CheckSwitchState()
    {   //fall/idle

        //fall
        if (!ctx._characterController.isGrounded)
        {
            SwitchState(factory.Fall());
            return;
        }

        //idle (TODO)
        else
        {
            if (ctx._dash.WasPressedThisFrame()) {
                SwitchState(factory.Move());
                    
             }
            if (ctx._move.IsInProgress()){
                SwitchState(factory.Move());
            return;
            }
            else{ 
            SwitchState(factory.Idle());
            return;
            }

        }

    }
}
