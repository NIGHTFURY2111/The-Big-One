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
<<<<<<<< Updated upstream:The Big One/Assets/script/stateMachine/DashState.cs
        ctx.StartCoroutine(Dashing());
========
        ctx._TGTSpeed = ctx._dashSpeed;
        ctx._getPCC.setmaxlinvel(ctx._dashSpeed);
        ctx._getPCC._drag = 10;
        ctx.StartCoroutine(Dashing());  
>>>>>>>> Stashed changes:Assets/script/stateMachine/DashState.cs
    }

    public override void FixedState()
    {
        ctx._getPCC.calculateAccelration(ctx._TGTSpeed);
        ctx._getPCC.dashMove();
    }

    public override void UpdateState()
    {
       
    }

        
    public override void ExitState()
    {
<<<<<<<< Updated upstream:The Big One/Assets/script/stateMachine/DashState.cs
        ctx._moveDirectionX *= ctx._walkingSpeed / ctx._dashSpeed;
        ctx._moveDirectionZ *= ctx._walkingSpeed / ctx._dashSpeed;
========
>>>>>>>> Stashed changes:Assets/script/stateMachine/DashState.cs
        ctx._gravity = gravity;
    }

    IEnumerator Dashing()
    {
<<<<<<<< Updated upstream:The Big One/Assets/script/stateMachine/DashState.cs
        //moveDirection = MovementVector( ) * dashSpeed;
        DashVector = ((ctx.MoveDir().magnitude == 0) ? ctx.transform.forward : ctx.MovementVector()) * ctx._dashSpeed;
========
        DashVector = ((ctx.MoveDir().magnitude == 0) ? ctx.transform.forward : ctx.MovementVector());
>>>>>>>> Stashed changes:Assets/script/stateMachine/DashState.cs
        ctx._moveDirectionX = DashVector.x;
        ctx._moveDirectionZ = DashVector.z;
        ctx._moveDirectionY = 0f;
        ctx._gravity = 0f;

        yield return new WaitForSecondsRealtime(ctx._dashtime);
        CheckSwitchState();
    }

    public override void CheckSwitchState()
<<<<<<<< Updated upstream:The Big One/Assets/script/stateMachine/DashState.cs
    {   //fall/idle

        //fall
        if (!ctx._characterController.isGrounded)
        {
            SwitchState(factory.Fall());
            return;
        }

        else
        {
           
            if (ctx._move.IsPressed()){
                SwitchState(factory.Move());
            return;
            }
        //idle 
            else{ 
            SwitchState(factory.Idle());
            return;
            }

        }

========
    {
        SwitchState(factory.Idle());
        return;
>>>>>>>> Stashed changes:Assets/script/stateMachine/DashState.cs
    }
}
