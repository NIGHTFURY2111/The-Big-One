using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class DashState : BaseState
{
    public DashState(PlayerStateMachine ctx, StateFactory factory) : base(ctx, factory)
    {
        gravity = ctx._gravity;
    }
    Vector3 DashVector;
    float gravity;
    float dashExitMultiplier;

    public override void UpdateState()
    {
        
    }

    public override void EnterState()
    {
        dashExitMultiplier = ctx._walkingSpeed / ctx._dashSpeed;
        ctx.StartCoroutine(Dashing());
    }

    public override void ExitState()
    {
        ctx._moveDirectionX *= dashExitMultiplier;
        ctx._moveDirectionZ *= dashExitMultiplier;
        ctx._gravity = gravity;
    }

    IEnumerator Dashing()
    {
        //moveDirection = MovementVector( ) * dashSpeed;
        DashVector = ((ctx.MoveDir().magnitude == 0) ? ctx.transform.forward : ctx.MovementVector()) * ctx._dashSpeed;
        ctx._moveDirectionX += DashVector.x;
        ctx._moveDirectionZ += DashVector.z;
        ctx._moveDirectionY = 0f;
        ctx._gravity = 0f;
        yield return new WaitForSecondsRealtime(ctx._dashtime);
        CheckSwitchState();
    }

    public override void CheckSwitchState()
    {
        ////fall
        //if (!ctx._characterController.isGrounded)
        //{
        //    SwitchState(factory.Fall());
        //    return;
        //}
        //else
        //{
        //   //move
        //    if (ctx._move.IsPressed()){
        //        SwitchState(factory.Move());
        //        return;
        //    }
        //    //idle 
        //    else
        //    { 
                SwitchState(factory.Idle());
                return;
            //}

        //}

    }
}
