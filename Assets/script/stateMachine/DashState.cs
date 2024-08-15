using System;
using System.Collections;
using System.Collections.Generic;
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

    public override void EnterState()
    {
        ctx._TGTSpeed = ctx._getPCC._currentVelocityMagnitude + ctx._dashSpeed;    
        ctx._getPCC.SetMaxlinVel(ctx._TGTSpeed);
        //ctx._getPCC._drag = 10;
        ctx.StartCoroutine(Dashing());  
    }

    public override void FixedState()
    {
        ctx._getPCC.calculateAccelration(ctx._TGTSpeed);
        ctx._getPCC.Move();
    }

    public override void UpdateState()
    {
       
    }

        
    public override void ExitState()
    {
        ctx._gravity = gravity;
        ctx._getPCC.SetvelocityMagnitudeasZero();
    }

    IEnumerator Dashing()
    {
        DashVector = ((ctx.MoveDir().magnitude == 0) ? ctx.transform.forward : ctx.MovementVector());
        ctx._moveDirectionX = DashVector.x;
        ctx._moveDirectionZ = DashVector.z;
        ctx._moveDirectionY = 0f;
        ctx._gravity = 0f;

        yield return new WaitForSecondsRealtime(ctx._dashtime);
        CheckSwitchState();
    }

    public override void CheckSwitchState()
    {   //fall/idle

        //fall
        if (!ctx._isGrounded)
        {
            SwitchState(factory.Fall());
            return;
        }
        else
        {

            if (ctx._move.IsPressed())
            {
                SwitchState(factory.Move());
                return;
            }
            //idle 
            else
            {
                SwitchState(factory.Idle());
                return;
            }

        }
    }

}
